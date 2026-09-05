namespace DynamicDataVNext;

/// <summary>
/// A collection of items, with distinct keys, that tracks mutations made to it, and its items, over time, allowing consumers to read and extract them for publication.
/// </summary>
/// <typeparam name="TKey">The type of the item keys in the collection.</typeparam>
/// <typeparam name="TValue">The type of the item values in the collection.</typeparam>
[DebuggerDisplay("Count = {Count}")]
public partial class ChangeTrackingDictionary<TKey, TValue>
        : IDictionary<TKey, TValue>,
            IRangeAwareDictionary<TKey, TValue>,
            IRefreshableDictionary<TKey>,
            IReadOnlyDictionary<TKey, TValue>,
            IExpandableCollection
    where TKey : notnull
{
    /// <summary>
    /// Initializes a new empty instance of the <see cref="ChangeTrackingDictionary{TKey, TValue}"/> class. 
    /// </summary>
    /// <param name="comparer">The value to use for <see cref="Comparer"/>. Defaults to <see cref="EqualityComparer{T}.Default"/>, when <see langword="null"/> is given.</param>
    /// <param name="options">The value to use for <see cref="Options"/>.</param>
    public ChangeTrackingDictionary(
            IEqualityComparer<TKey>?    comparer    = null,
            KeyedItemOptions            options     = default)
        : this(
            items:      new(comparer: comparer),
            options:    options)
    { }

    /// <inheritdoc cref="ChangeTrackingDictionary{TKey, TValue}(System.Collections.Generic.IEqualityComparer{TKey}, KeyedItemOptions)"/>
    /// <param name="capacity">The initial value to use for <see cref="Capacity"/>.</param>
    public ChangeTrackingDictionary(
            int                         capacity,
            IEqualityComparer<TKey>?    comparer    = null,
            KeyedItemOptions            options     = default)
        : this(
            items:      new(
                capacity: capacity,
                comparer: comparer),
            options:    options)
    { }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeTrackingDictionary{TKey, TValue}"/> class, containing the given items. 
    /// </summary>
    /// <inheritdoc cref="ChangeTrackingDictionary{TKey, TValue}(System.Collections.Generic.IEqualityComparer{TKey}, KeyedItemOptions)"/>
    /// <param name="items">The initial set of items to be loaded into the collection. Duplicate items are ignored.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    /// <exception cref="ArgumentException">Throws if <paramref name="items"/> contains any key values that are <see langword="null"/> or duplicated.</exception>
    public ChangeTrackingDictionary(
            IEnumerable<KeyValuePair<TKey, TValue>> items,
            IEqualityComparer<TKey>?                comparer    = null,
            KeyedItemOptions                        options     = default)
        : this(
            items:      ConstructItems(items, comparer),
            options:    options)
    { }
    
    private ChangeTrackingDictionary(
        Dictionary<TKey, TValue>    items,
        KeyedItemOptions            options)
    {
        _items              = items;
        _options            = options;
        _bufferedChanges    = new(sourceCount: items.Count);
    }            

    /// <inheritdoc cref="IDictionary{TKey, TValue}.this[TKey]"/>
    public TValue this[TKey key]
    {
        get => _items[key];
        set
        {
            if (_items.TryGetValue(key, out var priorValue))
            {
                if (EqualityComparer<TValue>.Default.Equals(value, priorValue))
                    return;
            
                _items[key] = value;
                
                _bufferedChanges.Add(KeyedChange.CreateReplacement(
                    key:        key,
                    oldItem:    priorValue,
                    newItem:    value));
            }
            else
            {
                _items.Add(key, value);

                _bufferedChanges.Add(KeyedChange.CreateAddition(key, value));
            }
        }
    }

    /// <summary>
    /// The sequence of buffered changes that have recently been made to the collection, and its items.
    /// </summary>
    public BufferedChangeCollection BufferedChanges
        => _bufferedChanges;
    
    /// <inheritdoc/>
    public int Capacity
        => _items.Capacity;

    /// <summary>
    /// The comparer to be used for determining whether key values within the collection are equal to each other.
    /// </summary>
    public IEqualityComparer<TKey> Comparer
        => _items.Comparer;

    /// <inheritdoc cref="IDictionary{TKey, TValue}.Count"/>
    public int Count
        => _items.Count;

    /// <inheritdoc cref="IDictionary{TKey, TValue}.Keys"/>
    public Dictionary<TKey, TValue>.KeyCollection Keys
        => _items.Keys;

    /// <summary>
    /// A set of options describing the functional nature of the items in the collection.
    /// </summary>
    public KeyedItemOptions Options
        => _options;

    /// <inheritdoc cref="IDictionary{TKey, TValue}.Values"/>
    public Dictionary<TKey, TValue>.ValueCollection Values
        => _items.Values;

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">Throws if <paramref name="item"/> has a key value of <see langword="null"/> or that already exists in the collection.</exception>
    public void Add(KeyValuePair<TKey, TValue> item)
    {
        try
        {
            _items.Add(item.Key, item.Value);
        }
        catch (ArgumentException exception) when (exception.ParamName is not nameof(item))
        {
            throw new ArgumentException(
                paramName:      nameof(item),
                message:        exception.Message,
                innerException: exception);
        }

        _bufferedChanges.Add(KeyedChange.CreateAddition(item));
    }

    /// <inheritdoc/>
    public void Add(TKey key, TValue value)
    {
        try
        {
            _items.Add(key, value);
        }
        catch (ArgumentException exception) when (exception.ParamName is not nameof(key))
        {
            throw new ArgumentException(
                paramName:      nameof(key),
                message:        exception.Message,
                innerException: exception);
        }

        _bufferedChanges.Add(KeyedChange.CreateAddition(key, value));
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">Throws if <paramref name="items"/> contains any key values that are <see langword="null"/>, duplicated, or already in the collection.</exception>
    public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        
        try
        {
            AddRange_Internal(
                elements:       items,
                keySelector:    static item => item.Key,
                valueSelector:  static item => item.Value);
        }
        catch (ArgumentException exception)
        {
            throw new ArgumentException(
                paramName:      nameof(items),
                message:        exception.Message,
                innerException: exception);
        }
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">Throws if <paramref name="keySelector"/> returns <see langword="null"/>, a duplicated key value, or a key value that already exists within the collection.</exception>
    public void AddRange(
        IEnumerable<TValue> values,
        Func<TValue, TKey>  keySelector)
    {
        ArgumentNullException.ThrowIfNull(values);
        ArgumentNullException.ThrowIfNull(keySelector);
    
        try
        {
            AddRange_Internal(
                elements:       values,
                keySelector:    keySelector,
                valueSelector:  static value => value);
        }
        catch (ArgumentException exception)
        {
            throw new ArgumentException(
                paramName:      nameof(keySelector),
                message:        exception.Message,
                innerException: exception);
        }
    }

    /// <inheritdoc/>
    public void Clear()
    {
        // Can't clear the collection if it's empty
        if (_items.Count is 0)
            return;
        
        // We will add exactly one remove change for each item in the collection.
        _bufferedChanges.EnsureCapacity(_bufferedChanges.Count + _items.Count);
        
        foreach (var item in _items)
            _bufferedChanges.Add(KeyedChange.CreateRemoval(item));
        
        _items.Clear();
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">Throws if <paramref name="item"/> has a key value of <see langword="null"/> or that already exists in the collection.</exception>
    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        try
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)_items).Contains(item);
        }
        catch (ArgumentException exception) when (exception.ParamName is not nameof(item))
        {
            throw new ArgumentException(
                paramName:      nameof(item),
                message:        exception.Message,
                innerException: exception);
        }
    }

    /// <inheritdoc cref="IDictionary{TKey, TValue}.ContainsKey(TKey)"/>
    public bool ContainsKey(TKey key)
        => _items.ContainsKey(key);

    /// <inheritdoc/>
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        try
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)_items).CopyTo(array, arrayIndex);
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
        => _items.EnsureCapacity(capacity);

    /// <inheritdoc cref="IDictionary{TKey, TValue}.GetEnumerator()"/>
    public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        => _items.GetEnumerator();

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="key"/>.</exception>
    public bool Refresh(TKey key)
    {
        if (!_options.ItemsAreMutable)
            throw new ImmutableRefreshException();
        
        if (!_items.TryGetValue(key, out var value))
            return false;

        _bufferedChanges.Add(KeyedChange.CreateRefreshment(key, value));

        return true;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">Throws if <paramref name="item"/> has a key value of <see langword="null"/>.</exception>
    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        try
        {
            if (!((ICollection<KeyValuePair<TKey, TValue>>)_items).Remove(item))
                return false;
        }
        catch (ArgumentNullException exception) when (exception.ParamName is not nameof(item))
        {
            throw new ArgumentException(
                paramName:      nameof(item),
                message:        exception.Message,
                innerException: exception);
        }

        _bufferedChanges.Add(KeyedChange.CreateRemoval(item));

        return true;
    }

    /// <inheritdoc/>
    public bool Remove(TKey key)
    {
        if (!_items.Remove(key, out var value))
            return false;

        _bufferedChanges.Add(KeyedChange.CreateRemoval(key, value));

        return true;
    }

    /// <inheritdoc/>
    public void Reset<TItems>(TItems items)
        where TItems : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        if (items is null)
            throw new ArgumentNullException(nameof(items));

        try
        {
            Reset_Internal<TItems, KeyValuePair<TKey, TValue>>(
                elements:       items,
                keySelector:    static item => item.Key,
                valueSelector:  static item => item.Value);
        }
        catch (ArgumentException exception) when (exception.ParamName is not nameof(items)) 
        {
            throw new ArgumentException(
                paramName:      nameof(items),
                message:        exception.Message,
                innerException: exception);
        }
    }

    /// <inheritdoc/>
    public void Reset<TValues>(
            TValues             values,
            Func<TValue, TKey>  keySelector)
        where TValues : IEnumerable<TValue>
    {
        if (values is null)
            throw new ArgumentNullException(nameof(values));
        ArgumentNullException.ThrowIfNull(keySelector);

        try
        {
            Reset_Internal(
                elements:       values,
                keySelector:    keySelector,
                valueSelector:  static value => value);
        }
        catch (ArgumentException exception) when (exception.ParamName is not nameof(keySelector)) 
        {
            throw new ArgumentException(
                paramName:      nameof(keySelector),
                message:        exception.Message,
                innerException: exception);
        }
    }

    /// <inheritdoc cref="IDictionary{TKey, TValue}.TryGetValue(TKey, out TValue)"/>
    public bool TryGetValue(            TKey    key,
            [MaybeNullWhen(false)]  out TValue  value)
        => _items.TryGetValue(key, out value);

    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        => false;
    
    ICollection<TKey> IDictionary<TKey, TValue>.Keys
        => _items.Keys;

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        => _items.Keys;

    ICollection<TValue> IDictionary<TKey, TValue>.Values
        => _items.Values;

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        => _items.Values;

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        => ((IEnumerable<KeyValuePair<TKey, TValue>>)_items).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable)_items).GetEnumerator();

    private void AddRange_Internal<TElement>(
        IEnumerable<TElement>   elements,
        Func<TElement, TKey>    keySelector,
        Func<TElement, TValue>  valueSelector)
    {
        if (elements.TryGetNonEnumeratedCount(out var elementCount))
        {
            if (elementCount is 0)
                return;
        
            _items.EnsureCapacity(_items.Count + elementCount);
        }
            
        var checkpoint = _bufferedChanges.CreateCheckpoint();
        var priorBufferedChangeCount = _bufferedChanges.Count;
        try
        {
            foreach (var element in elements)
            {
                var key     = keySelector.Invoke(element);
                var value   = valueSelector.Invoke(element);
            
                _items.Add(key, value);
                _bufferedChanges.Add(KeyedChange.CreateAddition(key, value));
            }
        }
        catch
        {
            // Before we rollback the change buffer, use it to actually undo any adds we actually did, before the error.
            for (var i = priorBufferedChangeCount; i < _bufferedChanges.Count; ++i)
                _items.Remove(_bufferedChanges[i].Key);

            checkpoint.Restore();
            throw;
        }
    }

    private void Reset_Internal<TElements, TElement>(
            TElements               elements,
            Func<TElement, TKey>    keySelector,
            Func<TElement, TValue>  valueSelector)
        where TElements : IEnumerable<TElement>
    {
        // If there's no existing items to remove, this is equivalent to an AddRange().
        if (_items.Count is 0)
        {
            AddRange_Internal(
                elements:       elements,
                keySelector:    keySelector,
                valueSelector:  valueSelector);
            return;
        }

        if (elements.TryGetNonEnumeratedCount(out var elementsCount))
        {
            // If there are no new items to add, this is equivalent to a Clear()
            if (elementsCount is 0)
            {
                Clear();
                return;
            }
            
            // The final size of the collection will be the new value count. 
            _items.EnsureCapacity(elementsCount);
        }

        // We'll be adding a change for each item in the current collection, and each item in the new collection
        // (although we don't know for sure how many the new collection has)
        _bufferedChanges.EnsureCapacity(_bufferedChanges.Count + _items.Count + elementsCount);

        var checkpoint = _bufferedChanges.CreateCheckpoint();
        var priorBufferedChangeCount = _bufferedChanges.Count; 
        var lastRemovalIndex = _bufferedChanges.Count + _items.Count - 1;
        foreach (var item in _items)
            _bufferedChanges.Add(KeyedChange.CreateRemoval(item));

        _items.Clear();

        try
        {
            foreach(var element in elements)
            {
                var key     = keySelector   .Invoke(element);
                var value   = valueSelector .Invoke(element);
                
                _items.Add(key, value);
                _bufferedChanges.Add(KeyedChange.CreateAddition(key, value));
            }
        }
        catch
        {
            // Before we rollback the change buffer, use it to put back all the items we removed.
            _items.Clear();
            for (var i = priorBufferedChangeCount; i <= lastRemovalIndex; ++i)
            {
                var removal = _bufferedChanges[i].AsRemoval();
                _items.Add(removal.Key, removal.Item);
            }
            
            checkpoint.Restore();
            throw;
        }
    }

    private static Dictionary<TKey, TValue> ConstructItems(
        IEnumerable<KeyValuePair<TKey, TValue>> items,
        IEqualityComparer<TKey>?                comparer)
    {
        try
        {
            return new(
                collection: items ?? throw new ArgumentNullException(nameof(items)),
                comparer:   comparer);
        }
        catch (ArgumentException exception) when (exception.ParamName is "key")
        {
            throw new ArgumentException(
                paramName:      nameof(items),
                message:        exception.Message,
                innerException: exception);
        }
    }

    private readonly BufferedChangeCollection   _bufferedChanges;
    private readonly Dictionary<TKey, TValue>   _items;
    private readonly KeyedItemOptions           _options;
}
