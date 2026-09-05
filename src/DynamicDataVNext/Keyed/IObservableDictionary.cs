namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of items, with distinct keys, which publishes notifications about mutations made to itself or its items.
/// </summary>
/// <typeparam name="TKey">The type of the item keys in the collection.</typeparam>
/// <typeparam name="TValue">The type of the item values in the collection.</typeparam>
public interface IObservableDictionary<TKey, TValue>
    : IObservableCollection<KeyValuePair<TKey, TValue>>,
        IDictionary<TKey, TValue>,
        IRangeAwareDictionary<TKey, TValue>,
        IRefreshableDictionary<TKey>
{
    /// <summary>
    /// The stream of changes describing mutations made to the collection.
    /// </summary>
    KeyedChangeStream<TKey, TValue> ChangeStream { get; }

    /// <inheritdoc cref="IDictionary{TKey, TValue}.Keys"/>
    new IReadOnlyCollection<TKey> Keys { get; }

    /// <inheritdoc cref="IDictionary{TKey, TValue}.Values"/>
    new IReadOnlyCollection<TValue> Values { get; }
}
