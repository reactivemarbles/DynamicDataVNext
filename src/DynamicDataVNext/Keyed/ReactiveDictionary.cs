using System.Threading;


namespace DynamicDataVNext;

/// <summary>
/// A collection of items, with distinct keys, that tracks mutations from a given stream, and materializes them for read-only use.
/// </summary>
/// <typeparam name="TKey">The type of the item keys in the collection.</typeparam>
/// <typeparam name="TValue">The type of the item values in the collection.</typeparam>
public sealed class ReactiveDictionary<TKey, TValue>
        : IObservableReadOnlyDictionary<TKey, TValue>,
            IDisposable
    where TKey : notnull
{
    /// <inheritdoc cref="ChangeTrackingDictionary{TKey, TValue}(IEqualityComparer{TKey}, KeyedItemOptions)"/>
    /// <summary>
    /// Initializes a new instance of the <see cref="ReactiveDictionary{TKey, TValue}"/> class, upon a given stream. 
    /// </summary>
    /// <param name="source">The change stream to be materialized.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="source"/>.</exception>
    public ReactiveDictionary(
        IObservable<KeyedChangeSet<TKey, TValue>>   source,
        IEqualityComparer<TKey>?                    comparer    = null,
        KeyedItemOptions                            options     = default)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        _changeStreamSourceSource   = new();
        _collectionChanged          = new();
        _items                      = new(comparer);
        
        _changeStream = new()
        {
            KeyComparer = comparer ?? EqualityComparer<TKey>.Default,
            Options     = options,
            Source      = Signal.Create<KeyedChangeSet<TKey, TValue>>(downstreamObserver =>
            {
                if (_items.Count is not 0)
                    downstreamObserver.OnNext(KeyedChangeSet.CreateForReset(additions: _items));
                
                return _changeStreamSourceSource.SubscribeSafe(downstreamObserver);
            })
        };

        _sourceSubscription = source.SubscribeSafe(Witness.Create<KeyedChangeSet<TKey, TValue>>(
            onNext:         changeSet =>
            {
                if (changeSet.Type is ChangeSetType.Empty)
                    return;

                changeSet.ApplyTo(_items);
                
                _collectionChanged.OnNext(default);
                _changeStreamSourceSource.OnNext(changeSet);
            },
            onError:        error =>
            {
                _collectionChanged.OnError(error);
                _changeStreamSourceSource.OnError(error);
            },
            onCompleted:    () =>
            {
                _collectionChanged.OnCompleted();
                _changeStreamSourceSource.OnCompleted();
            }));
    }
    
    /// <inheritdoc/>
    public TValue this[TKey key]
        => _items[key];
    
    /// <inheritdoc/>
    public KeyedChangeStream<TKey, TValue> ChangeStream
        => _changeStream;

    /// <inheritdoc/>
    public int Count
        => _items.Count;
    
    /// <inheritdoc/>
    public IObservable<RxVoid> CollectionChanged
        => _collectionChanged;

    /// <inheritdoc cref="IObservableReadOnlyDictionary{TKey, TValue}.Keys"/>
    public Dictionary<TKey, TValue>.KeyCollection Keys
        => _items.Keys;    

    /// <inheritdoc cref="IObservableReadOnlyDictionary{TKey, TValue}.Values"/>
    public Dictionary<TKey, TValue>.ValueCollection Values
        => _items.Values;

    /// <inheritdoc/>
    public bool ContainsKey(TKey key)
        => _items.ContainsKey(key);

    /// <inheritdoc/>
    public void Dispose()
    {
        var hasDisposed = Interlocked.Exchange(ref _hasDisposed, true);
        if (hasDisposed)
            return;

        _changeStreamSourceSource   .OnCompleted();
        _collectionChanged          .OnCompleted();
        
        _changeStreamSourceSource   .Dispose();
        _collectionChanged          .Dispose();
        _sourceSubscription         .Dispose();
    }
    
    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator()"/>
    public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        => _items.GetEnumerator();

    /// <inheritdoc/>
    public bool TryGetValue(
                                        TKey    key,
            [MaybeNullWhen(false)]  out TValue  value)
        => _items.TryGetValue(key, out value);

    IReadOnlyCollection<TKey> IObservableReadOnlyDictionary<TKey, TValue>.Keys
        => _items.Keys;    

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        => _items.Keys;    

    IReadOnlyCollection<TValue> IObservableReadOnlyDictionary<TKey, TValue>.Values
        => _items.Values;

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        => _items.Values;

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => _items.GetEnumerator();

    private readonly KeyedChangeStream<TKey, TValue>        _changeStream;
    private readonly Signal<KeyedChangeSet<TKey, TValue>>   _changeStreamSourceSource;
    private readonly Signal<RxVoid>                         _collectionChanged;
    private readonly Dictionary<TKey, TValue>               _items;
    private readonly IDisposable                            _sourceSubscription;
    
    private bool _hasDisposed;
}
