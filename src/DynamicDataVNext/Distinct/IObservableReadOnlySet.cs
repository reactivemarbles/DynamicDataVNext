using System;
using System.Collections.Generic;
using System.Reactive;

namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of distinct items, which may not be mutated by the consumer, and which publishes notifications about its mutations to subscribers, as they occur.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public interface IObservableReadOnlySet<T>
    : IReadOnlySet<T>,
        IObservable<DistinctChangeSet<T>>
{
    /// <inheritdoc cref="IObservableSet{T}.CollectionChanged"/>
    IObservable<Unit> CollectionChanged { get; }
}
