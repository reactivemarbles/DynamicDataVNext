using System.Collections.Generic;

namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of distinct items, which may not be mutated by the consumer, and which publishes notifications about its mutations, as they occur.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public interface IObservableReadOnlySet<T>
    : IObservableReadOnlyCollection<T>,
        IReadOnlySet<T>
{
    /// <inheritdoc cref="IObservableSet{T}.ChangeStream"/>
    DistinctChangeStream<T> ChangeStream { get; }
}
