using System;
using System.Collections.Generic;
using System.Reactive;

namespace DynamicDataVNext;

/// <summary>
/// Describes a sorted collection of items, which may not be mutated by the consumer, and which publishes notifications about its mutations to subscribers, as they occur.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public interface IObservableList<T>
    : IReadOnlyList<T>,
        IObservable<SortedChangeSet<T>>
{
    /// <inheritdoc cref="ISubjectList{T}.CollectionChanged"/>
    IObservable<Unit> CollectionChanged { get; }

    /// <inheritdoc cref="ISubjectList{T}.ObserveValue(int)"/>
    IObservable<T> ObserveValue(int index);
}
