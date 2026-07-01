using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DynamicDataVNext;

/// <summary>
/// A collection of distinct items that tracks mutations made to it, and its items, over time, allowing consumers to read and extract them for publication.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
[DebuggerDisplay("Count = {Count}")]
public partial class ChangeTrackingHashSet<T>
    : ISet<T>,
        IReadOnlySet<T>
{
    /// <summary>
    /// Initializes a new empty instance of the <see cref="ChangeTrackingHashSet{T}"/> class. 
    /// </summary>
    /// <param name="comparer">The value to use for <see cref="Comparer"/>. Defaults to <see cref="EqualityComparer{T}.Default"/>, when <see langword="null"/> is given.</param>
    /// <param name="options">The value to use for <see cref="Options"/>.</param>
    public ChangeTrackingHashSet(
            IEqualityComparer<T>?   comparer    = null,
            DistinctItemOptions     options     = default)
        : this(
            items:      new(comparer: comparer),
            options:    options)
    { }

    /// <inheritdoc cref="ChangeTrackingHashSet{T}(System.Collections.Generic.IEqualityComparer{T}, DistinctItemOptions)"/>
    /// <param name="capacity">The initial value to use for <see cref="Capacity"/>.</param>
    public ChangeTrackingHashSet(
            int                     capacity,
            IEqualityComparer<T>?   comparer    = null,
            DistinctItemOptions     options     = default)
        : this(
            items:      new(
                capacity: capacity,
                comparer: comparer),
            options:    options)
    { }
    
    /// <inheritdoc cref="ChangeTrackingHashSet{T}(System.Collections.Generic.IEqualityComparer{T}, DistinctItemOptions)"/>
    /// <param name="items">The initial set of items to be loaded into the collection. Duplicate items are ignored.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    public ChangeTrackingHashSet(
            IEnumerable<T>          items,
            IEqualityComparer<T>?   comparer    = null,
            DistinctItemOptions     options     = default)
        : this(
            items:      new(
                collection: items ?? throw new ArgumentNullException(nameof(items)),
                comparer:   comparer),
            options:    options)
    { }
    
    private ChangeTrackingHashSet(
        HashSet<T>          items,
        DistinctItemOptions options)
    {
        _items              = items;
        _options            = options;
        _bufferedChanges    = new(isSourceEmpty: items.Count is 0);
    }            

    /// <summary>
    /// The sequence of buffered changes that have recently been made to the collection, and its items.
    /// </summary>
    public BufferedChangeCollection BufferedChanges
        => _bufferedChanges;
    
    /// <summary>
    /// The maximum number of items that the collection can store without having to perform an internal resizing re-allocation.
    /// </summary>
    public int Capacity
        => _items.Capacity;
    
    /// <summary>
    /// The comparer to be used for determining whether collection items are equal to each other.
    /// </summary>
    public IEqualityComparer<T> Comparer
        => _items.Comparer;
    
    /// <inheritdoc cref="ISet{T}.Count"/>
    public int Count
        => _items.Count;

    /// <summary>
    /// A set of options describing the functional nature of the items in the collection.
    /// </summary>
    public DistinctItemOptions Options
        => _options;

    /// <inheritdoc/>
    public bool Add(T item)
    {
        if(!_items.Add(item))
            return false;

        _bufferedChanges.Add(new()
        {
            Item = item,
            Type = DistinctChangeType.Addition
        });

        return true;
    }

    /// <inheritdoc/>
    public void Clear()
    {
        // Can't clear the set if it's empty
        if (_items.Count is 0)
            return;

        // We will add exactly one remove change for each item in the set.
        var finalPendingChangeCount = _bufferedChanges.Count + _items.Count;

        _bufferedChanges.EnsureCapacity(finalPendingChangeCount);

        var lastChangeIndex = finalPendingChangeCount - 1;
        foreach (var item in _items)
            _bufferedChanges.Add(
                change:         new()
                {
                    Item = item,
                    Type = DistinctChangeType.Removal
                },
                // The set will be empty upon the last removal
                isSourceEmpty:  _bufferedChanges.Count == lastChangeIndex);

        _items.Clear();
    }

    /// <inheritdoc cref="ISet{T}.Contains(T)"/>
    public bool Contains(T item)
        => _items.Contains(item);

    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex)
        => _items.CopyTo(array, arrayIndex);

    public void EnsureCapacity(int capacity)
        => _items.EnsureCapacity(capacity);
    
    /// <inheritdoc/>
    public void ExceptWith(IEnumerable<T> other)
    {
        ArgumentNullException.ThrowIfNull(other, nameof(other));

        // Can't remove items from the set if it's empty
        if (_items.Count is 0)
            return;

        // We can potentially add one remove change for each item in the set.
        _bufferedChanges.EnsureCapacity(_bufferedChanges.Count + _items.Count);

        foreach (var item in other)
            if (_items.Remove(item))
                _bufferedChanges.Add(
                    change:         new()
                    {
                        Item = item,
                        Type = DistinctChangeType.Removal
                    },
                    // Since we're removing items as we go, we can just check if the set is empty
                    isSourceEmpty:  _items.Count is 0);
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator()
        => _items.GetEnumerator();

    /// <inheritdoc/>
    public void IntersectWith(IEnumerable<T> other)
    {
        ArgumentNullException.ThrowIfNull(other, nameof(other));

        // Can't remove items from the set if it's empty
        if (_items.Count is 0)
            return;

        // We can potentially add one remove change for each item in the set.
        _bufferedChanges.EnsureCapacity(_bufferedChanges.Count + _items.Count);

        // We need to identify items in the set that are not in the other set.
        // Simplest way to do that without repeatedly linear searching through the other set is to move all the
        // items we want to keep to a new inner instance, and whatever's left is what we remove. That gives us, at
        // most, 1 iteration over each collection, and one allocation for the new set.
        // This could probably be optimized further to detect if the other set is actually an ISet<> and use it's
        // .Contains() instead.
        var newItems = new HashSet<T>(capacity: _items.Count);
        foreach (var item in other)
            if (_items.Remove(item))
                newItems.Add(item);
        
        var lastChangeIndex = _bufferedChanges.Count + _items.Count - 1;
        foreach (var item in _items)
            _bufferedChanges.Add(
                change:         new()
                {
                    Item = item,
                    Type = DistinctChangeType.Removal
                },
                // If the new set of items is empty, report it when adding the last remove change
                isSourceEmpty:  (newItems.Count is 0) && (_bufferedChanges.Count == lastChangeIndex));

        _items = newItems;
    }

    /// <inheritdoc cref="ISet{T}.IsProperSubsetOf(IEnumerable{T})"/>
    public bool IsProperSubsetOf(IEnumerable<T> other)
        => _items.IsProperSubsetOf(other);

    /// <inheritdoc cref="ISet{T}.IsProperSupersetOf(IEnumerable{T})"/>
    public bool IsProperSupersetOf(IEnumerable<T> other)
        => _items.IsProperSupersetOf(other);

    /// <inheritdoc cref="ISet{T}.IsSubsetOf(IEnumerable{T})"/>
    public bool IsSubsetOf(IEnumerable<T> other)
        => _items.IsSubsetOf(other);

    /// <inheritdoc cref="ISet{T}.IsSupersetOf(IEnumerable{T})"/>
    public bool IsSupersetOf(IEnumerable<T> other)
        => _items.IsSupersetOf(other);

    /// <inheritdoc cref="ISet{T}.Overlaps(IEnumerable{T})"/>
    public bool Overlaps(IEnumerable<T> other)
        => _items.Overlaps(other);

    /// <summary>
    /// Signals that the given item within the collection has, itself, mutated, triggering a <see cref="DistinctChangeType.Refreshment"/> record to be added to <see cref="BufferedChanges"/>.
    /// </summary>
    /// <param name="item">The item that was refreshed.</param>
    /// <returns><see langword="false"/> if the collection does not actually contain <paramref name="item"/>. Otherwise, <see langword="true"/>.</returns>
    /// <exception cref="ImmutableRefreshException">Throws if <see cref="Options"/>.<see cref="DistinctItemOptions.ItemsAreMutable"/> is <see langword="false"/>.</exception>
    public bool Refresh(T item)
    {
        if (!_options.ItemsAreMutable)
            throw new ImmutableRefreshException();
        
        if (!_items.Contains(item))
            return false;

        _bufferedChanges.Add(
            change:         new()
            {
                Item = item,
                Type = DistinctChangeType.Refreshment
            },
            isSourceEmpty:  _items.Count is 0);

        return true;
    }
    
    /// <inheritdoc/>
    public bool Remove(T item)
    {
        if (!_items.Remove(item))
            return false;

        _bufferedChanges.Add(
            change:         new()
            {
                Item = item,
                Type = DistinctChangeType.Removal
            },
            isSourceEmpty:  _items.Count is 0);

        return true;
    }

    /// <summary>
    /// Performs a <see cref="ChangeSetType.Reset"/> operation upon the collection, by removing any existing items within the collection, and replacing them with the given items. 
    /// </summary>
    /// <param name="items">The new set of items to be loaded into the collection.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    /// <remarks>
    /// Any duplicate items within <paramref name="items"/> are automatically ignored.
    /// </remarks>
    public void Reset(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        if (items.TryGetNonEnumeratedCount(out var itemsCount) && (itemsCount is 0))
        {
            // If there are no new items to add, this is either equivalent to a Clear(), or a NOP.
            if (_items.Count is not 0)
                Clear();

            return;
        }
        
        // If there's no existing items to remove, this is equivalent to AddRange().
        if (_items.Count is 0)
        {
            AddRange(
                items:      items,
                itemsCount: itemsCount);
            return;
        }

        _items.EnsureCapacity(itemsCount);

        // We'll be adding a change for each item in the current set, and each item in the new set
        _bufferedChanges.EnsureCapacity(_bufferedChanges.Count +_items.Count + itemsCount);
        
        var lastRemovalIndex = _bufferedChanges.Count +_items.Count - 1;
        foreach (var item in _items)
            _bufferedChanges.Add(
                change:         new()
                {
                    Item = item,
                    Type = DistinctChangeType.Removal
                },
                // Report the collection as empty upon the last removal
                isSourceEmpty:  _bufferedChanges.Count == lastRemovalIndex);

        _items.Clear();

        foreach(var item in items)
            if (_items.Add(item))
                _bufferedChanges.Add(new()
                {
                    Item = item,
                    Type = DistinctChangeType.Addition
                });
    }

    /// <inheritdoc cref="ISet{T}.SetEquals"/>
    public bool SetEquals(IEnumerable<T> other)
        => _items.SetEquals(other);

    /// <inheritdoc/>
    public void SymmetricExceptWith(IEnumerable<T> other)
    {
        ArgumentNullException.ThrowIfNull(other, nameof(other));

        if(other.TryGetNonEnumeratedCount(out var otherCount))
            // At most, we'll have to house all items in both sets
            _items.EnsureCapacity(_items.Count + otherCount);

        // At most, we'll be adding a change to remove every current item, and add every new item
        var priorPendingChangeCount = _bufferedChanges.Count;
        _bufferedChanges.EnsureCapacity(priorPendingChangeCount + _items.Count + otherCount);

        // As we iterate over the items, we'll cache all the items to be added, instead of adding them right
        // away. That way, we can perform all removes first, which is slightly more optimal for downstream
        // memory usage, and for the possibility of a Reset being produced.
        var itemsToAdd = new List<T>(capacity: otherCount);

        foreach (var item in other)
        {
            if (_items.Remove(item))
                _bufferedChanges.Add(
                    change:         new()
                    {
                        Item = item,
                        Type = DistinctChangeType.Removal
                    },
                    // Since we're removing items one-at-a-time, we can just report whenever a removal leaves the set empty
                    isSourceEmpty:  _items.Count is 0);
            else
                itemsToAdd.Add(item);
        }

        foreach (var item in itemsToAdd)
        {
            _items.Add(item);
            _bufferedChanges.Add(new()
            {
                Item = item,
                Type = DistinctChangeType.Addition
            });
        }
    }

    /// <inheritdoc/>
    public void UnionWith(IEnumerable<T> other)
    {
        ArgumentNullException.ThrowIfNull(other, nameof(other));

        if(other.TryGetNonEnumeratedCount(out var otherCount) && (otherCount is 0))
            return;

        AddRange(
            items:      other,
            itemsCount: otherCount);
    }

    bool ICollection<T>.IsReadOnly
        => false;
    
    void ICollection<T>.Add(T item)
        => Add(item);

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    private void AddRange(
        IEnumerable<T>  items,
        int?            itemsCount)
    {
        if (itemsCount.HasValue)
        {
            _items.EnsureCapacity(_items.Count + itemsCount.Value);

            // We'll be adding a change for every item in the other set
            _bufferedChanges.EnsureCapacity(_bufferedChanges.Count + itemsCount.Value);
        }

        foreach(var item in items)
            if (_items.Add(item))
                 _bufferedChanges.Add(new()
                 {
                     Item = item,
                     Type = DistinctChangeType.Addition
                 });
    }

    private readonly BufferedChangeCollection   _bufferedChanges;
    private readonly DistinctItemOptions        _options;

    private HashSet<T> _items;
}
