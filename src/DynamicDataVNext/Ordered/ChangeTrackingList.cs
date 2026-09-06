using System.Runtime.InteropServices;

namespace DynamicDataVNext;

/// <summary>
/// An ordered collection of  items that tracks mutations made to it, and its items, over time, allowing consumers to read and extract them for publication.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
[DebuggerDisplay("Count = {Count}")]
public partial class ChangeTrackingList<T>
    : IList<T>,
        IRangeAwareList<T>,
        IMovementAwareList,
        IRefreshableList,
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
        _bufferedChanges    = new(sourceCount: orderedItems.Count);
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

    // <inheritdoc/>
    public void AddRange(IEnumerable<T> items)
    {
        var priorOrderedItemsCount = _orderedItems.Count;
        
        AddRange_Internal_Perform(
            priorOrderedItemsCount: priorOrderedItemsCount,
            items:                  items);
    
        _bufferedChanges.EnsureCapacity(_bufferedChanges.Count + (_orderedItems.Count - priorOrderedItemsCount));

        AddRange_Internal_BufferChanges(priorOrderedItemsCount);
    }

    /// <inheritdoc/>
    public void Clear()
    {
        // Buffer removals in reverse, to avoid internal copies and allocations within downstream copies, when
        // processing items one-at-a-time.
        _bufferedChanges.EnsureCapacity(_bufferedChanges.Count + _orderedItems.Count);
        for (var i = _orderedItems.Count - 1; i >= 0; --i)
            _bufferedChanges.Add(OrderedChange.CreateRemoval(
                index:  i,
                item:   _orderedItems[i]));
    
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

    // <inheritdoc/>
    public void InsertRange(
        int             index,
        IEnumerable<T>  items)
    {
        var priorOrderedItemsCount = _orderedItems.Count;
        try
        {
            // Benchmarking confirms that using List<T>.InsertRange() instead of individual .Insert()s, and then
            // extracting the items into the change buffer after-the-fact, (I.E. effectively iterating the item range
            // twice) is almost universally better, for both runtime (up to 85% reduction) and memory usage (up to 25%
            // reduction), across a variety of range sizes. Native optimizations within .InsertRange() seem to be good
            // enough to offset the cost of double-iterating.
            _orderedItems.InsertRange(
                index:      index,
                collection: items);
        }
        catch (Exception exception)
        {
            if (exception is ArgumentNullException)
                throw new ArgumentNullException(nameof(items));
                
            // If an exception occurs during iteration, we need to roll back whatever items were added.
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

    // <inheritdoc/>
    public void Move(
        int oldIndex,
        int newIndex)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(oldIndex, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(newIndex, 0);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(oldIndex, _orderedItems.Count);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(newIndex, _orderedItems.Count);

        if (oldIndex == newIndex)
            return;

        // It might seem obvious, but I went ahead and benchmarked the span copying approach below, over just doing a
        // basic remove/insert pair, and it confirms that using span copies like this, rather than a basic remove/insert
        // pair, is almost universally better for runtime (up to 25% reduction). 

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
    public void RefreshAt(int index)
    {
        if (!_options.ItemsAreMutable)
            throw new ImmutableRefreshException();
    
        _bufferedChanges.Add(OrderedChange.CreateRefreshment(
            index:  index,
            item:   _orderedItems[index]));
    }
    
    /// <inheritdoc/>
    public bool Remove(T item)
    {
        var index = _orderedItems.IndexOf(item);
        if (index < 0)
            return false;
        
        _orderedItems.RemoveAt(index);
        
        _bufferedChanges.Add(OrderedChange.CreateRemoval(
            index:  index,
            item:   item));
            
        return true;
    }
    
    /// <inheritdoc/>
    public void RemoveAt(int index)
    {
        var item = _orderedItems[index];
        
        _orderedItems.RemoveAt(index);
        
        _bufferedChanges.Add(OrderedChange.CreateRemoval(
            index:  index,
            item:   item));
    }
    
    /// <inheritdoc/>
    public void RemoveRange(
        int index,
        int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, _orderedItems.Count);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(count, _orderedItems.Count - index);

        if (count is 0)
            return;

        // Benchmarking confirms that using List<T>.RemoveRange() instead of individual .RemoveAt()s, after extracting
        // the items into the change buffer ahead-of-time, (I.E. effectively iterating the item range twice) is almost
        // universally better for runtime (up to 85% reduction) across a variety of range sizes. Native optimizations
        // within .RemoveRange() seem to be good enough to offset the cost of double-iterating.

        _bufferedChanges.EnsureCapacity(_bufferedChanges.Count + count);
        for (var i = index + count - 1; i >= index; --i)
        {
            _bufferedChanges.Add(OrderedChange.CreateRemoval(
                index:  i,
                item:   _orderedItems[i]));
        }

        _orderedItems.RemoveRange(index, count);
    }

    /// <inheritdoc/>
    public void Reset(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        // If there's no existing items to remove, this is equivalent to an AddRange().
        if (_orderedItems.Count is 0)
        {
            AddRange(items);
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
        // (we don't know for sure how many items the new collection has, but the value defaults to 0, which is fine)
        _bufferedChanges.EnsureCapacity(_bufferedChanges.Count + _orderedItems.Count + itemCount);

        var checkpoint = _bufferedChanges.CreateCheckpoint();
        var priorBufferedChangeCount = _bufferedChanges.Count; 
        var lastRemovalIndex = _bufferedChanges.Count + _orderedItems.Count - 1;

        Clear_Internal();

        var priorOrderedItemsCount = _orderedItems.Count;
        
        try
        {
            AddRange_Internal_Perform(
                priorOrderedItemsCount: priorOrderedItemsCount,
                items:                  items);
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

        AddRange_Internal_BufferChanges(priorOrderedItemsCount);
    }

    bool ICollection<T>.IsReadOnly
        => false;

    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable)_orderedItems).GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
        => _orderedItems.GetEnumerator();
        
    // De-duplicated logic for the second-half of an AddRange(), where we retroactively load the added items into the
    // change buffer.
    private void AddRange_Internal_BufferChanges(int priorOrderedItemsCount)
    {
        for (var i = priorOrderedItemsCount; i < _orderedItems.Count; ++i)
            _bufferedChanges.Add(OrderedChange.CreateInsertion(
                index:  i,
                item:   _orderedItems[i]));
    }

    // De-duplicated logic for the first-half of an AddRange(), which passes off to List<T>.AddRange(), with rollback
    // logic.
    private void AddRange_Internal_Perform(
        int             priorOrderedItemsCount,
        IEnumerable<T>  items)
    {
        try
        {
            // Benchmarking confirms that using List<T>.AddRange() instead of individual .Add()s, and then extracting
            // the items into the change buffer after-the-fact, (I.E. effectively iterating the item range twice) is
            // almost universally better, for both runtime (up to 20% reduction) and memory usage (up to 25% reduction),
            // across a variety of range sizes. Native optimizations within .AddRange() seem to be good enough to offset
            // the cost of double-iterating.
            _orderedItems.AddRange(items);
        }
        catch (Exception exception)
        {
            if (exception is ArgumentNullException)
                throw new ArgumentNullException(nameof(items));
                    
            // If an exception occurs during iteration, we need to roll back whatever items were added.
            _orderedItems.RemoveRange(
                index: priorOrderedItemsCount,
                count: _orderedItems.Count - priorOrderedItemsCount);
            
            throw;
        }
    }

    // De-duplicated logic for a Clear()
    private void Clear_Internal()
    {
        // Buffer removals in reverse, to avoid internal copies and allocations within downstream copies, when
        // processing items one-at-a-time.
        for (var i = _orderedItems.Count - 1; i >= 0; --i)
            _bufferedChanges.Add(OrderedChange.CreateRemoval(
                index:  i,
                item:   _orderedItems[i]));
    
        _orderedItems.Clear();
    }

    private readonly BufferedChangeCollection   _bufferedChanges;
    private readonly List<T>                    _orderedItems;
    private readonly OrderedItemOptions         _options;
}
