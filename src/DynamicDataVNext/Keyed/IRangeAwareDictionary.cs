namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of items, with distinct keys, which supports various atomic operations upon ranges of items.
/// </summary>
/// <typeparam name="TKey">The type of the item keys in the collection.</typeparam>
/// <typeparam name="TValue">The type of the item values in the collection.</typeparam>
public interface IRangeAwareDictionary<TKey, TValue>
{
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
    /// Performs a <see cref="ChangeSetType.Reset"/> operation upon the collection, by removing any existing items within the collection, and replacing them with the given items. 
    /// </summary>
    /// <typeparam name="TItems">The type of the <paramref name="items"/> sequence.</typeparam>
    /// <param name="items">The new set of items to be loaded into the collection.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    void Reset<TItems>(TItems items)
        where TItems : IEnumerable<KeyValuePair<TKey, TValue>>;

    /// <summary>
    /// Performs a <see cref="ChangeSetType.Reset"/> operation upon the collection, by removing any existing items within the collection, and replacing them with the given items. 
    /// </summary>
    /// <typeparam name="TValues">The type of the <paramref name="values"/> sequence.</typeparam>
    /// <param name="values">The values to use as <see cref="KeyValuePair{TKey, TValue}.Value"/> for the new set of items to be loaded into the collection.</param>
    /// <param name="keySelector">A selector to select a <see cref="KeyValuePair{TKey, TValue}.Key"/> value for each new item.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="values"/> and <paramref name="keySelector"/>.</exception>
    void Reset<TValues>(
            TValues             values,
            Func<TValue, TKey>  keySelector)
        where TValues : IEnumerable<TValue>;
}
