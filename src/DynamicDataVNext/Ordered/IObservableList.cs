namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of ordered items, and which publishes notifications about mutations made to itself or its items.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public interface IObservableList<T>
    : IObservableCollection<T>,
        IList<T>,
        IRangeAwareList<T>,
        IMovementAwareList,
        IRefreshableList
{
    /// <summary>
    /// The stream of changes describing mutations made to the collection.
    /// </summary>
    DistinctChangeStream<T> ChangeStream { get; }
}
