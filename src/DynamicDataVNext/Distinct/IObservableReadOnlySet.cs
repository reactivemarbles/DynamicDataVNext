namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of distinct items, which may not be mutated by the consumer, and which publishes notifications about mutations made to itself or its items.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public interface IObservableReadOnlySet<T>
    : IObservableReadOnlyCollection<T>,
        IReadOnlySet<T>
{
    /// <inheritdoc cref="IObservableSet{T}.ChangeStream"/>
    DistinctChangeStream<T> ChangeStream { get; }
}
