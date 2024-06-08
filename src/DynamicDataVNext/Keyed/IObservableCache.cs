using System;
using System.Collections.Generic;
using System.Reactive;

namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of distinctly-keyed items, which may not be mutated by the consumer, and which publishes notifications about its mutations to subscribers, as they occur.
/// </summary>
/// <typeparam name="TKey">The type of the key values of items in the collection.</typeparam>
/// <typeparam name="TItem">The type of the items in the collection.</typeparam>
public interface IObservableCache<TKey, TItem>
    : IReadOnlyCache<TKey, TItem>,
        IObservable<KeyedChangeSet<TKey, TItem>>
{
    /// <inheritdoc cref="ISubjectCache{TKey, TItem}.CollectionChanged"/>
    IObservable<Unit> CollectionChanged { get; }

    /// <inheritdoc cref="ISubjectCache{TKey, TItem}.ObserveValue(TKey)"/>
    IObservable<TItem> ObserveValue(TKey key);
}
