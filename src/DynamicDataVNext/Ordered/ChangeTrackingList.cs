using System.Runtime.InteropServices;

namespace DynamicDataVNext;

/// <summary>
/// An ordered collection of  items that tracks mutations made to it, and its items, over time, allowing consumers to read and extract them for publication.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
[DebuggerDisplay("Count = {Count}")]
public partial class ChangeTrackingList<T>
    : IList<T>,
        IReadOnlyList<T>,
        IExpandableCollection
{
    /// <summary>
    /// Initializes a new empty instance of the <see cref="ChangeTrackingList{T}"/> class.
    /// </summary>
    /// <param name="options">The value to use for <see cref="Options"/>.</param>
    public ChangeTrackingList(OrderedItemOptions options = default)
        : this(
            orderedItems:   new(),
            options:        options)
    { }

    /// <inheritdoc cref="ChangeTrackingList{T}(OrderedItemOptions)"/>
    /// <param name="capacity">The initial value to use for <see cref="Capacity"/>.</param>
    public ChangeTrackingList(
            int                 capacity,
            OrderedItemOptions  options     = default)
        : this(
            orderedItems:   new(capacity: capacity),
            options:        options)
    { }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeTrackingList{T}"/> class, containing the given items. 
    /// </summary>
    /// <inheritdoc cref="ChangeTrackingList{T}(OrderedItemOptions)"/>
    /// <param name="items">The initial set of items to be loaded into the collection.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    public ChangeTrackingList(
            IEnumerable<T>      items,
            OrderedItemOptions  options = default)
        : this(
            orderedItems:   new(collection: items ?? throw new ArgumentNullException(nameof(items))),
            options:        options)
    { }
    
    private ChangeTrackingList(
        List<T>             orderedItems,
        OrderedItemOptions  options)
    {
        _orderedItems       = orderedItems;
        _options            = options;
        _bufferedChanges    = new(isSourceEmpty: orderedItems.Count is 0);
    }            

    /// <summary>
    /// The sequence of buffered changes that have recently been made to the collection, and its items.
    /// </summary>
    public BufferedChangeCollection BufferedChanges
        => _bufferedChanges;
    
    /// <inheritdoc/>
    public int Capacity
        => _orderedItems.Capacity;
    
    /// <inheritdoc cref="IList{T}.Count"/>
    public int Count
        => _orderedItems.Count;

    /// <summary>
    /// A set of options describing the functional nature of the items in the collection.
    /// </summary>
    public OrderedItemOptions Options
        => _options;

    // <inheritdoc/>
    public T this[int index]
    {
        get => _orderedItems[index];
        set
        {
            var oldItem = _orderedItems[index];
            
            if (EqualityComparer<T>.Default.Equals(oldItem, value))
                return;

            _orderedItems[index] = value;

            _bufferedChanges.Add(OrderedChange.CreateReplacement(
                index:      index,
                oldItem:    oldItem,
                newItem:    value));
        }
    }

    /// <inheritdoc/>
    public void Add(T item)
    {
        _orderedItems.Add(item);
        
        _bufferedChanges.Add(OrderedChange.CreateInsertion(
            index:  _orderedItems.Count - 1,
            item:   item));
    }

    /// <inheritdoc cref="IObservableList{T}.AddRange(IEnumerable{T})"/>
    public void AddRange(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);
    
        AddRange_Internal(items);
    }

    public void AddRange2(IEnumerable<T> items)
    {
        var priorOrderedItemsCount = _orderedItems.Count;
        
        try
        {
            _orderedItems.AddRange(items);
        }
        catch (Exception exception)
        {
            if (exception is ArgumentNullException)
                throw new ArgumentNullException(nameof(items));
                    
            // List<T>.AddRange() is more efficient than doing individual .Add()s ourselves, but it's not atomic. If an
            // exception occurs during iteration, we need to roll back whatever items were added (or add them to the
            // change buffer, but we're opting for the first option).
            _orderedItems.RemoveRange(
                index: priorOrderedItemsCount,
                count: _orderedItems.Count - priorOrderedItemsCount);
            
            throw;
        }
    
        _bufferedChanges.EnsureCapacity(_bufferedChanges.Count + (_orderedItems.Count - priorOrderedItemsCount));
        for (var i = priorOrderedItemsCount; i < _orderedItems.Count; ++i)
            _bufferedChanges.Add(OrderedChange.CreateInsertion(
                index:  i,
                item:   _orderedItems[i]));
    }

    /// <inheritdoc/>
    public void Clear()
    {
        // Buffer removals in reverse, to avoid internal copies and allocations, within _orderedItems.
        for (var i = _orderedItems.Count - 1; i >= 0; --i)
            _bufferedChanges.Add(
                change:         OrderedChange.CreateRemoval(
                    index:  i,
                    item:   _orderedItems[i]),
                isSourceEmpty:  i == 0);
    
        _orderedItems.Clear();
    }
    
    /// <inheritdoc/>
    public bool Contains(T item)
        => _orderedItems.Contains(item);

    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex)
    {
        try
        {
            _orderedItems.CopyTo(array, arrayIndex);
        }
        catch (ArgumentNullException exception) when (exception.ParamName is "destinationArray")
        {
            throw new ArgumentNullException(nameof(array));
        }
        catch (ArgumentException exception) when (exception.ParamName is "destinationIndex")
        {
            throw new ArgumentException(
                paramName:      nameof(arrayIndex),
                message:        exception.Message,
                innerException: exception);
        }
    }

    /// <inheritdoc/>
    public void EnsureCapacity(int capacity)
        => _orderedItems.EnsureCapacity(capacity);
    
    /// <inheritdoc cref="List{T}.GetEnumerator()"/>
    public List<T>.Enumerator GetEnumerator()
        => _orderedItems.GetEnumerator();

    /// <inheritdoc/>
    public int IndexOf(T item)
        => _orderedItems.IndexOf(item);
        
    /// <inheritdoc/>
    public void Insert(
        int index,
        T   item)
    {
        _orderedItems.Insert(
            index:  index,
            item:   item);
        
        _bufferedChanges.Add(OrderedChange.CreateInsertion(
            index:  index,
            item:   item));
    }

    /// <inheritdoc cref="IObservableList{T}.InsertRange(int, IEnumerable{T})"/>
    public void InsertRange(
        int             index,
        IEnumerable<T>  items)
    {
        // I'd rather use List<T>.InsertRange() to leverage its internal optimizations, but it's not atomic, which we
        // need to be.
        ArgumentNullException.ThrowIfNull(items);
    
        if (items.TryGetNonEnumeratedCount(out var itemsCount))
        {
            _orderedItems.EnsureCapacity(itemsCount);
            _bufferedChanges.EnsureCapacity(_bufferedChanges.Count + itemsCount);
        }
        
        var priorBufferedChangeCount = _bufferedChanges.Count;
        var checkpoint = _bufferedChanges.CreateCheckpoint();
        try
        {
            var insertionIndex = index;
            foreach (var item in items)
            {
                _orderedItems.Insert(
                    index:  insertionIndex,
                    item:   item);
                
                _bufferedChanges.Add(OrderedChange.CreateInsertion(
                    index:  insertionIndex++,
                    item:   item));
            }
        }
        catch
        {
            // Before we rollback the change buffer, use it (well, all we need is its size) to undo the partial changes
            // we already made.
            if (_bufferedChanges.Count != priorBufferedChangeCount)
                _orderedItems.RemoveRange(
                    index: index,
                    count: _bufferedChanges.Count - priorBufferedChangeCount);
        
            checkpoint.Restore();
            
            throw;
        }
    }

    public void InsertRange2(
        int             index,
        IEnumerable<T>  items)
    {
        var priorOrderedItemsCount = _orderedItems.Count;
        try
        {
            _orderedItems.InsertRange(
                index:      index,
                collection: items);
        }
        catch (Exception exception)
        {
            if (exception is ArgumentNullException)
                throw new ArgumentNullException(nameof(items));
                
            // List<T>.InsertRange() is way more efficient than doing individual .Insert()s ourselves, but it's not
            // atomic. If an exception occurs during iteration, we need to roll back whatever items were added (or add
            // them to the change buffer, but we're opting for the first option).
            if (_orderedItems.Count != priorOrderedItemsCount)
                _orderedItems.RemoveRange(
                    index: index,
                    count: _orderedItems.Count - priorOrderedItemsCount);
                
            throw;
        }
    
        var addedItemCount = _orderedItems.Count - priorOrderedItemsCount;
        _bufferedChanges.EnsureCapacity(_bufferedChanges.Count + addedItemCount);
        
        for (var i = index; i < index + addedItemCount; ++i)
            _bufferedChanges.Add(OrderedChange.CreateInsertion(
                index:  i,
                item:   _orderedItems[i]));
    }

    /// <inheritdoc cref="IObservableList{T}.Move(int, int)"/>
    public void Move(
        int oldIndex,
        int newIndex)
    {
        // Intentionally doing this before checking for identical indexes, cause I think it makes sense for this method
        // to throw for invalid indexes, even if they're referring to a pointless move.
        T item;
        try
        {
            item = _orderedItems[oldIndex];
        }
        catch (ArgumentOutOfRangeException exception)
        {
            throw new ArgumentOutOfRangeException(
                message:    exception.Message,
                paramName:  nameof(oldIndex));
        }
        
        if (oldIndex == newIndex)
            return;

        // Remove first, to avoid an internal reallocation, within List<T>
        _orderedItems.RemoveAt(oldIndex);
    
        try
        {
            _orderedItems.Insert(
                index:  newIndex,
                item:   item);
        }
        catch (Exception exception)
        {
            // Make sure and undo the partial change we made
            _orderedItems.Insert(
                index:  oldIndex,
                item:   item);

            if (exception is ArgumentOutOfRangeException)
                throw new ArgumentOutOfRangeException(
                    message:    exception.Message,
                    paramName:  nameof(newIndex));
            
            throw;
        }
    
        _bufferedChanges.Add(OrderedChange.CreateMovement(
            oldIndex:   oldIndex,
            newIndex:   newIndex,
            item:       item));
    }

    public void Move2(
        int oldIndex,
        int newIndex)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(oldIndex, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(newIndex, 0);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(oldIndex, _orderedItems.Count);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(newIndex, _orderedItems.Count);

        if (oldIndex == newIndex)
            return;

        var orderedItems = CollectionsMarshal.AsSpan(_orderedItems);
        var item = orderedItems[oldIndex];
        
        if (oldIndex < newIndex)
            orderedItems[(oldIndex + 1)..(newIndex + 1)].CopyTo(orderedItems[oldIndex..newIndex]);
        else
            orderedItems[newIndex..oldIndex].CopyTo(orderedItems[(newIndex + 1)..(oldIndex + 1)]);

        orderedItems[newIndex] = item;
        
        _bufferedChanges.Add(OrderedChange.CreateMovement(
            oldIndex:   oldIndex,
            newIndex:   newIndex,
            item:       item));
    }

    /// <inheritdoc/>
    public bool Remove(T item)
    {
        var index = _orderedItems.IndexOf(item);
        if (index < 0)
            return false;
        
        _orderedItems.RemoveAt(index);
        
        _bufferedChanges.Add(
            change:         OrderedChange.CreateRemoval(
                index:  index,
                item:   item),
            isSourceEmpty:  _orderedItems.Count is 0);
            
        return true;
    }
    
    /// <inheritdoc/>
    public void RemoveAt(int index)
    {
        var item = _orderedItems[index];
        
        _orderedItems.RemoveAt(index);
        
        _bufferedChanges.Add(
            change:         OrderedChange.CreateRemoval(
                index:  index,
                item:   item),
            isSourceEmpty:  _orderedItems.Count is 0);
    }
    
    /// <summary>
    /// Performs a <see cref="ChangeSetType.Reset"/> operation upon the collection, by removing any existing items within the collection, and replacing them with the given items. 
    /// </summary>
    /// <param name="items">The new set of items to be loaded into the collection.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    public void Reset(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        // If there's no existing items to remove, this is equivalent to an AddRange().
        if (_orderedItems.Count is 0)
        {
            AddRange_Internal(items);
            return;
        }

        if (items.TryGetNonEnumeratedCount(out var itemCount))
        {
            // If there are no new items to add, this is equivalent to a Clear()
            if (itemCount is 0)
            {
                Clear();
                return;
            }
            
            // The final size of the collection will be the new item count. 
            _orderedItems.EnsureCapacity(itemCount);
        }

        // We'll be adding a change for each item in the current collection, and each item in the new collection
        // (although we don't know for sure how many the new collection has)
        _bufferedChanges.EnsureCapacity(_bufferedChanges.Count + _orderedItems.Count + itemCount);

        var checkpoint = _bufferedChanges.CreateCheckpoint();
        var priorBufferedChangeCount = _bufferedChanges.Count; 
        var lastRemovalIndex = _bufferedChanges.Count + _orderedItems.Count - 1;
        // Remove items in reverse order, to eliminate the need for shuffles after each removal.
        for (var i = _orderedItems.Count - 1; i >= 0; --i)
            _bufferedChanges.Add(
                change:         OrderedChange.CreateRemoval(
                    index:  i,
                    item:   _orderedItems[i]),
                // Report the collection as empty upon the last removal
                isSourceEmpty:  _bufferedChanges.Count == lastRemovalIndex);

        _orderedItems.Clear();

        try
        {
            foreach(var item in items)
            {
                _bufferedChanges.Add(OrderedChange.CreateInsertion(
                    index:  _orderedItems.Count,
                    item:   item));

                _orderedItems.Add(item);
            }
        }
        catch
        {
            // Before we rollback the change buffer, use it to put back all the items we removed.
            _orderedItems.Clear();
            
            // We removed items in reverse order, so we have to add them back in reverse order
            for (var i = lastRemovalIndex; i >= priorBufferedChangeCount; --i)
            {
                var removal = _bufferedChanges[i].AsRemoval();
                _orderedItems.Add(removal.Item);
            }
            
            checkpoint.Restore();
            throw;
        }
    }

    bool ICollection<T>.IsReadOnly
        => false;

    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable)_orderedItems).GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
        => _orderedItems.GetEnumerator();

    private void AddRange_Internal(IEnumerable<T> items)
    {
        if (items.TryGetNonEnumeratedCount(out var itemsCount))
        {
            _orderedItems.EnsureCapacity(itemsCount);
            _bufferedChanges.EnsureCapacity(_bufferedChanges.Count + itemsCount);
        }
        
        var priorBufferedChangeCount = _bufferedChanges.Count;
        var checkpoint = _bufferedChanges.CreateCheckpoint();
        try
        {
            foreach (var item in items)
            {
                _bufferedChanges.Add(OrderedChange.CreateInsertion(
                    index:  _orderedItems.Count,
                    item:   item));

                _orderedItems.Add(item);
            }
        }
        catch
        {
            // Before we rollback the change buffer, use it (well, all we need is its size) to undo the partial changes
            // we already made.
            var rollbackCount = _bufferedChanges.Count - priorBufferedChangeCount;
            _orderedItems.RemoveRange(
                index: _orderedItems.Count - rollbackCount,
                count: rollbackCount);
        
            checkpoint.Restore();
            
            throw;
        }
    }

    private readonly BufferedChangeCollection   _bufferedChanges;
    private readonly List<T>                    _orderedItems;
    private readonly OrderedItemOptions         _options;
}
