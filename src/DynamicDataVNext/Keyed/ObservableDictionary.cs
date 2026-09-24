namespace DynamicDataVNext;


/// <summary>
/// A collection of items, with distinct keys, which publishes notifications about mutations made to itself or its items.
/// </summary>
/// <typeparam name="TKey">The type of the item keys in the collection.</typeparam>
/// <typeparam name="TValue">The type of the item values in the collection.</typeparam>
[DebuggerDisplay("Count = {Count}")]
public sealed partial class ObservableDictionary<TKey, TValue>
        : IObservableDictionary<TKey, TValue>,
            IObservableReadOnlyDictionary<TKey, TValue>,
            IExpandableCollection,
            IDisposable
    where TKey : notnull
{
    /// <summary>
    /// Initializes a new empty instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class. 
    /// </summary>
    /// <inheritdoc cref="ChangeTrackingDictionary{TKey, TValue}(IEqualityComparer{TKey}, KeyedItemOptions)"/>
    public ObservableDictionary(
            IEqualityComparer<TKey>?    comparer    = null,
            KeyedItemOptions            options     = default) 
        : this(new(
            comparer:   comparer,
            options:    options))
    { }

    /// <inheritdoc cref="ObservableDictionary{TKey, TValue}(System.Collections.Generic.IEqualityComparer{TKey}, KeyedItemOptions)"/>
    /// <param name="capacity">The initial value to use for <see cref="Capacity"/>.</param>
    public ObservableDictionary(
            int                         capacity,
            IEqualityComparer<TKey>?    comparer    = null,
            KeyedItemOptions            options     = default)
        : this(new(
            capacity:   capacity,
            comparer:   comparer,
            options:    options))
    { }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class, containing the given items. 
    /// </summary>
    /// <inheritdoc cref="ObservableDictionary{TKey, TValue}(System.Collections.Generic.IEqualityComparer{TKey}, KeyedItemOptions)"/>
    /// <param name="items">The initial set of items to be loaded into the collection.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    /// <exception cref="ArgumentException">Throws if <paramref name="items"/> contains any key values that are <see langword="null"/> or duplicated.</exception>
    public ObservableDictionary(
            IEnumerable<KeyValuePair<TKey, TValue>> items,
            IEqualityComparer<TKey>?                comparer    = null,
            KeyedItemOptions                        options     = default)
        : this(new(
            items:      items ?? throw new ArgumentNullException(nameof(items)),
            comparer:   comparer,
            options:    options))
    { }
    
    private ObservableDictionary(ChangeTrackingDictionary<TKey, TValue> items)
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
                        .Prepend(KeyedChangeSet.CreateForReset(additions: _items))
                    : _collectionChangesCaptured)
                .Switch()
        };
    }            

    /// <inheritdoc cref="IDictionary{TKey, TValue}.this"/>
    /// <exception cref="ObjectDisposedException">Throws when setting.</exception>
    public TValue this[TKey key]
    {
        get => _items[key];
        set
        {
            ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

            _items[key] = value;

            PublishNotificationsIfNeeded();
        }
    }

    /// <inheritdoc cref="IObservableCollection{T}.CollectionChanged"/>
    public IObservable<RxVoid> CollectionChanged
        => _collectionChanged;

    /// <inheritdoc/>
    public int Capacity
        => _items.Capacity;
    
    /// <inheritdoc cref="IObservableDictionary{TKey, TValue}.ChangeStream"/>
    public KeyedChangeStream<TKey, TValue> ChangeStream
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

    /// <inheritdoc cref="IObservableDictionary{TKey, TValue}.Keys"/>
    public Dictionary<TKey, TValue>.KeyCollection Keys
        => _items.Keys;

    /// <inheritdoc cref="IObservableDictionary{TKey, TValue}.Values"/>
    public Dictionary<TKey, TValue>.ValueCollection Values
        => _items.Values;

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"></exception>
    public void Add(
        TKey    key,
        TValue  value)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        _items.Add(key, value);

        PublishNotificationsIfNeeded();
    }

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"></exception>
    public void Add(KeyValuePair<TKey, TValue> item)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        _items.Add(item);

        PublishNotificationsIfNeeded();
    }

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"></exception>
    public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        _items.AddRange(items);

        PublishNotificationsIfNeeded();
    }

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"></exception>
    public void AddRange(
        IEnumerable<TValue> values,
        Func<TValue, TKey>  keySelector)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        _items.AddRange(values, keySelector);

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
    public bool Contains(KeyValuePair<TKey, TValue> item)
        => _items.Contains(item);

    /// <inheritdoc cref="IDictionary{TKey, TValue}.ContainsKey"/>
    public bool ContainsKey(TKey key)
        => _items.ContainsKey(key);

    /// <inheritdoc cref="ICollection{T}.CopyTo"/>
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
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
    
    /// <inheritdoc cref="ChangeTrackingDictionary{TKey, TValue}.GetEnumerator()"/>
    public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        => _items.GetEnumerator();

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"></exception>
    public bool Refresh(TKey key)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        var result = _items.Refresh(key);

        PublishNotificationsIfNeeded();

        return result;
    }

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"></exception>
    public bool Remove(TKey key)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        var result = _items.Remove(key);

        PublishNotificationsIfNeeded();

        return result;
    }

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"></exception>
    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        var result = _items.Remove(item);

        PublishNotificationsIfNeeded();

        return result;
    }

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"></exception>
    public void Reset(IEnumerable<KeyValuePair<TKey, TValue>> items)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        _items.Reset(items);

        PublishNotificationsIfNeeded();
    }

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"></exception>
    public void Reset(
        IEnumerable<TValue> values,
        Func<TValue, TKey>  keySelector)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        _items.Reset(values, keySelector);

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

    public bool TryGetValue(
                                        TKey    key,
            [MaybeNullWhen(false)]  out TValue  value)
        => _items.TryGetValue(key, out value);

    IReadOnlyCollection<TKey> IObservableDictionary<TKey, TValue>.Keys
        => _items.Keys;

    IReadOnlyCollection<TKey> IObservableReadOnlyDictionary<TKey, TValue>.Keys
        => _items.Keys;

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        => _items.Keys;

    ICollection<TKey> IDictionary<TKey, TValue>.Keys
        => _items.Keys;

    IReadOnlyCollection<TValue> IObservableDictionary<TKey, TValue>.Values
        => _items.Values;

    IReadOnlyCollection<TValue> IObservableReadOnlyDictionary<TKey, TValue>.Values
        => _items.Values;

    ICollection<TValue> IDictionary<TKey, TValue>.Values
        => _items.Values;

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        => _items.Values;

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        => _items.GetEnumerator();

    IDisposable IObservableCollection<KeyValuePair<TKey, TValue>>.SuspendNotifications()
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
    private readonly KeyedChangeStream<TKey, TValue>        _changeStream;
    private readonly Signal<RxVoid>                         _collectionChanged;
    private readonly Signal<KeyedChangeSet<TKey, TValue>>   _collectionChangesCaptured;
    private readonly ChangeTrackingDictionary<TKey, TValue> _items;

    private bool _hasDisposed;
}
