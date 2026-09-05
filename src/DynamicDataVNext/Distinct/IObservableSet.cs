namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of distinct items, which publishes notifications about mutations made to itself or its items.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public interface IObservableSet<T>
    : IObservableCollection<T>,
        IRangeAwareSet<T>,
        IRefreshableSet<T>,
        ISet<T>
{
    /// <summary>
    /// The stream of changes describing mutations made to the collection.
    /// </summary>
    DistinctChangeStream<T> ChangeStream { get; }
}
