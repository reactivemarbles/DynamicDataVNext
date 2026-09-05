namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of keyed items, which can report on mutations made to its items.
/// </summary>
/// <typeparam name="TKey">The type of the key values in the collection.</typeparam>
/// <typeparam name="TItem">The type of the items in the collection.</typeparam>
public interface IRefreshableCache<in TKey, TItem>
{
    /// <summary>
    /// Signals that an item within the collection has been externally mutated.
    /// </summary>
    /// <param name="item">The item that was mutated.</param>
    /// <returns><see langword="false"/> if the collection does not actually contain <paramref name="item"/>. Otherwise, <see langword="true"/>.</returns>
    /// <exception cref="ImmutableRefreshException">Throws if the items in the collection are immutable. This is generally the result of a collection being initialized with a <see cref="KeyedItemOptions.ItemsAreMutable"/> value of <see langword="false"/>.</exception>
    bool Refresh(TItem item);

    /// <summary>
    /// Signals that an item within the collection has been externally mutated.
    /// </summary>
    /// <param name="key">The key value of the item that was mutated.</param>
    /// <returns><see langword="false"/> if the collection does not actually contain <paramref name="key"/>. Otherwise, <see langword="true"/>.</returns>
    /// <exception cref="ImmutableRefreshException">Throws if the items in the collection are immutable. This is generally the result of a collection being initialized with a <see cref="KeyedItemOptions.ItemsAreMutable"/> value of <see langword="false"/>.</exception>
    bool RefreshKey(TKey key);
}
