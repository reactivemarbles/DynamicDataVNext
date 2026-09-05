namespace DynamicDataVNext;

/// <summary>
/// A collection of keyed items, that tracks mutations made to it, and its items, over time, allowing consumers to read and extract them for publication.
/// </summary>
/// <typeparam name="TKey">The type of the keys of items in the collection.</typeparam>
/// <typeparam name="TItem">The type of the items in the collection.</typeparam>
[DebuggerDisplay("Count = {Count}")]
public partial class ChangeTrackingCache<TKey, TItem>
        : ICache<TKey, TItem>,
            IRangeAwareCache<TItem>,
            IRefreshableCache<TKey, TItem>,
            IReadOnlyCache<TKey, TItem>,
            IExpandableCollection
    where TKey : notnull
{
    /// <summary>
    /// Initializes a new empty instance of the <see cref="ChangeTrackingCache{TKey, TItem}"/> class. 
    /// </summary>
    /// <param name="keySelector">The initial value to use for <see cref="KeySelector"/>.</param>
    /// <param name="comparer">The value to use for <see cref="Comparer"/>. Defaults to <see cref="EqualityComparer{T}.Default"/>, when <see langword="null"/> is given.</param>
    /// <param name="options">The value to use for <see cref="Options"/>.</param>
    public ChangeTrackingCache(
            Func<TItem, TKey>           keySelector,
            IEqualityComparer<TKey>?    comparer    = null,
            KeyedItemOptions            options     = default)
        : this(
            itemsByKey:     new(comparer: comparer),
            keySelector:    keySelector ?? throw new ArgumentNullException(nameof(keySelector)),
            options:        options)
    { }

    /// <inheritdoc cref="ChangeTrackingCache{TKey, TItem}(Func{TItem, TKey}, System.Collections.Generic.IEqualityComparer{TKey}, KeyedItemOptions)"/>
    /// <param name="capacity">The initial value to use for <see cref="Capacity"/>.</param>
    public ChangeTrackingCache(
            int                         capacity,
            Func<TItem, TKey>           keySelector,
            IEqualityComparer<TKey>?    comparer    = null,
            KeyedItemOptions            options     = default)
        : this(
            itemsByKey:     new(
                capacity: capacity,
                comparer: comparer),
            keySelector:    keySelector ?? throw new ArgumentNullException(nameof(keySelector)),
            options:        options)
    { }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeTrackingCache{TKey, TItem}"/> class, containing the given items. 
    /// </summary>
    /// <inheritdoc cref="ChangeTrackingCache{TKey, TItem}(Func{TItem, TKey}, System.Collections.Generic.IEqualityComparer{TKey}, KeyedItemOptions)"/>
    /// <param name="items">The initial set of items to be loaded into the collection. Duplicate items are ignored.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    /// <exception cref="ArgumentException">Throws if <paramref name="items"/> contains any items whose key is <see langword="null"/> or duplicated.</exception>
    public ChangeTrackingCache(
            IEnumerable<TItem>          items,
            Func<TItem, TKey>           keySelector,
            IEqualityComparer<TKey>?    comparer    = null,
            KeyedItemOptions            options     = default)
        : this(
            itemsByKey:     ConstructItemsByKey(
                items:          items,
                keySelector:    keySelector ?? throw new ArgumentNullException(nameof(keySelector)),
                comparer:       comparer),
            keySelector:    keySelector,
            options:        options)
    { }
    
    private ChangeTrackingCache(
        Dictionary<TKey, TItem> itemsByKey,
        Func<TItem, TKey>       keySelector,
        KeyedItemOptions        options)
    {
        _itemsByKey         = itemsByKey;
        _keySelector        = keySelector;
        _options            = options;
        _bufferedChanges    = new(sourceCount: itemsByKey.Count);
    }            

    /// <inheritdoc cref="IDictionary{TKey, TValue}.this[TKey]"/>
    public TItem this[TKey key]
        => _itemsByKey[key];

    /// <summary>
    /// The sequence of buffered changes that have recently been made to the collection, and its items.
    /// </summary>
    public BufferedChangeCollection BufferedChanges
        => _bufferedChanges;
    
    /// <inheritdoc/>
    public int Capacity
        => _itemsByKey.Capacity;

    /// <summary>
    /// The comparer to be used for determining whether key values within the collection are equal to each other.
    /// </summary>
    public IEqualityComparer<TKey> Comparer
        => _itemsByKey.Comparer;

    /// <inheritdoc cref="ICache{TKey, TItem}.Count"/>
    public int Count
        => _itemsByKey.Count;

    /// <inheritdoc cref="ICache{TKey, TItem}.Keys"/>
    public Dictionary<TKey, TItem>.KeyCollection Keys
        => _itemsByKey.Keys;

    /// <inheritdoc cref="ICache{TKey, TItem}.KeySelector"/>
    public Func<TItem, TKey> KeySelector
        => _keySelector;

    /// <summary>
    /// A set of options describing the functional nature of the items in the collection.
    /// </summary>
    public KeyedItemOptions Options
        => _options;

    /// <inheritdoc/>
    public void Add(TItem item)
    {
        var key = _keySelector.Invoke(item);
        try
        {
            _itemsByKey.Add(key, item);
        }
        catch (ArgumentException exception) when (exception.ParamName is not nameof(item))
        {
            throw new ArgumentException(
                paramName:      nameof(item),
                message:        exception.Message,
                innerException: exception);
        }

        _bufferedChanges.Add(KeyedChange.CreateAddition(key, item));
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">Throws if <paramref name="items"/> contains any items whose key value is <see langword="null"/>.</exception>
    public void AddRange(IEnumerable<TItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        
        AddRange_Internal(items);
    }

    /// <inheritdoc/>
    public void Clear()
    {
        // Can't clear the collection if it's empty
        if (_itemsByKey.Count is 0)
            return;
        
        // We will add exactly one remove change for each item in the collection.
        var finalPendingChangeCount = _bufferedChanges.Count + _itemsByKey.Count;
        
        _bufferedChanges.EnsureCapacity(finalPendingChangeCount);
        
        foreach (var pair in _itemsByKey)
            _bufferedChanges.Add(KeyedChange.CreateRemoval(pair));
        
        _itemsByKey.Clear();
    }

    /// <inheritdoc cref="ICache{TKey, TItem}.Contains(TItem)"/>
    /// <exception cref="ArgumentException">Throws if the key value of <paramref name="item"/>, as determined by <see cref="KeySelector"/> is <see langword="null"/>.</exception>
    public bool Contains(TItem item)
    {
        var key = _keySelector.Invoke(item);

        TItem? existingItem;
        try
        {
            if (!_itemsByKey.TryGetValue(key, out existingItem))
                return false;
        }
        catch (ArgumentException exception)
        {
            throw new ArgumentException(
                paramName:      nameof(item),
                message:        exception.Message,
                innerException: exception);
        }
        
        return EqualityComparer<TItem>.Default.Equals(item, existingItem);
    }

    /// <inheritdoc cref="ICache{TKey, TItem}.ContainsKey(TKey)"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="key"/>.</exception>
    public bool ContainsKey(TKey key)
        => _itemsByKey.ContainsKey(key);

    /// <inheritdoc/>
    public void CopyTo(TItem[] array, int arrayIndex)
    {
        try
        {
            _itemsByKey.Values.CopyTo(array, arrayIndex);
        }
        catch (ArgumentException exception) when (exception.ParamName is "index")
        {
            throw new ArgumentException(
                paramName:      nameof(arrayIndex),
                message:        exception.Message,
                innerException: exception);
        }
    }

    /// <inheritdoc/>
    public void EnsureCapacity(int capacity)
        => _itemsByKey.EnsureCapacity(capacity);

    /// <inheritdoc cref="ICache{TKey, TItem}.GetEnumerator()"/>
    public Dictionary<TKey, TItem>.ValueCollection.Enumerator GetEnumerator()
        => _itemsByKey.Values.GetEnumerator();

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">Throws if the key value of <paramref name="item"/>, as determined by <see cref="KeySelector"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// If the collection already contains an item for the corresponding key, <see cref="EqualityComparer{T}.Default"/> is used to check if that item is equivalent to the new one. If so, no change is made. 
    /// </remarks>
    public void Merge(TItem item)
    {
        var key = _keySelector.Invoke(item);
    
        if (_itemsByKey.TryGetValue(key, out var priorItem))
        {
            if (EqualityComparer<TItem>.Default.Equals(item, priorItem))
                return;
        
            _itemsByKey[key] = item;
                
            _bufferedChanges.Add(KeyedChange.CreateReplacement(
                key:        key,
                oldItem:    priorItem,
                newItem:    item));
        }
        else
        {
            _itemsByKey.Add(key, item);

            _bufferedChanges.Add(KeyedChange.CreateAddition(key, item));
        }
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">Throws if <paramref name="items"/> contains any items whose key value is <see langword="null"/>.</exception>
    public void MergeRange(IEnumerable<TItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        if (items.TryGetNonEnumeratedCount(out var itemCount))
        {
            if (itemCount is 0)
                return;
        
            _itemsByKey.EnsureCapacity(_itemsByKey.Count + itemCount);
        }
            
        var checkpoint = _bufferedChanges.CreateCheckpoint();
        var priorBufferedChangeCount = _bufferedChanges.Count;
        try
        {
            foreach (var item in items)
            {
                var key = _keySelector.Invoke(item);

                bool wasKeyFound;
                TItem? oldItem;
                try
                {
                    wasKeyFound = _itemsByKey.TryGetValue(key, out oldItem); 
                }
                catch (ArgumentException exception)
                {
                    throw new ArgumentException(
                        paramName:      nameof(items),
                        message:        exception.Message,
                        innerException: exception);
                }

                if (wasKeyFound)
                {
                    if (EqualityComparer<TItem>.Default.Equals(item, oldItem))
                        continue;
                    
                    _itemsByKey[key] = item;
                    
                    _bufferedChanges.Add(KeyedChange.CreateReplacement(
                        key:        key,
                        oldItem:    oldItem!,
                        newItem:    item));
                }
                else
                {
                    _itemsByKey.Add(key, item);

                    _bufferedChanges.Add(KeyedChange.CreateAddition(key, item));
                }
            }
        }
        catch
        {
            // Before we rollback the change buffer, use it to actually undo any adds we actually did, before the error.
            for (var i = _bufferedChanges.Count - 1; i >= priorBufferedChangeCount; --i)
            {
                var change = _bufferedChanges[i];
                
                switch (change.Type)
                {
                    case KeyedChangeType.Addition:
                        _itemsByKey.Remove(change.Key);
                        break;
                        
                    case KeyedChangeType.Replacement:
                        _itemsByKey[change.Key] = change.AsReplacement().OldItem;
                        break;
                }
            }

            checkpoint.Restore();
            throw;
        }
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">Throws if the key value of <paramref name="item"/>, as determined by <see cref="KeySelector"/> is <see langword="null"/>.</exception>
    public bool Refresh(TItem item)
    {
        if (!_options.ItemsAreMutable)
            throw new ImmutableRefreshException();
        
        var key = _keySelector.Invoke(item);
        TItem? existingItem;
        try
        {
            if (!_itemsByKey.TryGetValue(key, out existingItem))
                return false;
        }
        catch (ArgumentNullException exception)
        {
            throw new ArgumentException(
                paramName:      nameof(item),
                message:        exception.Message,
                innerException: exception);
        }

        if (!EqualityComparer<TItem>.Default.Equals(item, existingItem))
            return false;

        _bufferedChanges.Add(KeyedChange.CreateRefreshment(key, existingItem));

        return true;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="key"/>.</exception>
    public bool RefreshKey(TKey key)
    {
        if (!_options.ItemsAreMutable)
            throw new ImmutableRefreshException();
        
        if (!_itemsByKey.TryGetValue(key, out var item))
            return false;

        _bufferedChanges.Add(KeyedChange.CreateRefreshment(key, item));

        return true;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">Throws if the key value of <paramref name="item"/>, as determined by <see cref="KeySelector"/> is <see langword="null"/>.</exception>
    public bool Remove(TItem item)
    {
        var key = _keySelector.Invoke(item);
        
        TItem? existingItem;
        try
        {
            if (!_itemsByKey.TryGetValue(key, out existingItem))
                return false;
        }
        catch (ArgumentNullException exception)
        {
            throw new ArgumentException(
                paramName:      nameof(item),
                message:        exception.Message,
                innerException: exception);
        }

        if (!EqualityComparer<TItem>.Default.Equals(item, existingItem))
            return false;

        _itemsByKey.Remove(key);

        _bufferedChanges.Add(KeyedChange.CreateRemoval(key, existingItem));

        return true;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="key"/>.</exception>
    public bool Remove(             TKey    key,
        [MaybeNullWhen(false)]  out TItem   item)
    {
        if (!_itemsByKey.Remove(key, out item))
            return false;

        _bufferedChanges.Add(KeyedChange.CreateRemoval(key, item));

        return true;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">Throws if <paramref name="items"/> contains any items whose key value, as determined by <see cref="KeySelector"/>, is <see langword="null"/>.</exception>
    public void RemoveRange(IEnumerable<TItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        
        if (items.TryGetNonEnumeratedCount(out var itemCount))
        {
            if (itemCount is 0)
                return;
                
            // We'll be adding at most one change for each item to be removed.
            _bufferedChanges.EnsureCapacity(_bufferedChanges.Count + itemCount);
        }

        var initialBufferedChangeCount = _bufferedChanges.Count;
        var checkpoint = _bufferedChanges.CreateCheckpoint();
        try
        {
            foreach (var item in items)
            {
                var key = _keySelector.Invoke(item);
                
                bool keyWasFound;
                TItem? existingItem;
                try
                {
                    keyWasFound = _itemsByKey.TryGetValue(key, out existingItem);
                }
                catch (ArgumentException exception)
                {
                    throw new ArgumentException(
                        paramName:      nameof(items),
                        message:        exception.Message,
                        innerException: exception);
                }
                
                if (!keyWasFound)
                    continue;
                    
                if (!EqualityComparer<TItem>.Default.Equals(item, existingItem))
                    continue;
                    
                _itemsByKey.Remove(key);
                    
                _bufferedChanges.Add(KeyedChange.CreateRemoval(key, item));
            }
        }
        catch
        {
            // Before rolling back the buffered changes, undo each one.
            for (var i = initialBufferedChangeCount; i < _bufferedChanges.Count; ++i)
            {
                var removal = _bufferedChanges[i].AsRemoval();
            
                _itemsByKey.Add(
                    key:    removal.Key,
                    value:  removal.Item);
            }

            checkpoint.Restore();
            throw;
        }
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">Throws if <paramref name="items"/> contains any items whose key value, as determined by <see cref="KeySelector"/>, is <see langword="null"/>.</exception>
    public void Reset<TItems>(TItems items)
        where TItems : IEnumerable<TItem>
    {
        if (items is null)
            throw new ArgumentNullException(nameof(items));

        // If there's no existing items to remove, this is equivalent to an AddRange().
        if (_itemsByKey.Count is 0)
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
            _itemsByKey.EnsureCapacity(itemCount);
        }

        // We'll be adding a change for each item in the current collection, and each item in the new collection
        // (although we don't know for sure how many the new collection has)
        _bufferedChanges.EnsureCapacity(_bufferedChanges.Count + _itemsByKey.Count + itemCount);

        var checkpoint = _bufferedChanges.CreateCheckpoint();
        var priorBufferedChangeCount = _bufferedChanges.Count; 
        var lastRemovalIndex = _bufferedChanges.Count + _itemsByKey.Count - 1;
        foreach (var pair in _itemsByKey)
            _bufferedChanges.Add(KeyedChange.CreateRemoval(pair));

        _itemsByKey.Clear();

        try
        {
            foreach(var item in items)
            {
                var key = _keySelector.Invoke(item);
                
                try
                {
                    _itemsByKey.Add(key, item);
                }
                catch (ArgumentException exception)
                {
                    throw new ArgumentException(
                        paramName:      nameof(items),
                        message:        exception.Message,
                        innerException: exception);
                }
                
                _bufferedChanges.Add(KeyedChange.CreateAddition(key, item));
            }
        }
        catch
        {
            // Before we rollback the change buffer, use it to put back all the items we removed.
            _itemsByKey.Clear();
            for (var i = priorBufferedChangeCount; i <= lastRemovalIndex; ++i)
            {
                var removal = _bufferedChanges[i].AsRemoval();
                _itemsByKey.Add(removal.Key, removal.Item);
            }
            
            checkpoint.Restore();
            throw;
        }
    }

    /// <inheritdoc cref="ICache{TKey, TItem}.TryGetItem(TKey, out TItem)"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="key"/>.</exception>
    public bool TryGetItem(TKey key, [MaybeNullWhen(false)] out TItem item)
        => _itemsByKey.TryGetValue(key, out item);

    IReadOnlyCollection<TKey> ICache<TKey, TItem>.Keys
        => _itemsByKey.Keys;

    IReadOnlyCollection<TKey> IReadOnlyCache<TKey, TItem>.Keys
        => _itemsByKey.Keys;

    bool ICollection<TItem>.IsReadOnly
        => false;
    
    IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
        => _itemsByKey.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable)_itemsByKey.Values).GetEnumerator();

    private void AddRange_Internal<TItems>(TItems items)
        where TItems : IEnumerable<TItem>
    {
        if (items.TryGetNonEnumeratedCount(out var itemCount))
        {
            if (itemCount is 0)
                return;
        
            _itemsByKey.EnsureCapacity(_itemsByKey.Count + itemCount);
        }
            
        var checkpoint = _bufferedChanges.CreateCheckpoint();
        var priorBufferedChangeCount = _bufferedChanges.Count;
        try
        {
            foreach (var item in items)
            {
                var key = _keySelector.Invoke(item);
            
                try
                {
                    _itemsByKey.Add(key, item);
                }
                catch (ArgumentException exception)
                {
                    throw new ArgumentException(
                        paramName:      nameof(items),
                        message:        exception.Message,
                        innerException: exception);
                }

                _bufferedChanges.Add(KeyedChange.CreateAddition(key, item));
            }
        }
        catch
        {
            // Before we rollback the change buffer, use it to actually undo any adds we actually did, before the error.
            for (var i = priorBufferedChangeCount; i < _bufferedChanges.Count; ++i)
                _itemsByKey.Remove(_bufferedChanges[i].Key);

            checkpoint.Restore();
            throw;
        }
    }

    private static Dictionary<TKey, TItem> ConstructItemsByKey(
        IEnumerable<TItem>          items,
        Func<TItem, TKey>           keySelector,
        IEqualityComparer<TKey>?    comparer)
    {
        ArgumentNullException.ThrowIfNull(items);
    
        var result = items.TryGetNonEnumeratedCount(out var itemCount)
            ? new Dictionary<TKey, TItem>(
                capacity: itemCount,
                comparer: comparer)
            : new Dictionary<TKey, TItem>(comparer: comparer);

        try
        {
            foreach (var item in items)
                result.Add(
                    key:    keySelector.Invoke(item),
                    value:  item);
        }
        catch (ArgumentException exception) when (exception.ParamName is "key")
        {
            throw new ArgumentException(
                paramName:      nameof(items),
                message:        exception.Message,
                innerException: exception);
        }
        
        return result;
    }

    private readonly BufferedChangeCollection   _bufferedChanges;
    private readonly Dictionary<TKey, TItem>    _itemsByKey;
    private readonly Func<TItem, TKey>          _keySelector;
    private readonly KeyedItemOptions           _options;
}
