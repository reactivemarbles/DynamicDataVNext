namespace DynamicDataVNext;

/// <summary>
/// A collection of items, with distinct keys, which publishes notifications about mutations made to itself or its items.
/// </summary>
/// <typeparam name="TKey">The type of the key values in the collection.</typeparam>
/// <typeparam name="TItem">The type of the items in the collection.</typeparam>
[DebuggerDisplay("Count = {Count}")]
public sealed partial class ObservableCache<TKey, TItem>
        : IObservableCache<TKey, TItem>,
            IObservableReadOnlyCache<TKey, TItem>,
            IExpandableCollection,
            IDisposable
    where TKey : notnull
{
    /// <summary>
    /// Initializes a new empty instance of the <see cref="ObservableCache{TKey, TItem}"/> class. 
    /// </summary>
    /// <inheritdoc cref="ChangeTrackingCache{TKey, TItem}(Func{TItem, TKey}, IEqualityComparer{TKey}, KeyedItemOptions)"/>
    public ObservableCache(
            Func<TItem, TKey>           keySelector,
            IEqualityComparer<TKey>?    comparer    = null,
            KeyedItemOptions            options     = default) 
        : this(new(
            keySelector:    keySelector,
            comparer:       comparer,
            options:        options))
    { }

    /// <inheritdoc cref="ChangeTrackingCache{TKey, TItem}(System.Collections.Generic.IEqualityComparer{TKey}, KeyedItemOptions)"/>
    /// <param name="capacity">The initial value to use for <see cref="Capacity"/>.</param>
    public ObservableCache(
            int                         capacity,
            Func<TItem, TKey>           keySelector,
            IEqualityComparer<TKey>?    comparer    = null,
            KeyedItemOptions            options     = default)
        : this(new(
            capacity:       capacity,
            keySelector:    keySelector,
            comparer:       comparer,
            options:        options))
    { }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ObservableCache{TKey, TItem}"/> class, containing the given items. 
    /// </summary>
    /// <inheritdoc cref="ChangeTrackingCache{TKey, TItem}(Func{TItem, TKey}, IEqualityComparer{TKey}, KeyedItemOptions)"/>
    /// <param name="items">The initial set of items to be loaded into the collection.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    /// <exception cref="ArgumentException">Throws if <paramref name="items"/> contains any key values that are <see langword="null"/> or duplicated.</exception>
    public ObservableCache(
            IEnumerable<TItem>          items,
            Func<TItem, TKey>           keySelector,
            IEqualityComparer<TKey>?    comparer    = null,
            KeyedItemOptions            options     = default)
        : this(new(
            items:          items,
            keySelector:    keySelector,
            comparer:       comparer,
            options:        options))
    { }
    
    private ObservableCache(ChangeTrackingCache<TKey, TItem> items)
    {
        _areNotificationsSuspended  = new(false);
        _collectionChanged          = new();
        _collectionChangesCaptured  = new();
        _items                      = items;

        _changeStream = new()
        {
            KeyComparer = items.Comparer,
            Options     = items.Options,
            Source      = _areNotificationsSuspended
                .SkipWhile(areNotificationsSuspended => areNotificationsSuspended)
                .Take(1)
                .Select(_ => (_items.Count is not 0)
                    ? _collectionChangesCaptured
                        .Prepend(KeyedChangeSet.CreateForReset(additions: _items.KeyValuePairs))
                    : _collectionChangesCaptured)
                .Switch()
        };
    }            

    /// <inheritdoc cref="ICache{TKey, TItem}.this"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="key"/>.</exception>
    public TItem this[TKey key]
        => _items[key];

    /// <inheritdoc cref="IObservableCollection{T}.CollectionChanged"/>
    public IObservable<RxVoid> CollectionChanged
        => _collectionChanged;

    /// <inheritdoc/>
    public int Capacity
        => _items.Capacity;
    
    /// <inheritdoc cref="IObservableCache{TKey, TItem}.ChangeStream"/>
    public KeyedChangeStream<TKey, TItem> ChangeStream
        => _changeStream;

    /// <inheritdoc cref="ICollection{T}.Count"/>
    public int Count
        => _items.Count;

    /// <summary>
    /// A flag indicating whether the collection can currently be mutated.
    /// </summary>
    /// <remarks>
    /// <see langword="true"/> after <see cref="Dispose"/> has been called. <see langword="false"/> otherwise.
    /// </remarks>
    public bool IsReadOnly
        => _hasDisposed;

    /// <inheritdoc cref="ICache{TKey, TItem}.Keys"/>
    public Dictionary<TKey, TItem>.KeyCollection Keys
        => _items.Keys;

    /// <inheritdoc cref="ICache{TKey, TItem}.KeyedItems"/>
    public KeyedItemCollection<TKey, TItem> KeyedItems
        => _items.KeyedItems;

    /// <inheritdoc cref="ICache{TKey, TItem}.KeyValuePairs"/>
    public KeyValuePairCollection<TKey, TItem> KeyValuePairs
        => _items.KeyValuePairs;

    /// <inheritdoc cref="ICache{TKey, TItem}.KeySelector"/>
    public Func<TItem, TKey> KeySelector
        => _items.KeySelector;

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">Throws if the key value of <paramref name="item"/>, as determined by <see cref="KeySelector"/> is <see langword="null"/>.</exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public void Add(TItem item)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        _items.Add(item);

        PublishNotificationsIfNeeded();
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">Throws if <paramref name="items"/> contains any items whose key value is <see langword="null"/>.</exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public void AddRange(IEnumerable<TItem> items)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        _items.AddRange(items);

        PublishNotificationsIfNeeded();
    }

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"></exception>
    public void Clear()
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        _items.Clear();

        PublishNotificationsIfNeeded();
    }

    /// <inheritdoc cref="ICollection{T}.Contains"/>
    /// <exception cref="ArgumentException">Throws if the key value of <paramref name="item"/>, as determined by <see cref="KeySelector"/> is <see langword="null"/>.</exception>
    public bool Contains(TItem item)
        => _items.Contains(item);

    /// <inheritdoc cref="ICache{TKey, TItem}.ContainsKey"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="key"/>.</exception>
    public bool ContainsKey(TKey key)
        => _items.ContainsKey(key);

    /// <inheritdoc cref="ICollection{T}.CopyTo"/>
    public void CopyTo(TItem[] array, int arrayIndex)
        => _items.CopyTo(array, arrayIndex);

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_hasDisposed)
            return;
        _hasDisposed = true;

        _areNotificationsSuspended  .OnCompleted();
        _collectionChanged          .OnCompleted();
        _collectionChangesCaptured  .OnCompleted();

        _areNotificationsSuspended  .Dispose();
        _collectionChangesCaptured  .Dispose();
        _collectionChanged          .Dispose();
    }

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"></exception>
    public void EnsureCapacity(int capacity)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        _items.EnsureCapacity(capacity); 
    }
    
    /// <inheritdoc cref="ChangeTrackingCache{TKey, TItem}.GetEnumerator()"/>
    public Dictionary<TKey, TItem>.ValueCollection.Enumerator GetEnumerator()
        => _items.GetEnumerator();

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">Throws if the key value of <paramref name="item"/>, as determined by <see cref="KeySelector"/> is <see langword="null"/>.</exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public void Merge(TItem item)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        _items.Merge(item);

        PublishNotificationsIfNeeded();
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">Throws if <paramref name="items"/> contains any items whose key value is <see langword="null"/>.</exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public void MergeRange(IEnumerable<TItem> items)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        _items.MergeRange(items);

        PublishNotificationsIfNeeded();
    }
    
    /// <inheritdoc/>
    /// <exception cref="ArgumentException">Throws if the key value of <paramref name="item"/>, as determined by <see cref="KeySelector"/> is <see langword="null"/>.</exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public bool Refresh(TItem item)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        var result = _items.Refresh(item);

        PublishNotificationsIfNeeded();

        return result;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="key"/>.</exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public bool RefreshKey(TKey key)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        var result = _items.RefreshKey(key);

        PublishNotificationsIfNeeded();

        return result;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">Throws if the key value of <paramref name="item"/>, as determined by <see cref="KeySelector"/> is <see langword="null"/>.</exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public bool Remove(TItem item)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        var result = _items.Remove(item);

        PublishNotificationsIfNeeded();

        return result;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="key"/>.</exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public bool Remove(             TKey    key,
        [MaybeNullWhen(false)]  out TItem   item)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        var result = _items.Remove(key, out item);

        PublishNotificationsIfNeeded();

        return result;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">Throws if <paramref name="items"/> contains any items whose key value, as determined by <see cref="KeySelector"/>, is <see langword="null"/>.</exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public void RemoveRange(IEnumerable<TItem> items)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        _items.RemoveRange(items);

        PublishNotificationsIfNeeded();
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">Throws if <paramref name="items"/> contains any items whose key value, as determined by <see cref="KeySelector"/>, is <see langword="null"/>.</exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public void Reset(IEnumerable<TItem> items)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        _items.Reset(items);

        PublishNotificationsIfNeeded();
    }

    /// <inheritdoc cref="IObservableCollection{T}.SuspendNotifications"/>
    /// <exception cref="ObjectDisposedException"></exception>
    public Suspension SuspendNotifications()
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        if (_areNotificationsSuspended.Value)
            throw new InvalidOperationException("Notifications are already suspended");
        _areNotificationsSuspended.OnNext(true);

        return new(this);
    }

    /// <inheritdoc cref="ICache{TKey, TItem}.TryGetItem(TKey, out TItem)"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="key"/>.</exception>
    public bool TryGetItem(             TKey    key,
            [MaybeNullWhen(false)]  out TItem   item)
        => _items.TryGetItem(key, out item);

    IReadOnlyCollection<TKey> ICache<TKey, TItem>.Keys
        => _items.Keys;

    IReadOnlyCollection<TKey> IReadOnlyCache<TKey, TItem>.Keys
        => _items.Keys;

    IReadOnlyCollection<KeyedItem<TKey, TItem>> ICache<TKey, TItem>.KeyedItems
        => _items.KeyedItems;

    IReadOnlyCollection<KeyedItem<TKey, TItem>> IReadOnlyCache<TKey, TItem>.KeyedItems
        => _items.KeyedItems;

    IReadOnlyCollection<KeyValuePair<TKey, TItem>> ICache<TKey, TItem>.KeyValuePairs
        => _items.KeyValuePairs;

    IReadOnlyCollection<KeyValuePair<TKey, TItem>> IReadOnlyCache<TKey, TItem>.KeyValuePairs
        => _items.KeyValuePairs;

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
        => _items.GetEnumerator();

    IDisposable IObservableCollection<TItem>.SuspendNotifications()
        => SuspendNotifications();

    private void PublishNotificationsIfNeeded()
    {
        if (_areNotificationsSuspended.Value)
            return;

        var changes = _items.BufferedChanges.CaptureAndClear();
        if (changes.Type is ChangeSetType.Empty)
            return;

        _collectionChangesCaptured.OnNext(changes);
        _collectionChanged.OnNext(default);
    }

    private readonly StateSignal<bool>                      _areNotificationsSuspended;
    private readonly KeyedChangeStream<TKey, TItem>         _changeStream;
    private readonly Signal<RxVoid>                         _collectionChanged;
    private readonly Signal<KeyedChangeSet<TKey, TItem>>    _collectionChangesCaptured;
    private readonly ChangeTrackingCache<TKey, TItem>       _items;

    private bool _hasDisposed;
}
