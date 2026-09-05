namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of keyed items, which publishes notifications about mutations made to itself or its items.
/// </summary>
/// <typeparam name="TKey">The type of the key values in the collection.</typeparam>
/// <typeparam name="TItem">The type of the items in the collection.</typeparam>
public interface IObservableCache<TKey, TItem>
    : IObservableCollection<TItem>,
        ICache<TKey, TItem>,
        IRangeAwareCache<TItem>,
        IRefreshableCache<TKey, TItem>
{
    /// <summary>
    /// The stream of changes describing mutations made to the collection.
    /// </summary>
    KeyedChangeStream<TKey, TItem> ChangeStream { get; }
}
