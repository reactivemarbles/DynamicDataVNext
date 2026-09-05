namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of items, with distinct keys, which can report on mutations made to its items.
/// </summary>
/// <typeparam name="TKey">The type of the item keys in the collection.</typeparam>
public interface IRefreshableDictionary<in TKey>
{
    /// <summary>
    /// Signals that an item within the collection has been externally mutated.
    /// </summary>
    /// <param name="key">The key of the item that was mutated.</param>
    /// <returns><see langword="false"/> if the collection does not actually contain <paramref name="key"/>. Otherwise, <see langword="true"/>.</returns>
    /// <exception cref="ImmutableRefreshException">Throws if the items in the collection are immutable. This is generally the result of a collection being initialized with a <see cref="KeyedItemOptions.ItemsAreMutable"/> value of <see langword="false"/>.</exception>
    bool Refresh(TKey key);
}
