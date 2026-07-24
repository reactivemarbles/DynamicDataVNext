namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of keyed items, which may not be mutated by the consumer, and which publishes notifications about mutations made to itself or its items.
/// </summary>
/// <typeparam name="TKey">The type of the key values in the collection.</typeparam>
/// <typeparam name="TItem">The type of the items in the collection.</typeparam>
public interface IObservableReadOnlyCache<TKey, TItem>
    : IObservableReadOnlyCollection<TItem>,
        IReadOnlyCache<TKey, TItem>
{
    /// <inheritdoc cref="IObservableCache{TKey, TItem}.ChangeStream"/>
    KeyedChangeStream<TKey, TItem> ChangeStream { get; }
}
