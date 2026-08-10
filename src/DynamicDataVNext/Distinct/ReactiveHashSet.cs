using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using ReactiveUI.Primitives;
using ReactiveUI.Primitives.Advanced;
using ReactiveUI.Primitives.Signals;

namespace DynamicDataVNext;

/// <summary>
/// Defines a collection of distinct items, which tracks mutations from a given stream, and materializes them for read-only use..
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public sealed class ReactiveHashSet<T>
    : IObservableReadOnlySet<T>,
        IDisposable
{
    /// <inheritdoc cref="ChangeTrackingHashSet{T}(IEqualityComparer{T}, DistinctItemOptions)"/>
    /// <summary>
    /// Initializes a new instance of the <see cref="ReactiveHashSet{T}"/> class, upon a given stream. 
    /// </summary>
    /// <param name="source">The change stream to be materialized.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="source"/>.</exception>
    public ReactiveHashSet(
        IObservable<DistinctChangeSet<T>>   source,
        IEqualityComparer<T>?               comparer    = null,
        DistinctItemOptions                 options     = default)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        _changeStreamSourceSource   = new();
        _collectionChanged          = new();
        _items                      = new(comparer);
        
        _changeStream = new()
        {
            Comparer    = comparer ?? EqualityComparer<T>.Default,
            Options     = options,
            Source      = Signal.Create<DistinctChangeSet<T>>(downstreamObserver =>
            {
                downstreamObserver.OnNext(DistinctChangeSet.CreateForReset(addedItems: _items));
                
                return _changeStreamSourceSource.SubscribeSafe(downstreamObserver);
            })
        };

        _sourceSubscription = source.SubscribeSafe(Witness.Create<DistinctChangeSet<T>>(
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
    public DistinctChangeStream<T> ChangeStream
        => _changeStream;

    /// <inheritdoc/>
    public int Count
        => _items.Count;
    
    /// <inheritdoc/>
    public IObservable<RxVoid> CollectionChanged
        => _collectionChanged;
    
    /// <inheritdoc/>
    public bool Contains(T item)
        => _items.Contains(item);

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
    public HashSet<T>.Enumerator GetEnumerator()
        => _items.GetEnumerator();
    
    /// <inheritdoc/>
    public bool IsProperSubsetOf(IEnumerable<T> other)
        => _items.IsProperSubsetOf(other);

    /// <inheritdoc/>
    public bool IsProperSupersetOf(IEnumerable<T> other)
        => _items.IsProperSupersetOf(other);

    /// <inheritdoc/>
    public bool IsSubsetOf(IEnumerable<T> other)
        => _items.IsSubsetOf(other);

    /// <inheritdoc/>
    public bool IsSupersetOf(IEnumerable<T> other)
        => _items.IsSupersetOf(other);

    /// <inheritdoc/>
    public bool Overlaps(IEnumerable<T> other)
        => _items.Overlaps(other);

    /// <inheritdoc/>
    public bool SetEquals(IEnumerable<T> other)
        => _items.SetEquals(other);

    /// <inheritdoc/>
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
        => _items.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
        => _items.GetEnumerator();

    private readonly DistinctChangeStream<T>        _changeStream;
    private readonly Signal<DistinctChangeSet<T>>   _changeStreamSourceSource;
    private readonly Signal<RxVoid>                 _collectionChanged;
    private readonly HashSet<T>                     _items;
    private readonly IDisposable                    _sourceSubscription;
    
    private bool _hasDisposed;
}
