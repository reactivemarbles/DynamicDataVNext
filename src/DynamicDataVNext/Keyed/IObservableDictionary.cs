namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of items, with distinct keys, which publishes notifications about mutations made to itself or its items.
/// </summary>
/// <typeparam name="TKey">The type of the item keys in the collection.</typeparam>
/// <typeparam name="TValue">The type of the item values in the collection.</typeparam>
public interface IObservableDictionary<TKey, TValue>
    : IObservableCollection<KeyValuePair<TKey, TValue>>,
        IDictionary<TKey, TValue>
{
    /// <summary>
    /// The stream of changes describing mutations made to the collection.
    /// </summary>
    KeyedChangeStream<TKey, TValue> ChangeStream { get; }

    /// <inheritdoc cref="IDictionary{TKey, TValue}.Keys"/>
    new IReadOnlyCollection<TKey> Keys { get; }

    /// <inheritdoc cref="IDictionary{TKey, TValue}.Values"/>
    new IReadOnlyCollection<TValue> Values { get; }

    /// <summary>
    /// Adds a range of items to the collection, as a single operation.
    /// </summary>
    /// <param name="items">The key and value pairings to be added to the collection.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    /// <exception cref="ArgumentException">Throws if <paramref name="items"/> contains any key values that already exist within the collection.</exception>
    void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items);

    /// <summary>
    /// Adds a range of items to the collection, as a single operation.
    /// </summary>
    /// <param name="values">The values to use as <see cref="KeyValuePair{TKey, TValue}.Value"/> for each new item.</param>
    /// <param name="keySelector">A selector to select a <see cref="KeyValuePair{TKey, TValue}.Key"/> value for each new item.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="keySelector"/>.</exception>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="values"/> and <paramref name="keySelector"/>.</exception>
    /// <exception cref="ArgumentException">Throws if <paramref name="keySelector"/> returns a key that already exists within the collection.</exception>
    void AddRange(
        IEnumerable<TValue> values,
        Func<TValue, TKey>  keySelector);

    /// <summary>
    /// Signals that an item within the collection has, itself, mutated, triggering a <see cref="KeyedChangeType.Refreshment"/> notification to be published via <see cref="ChangeStream"/>.
    /// </summary>
    /// <param name="key">The key of the item that was mutated.</param>
    /// <returns><see langword="false"/> if the collection does not actually contain <paramref name="key"/>. Otherwise, <see langword="true"/>.</returns>
    bool Refresh(TKey key);

    /// <summary>
    /// Performs a <see cref="ChangeSetType.Reset"/> operation upon the collection, by removing any existing items within the collection, and replacing them with the given items. 
    /// </summary>
    /// <param name="values">The values to use as <see cref="KeyValuePair{TKey, TValue}.Value"/> for the new set of items to be loaded into the collection.</param>
    /// <param name="keySelector">A selector to select a <see cref="KeyValuePair{TKey, TValue}.Key"/> value for each new item.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="values"/> and <paramref name="keySelector"/>.</exception>
    void Reset(
        IEnumerable<TValue> values,
        Func<TValue, TKey>  keySelector);
}
