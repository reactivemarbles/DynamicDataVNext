using System.Collections.Generic;

namespace DynamicDataVNext;

/// <summary>
/// Describes an item within a keyed collection.
/// </summary>
/// <typeparam name="TKey">The type of the item's key.</typeparam>
/// <typeparam name="TItem">The type of the item.</typeparam>
public readonly record struct KeyedItem<TKey, TItem>
{
    /// <summary>
    /// The collection item.
    /// </summary>
    public required TItem Item { get; init; }

    /// <summary>
    /// The identifying key of the item.
    /// </summary>
    public required TKey Key { get; init; }
    
    /// <summary>
    /// Converts a given key-and-item pairing to a <see cref="KeyedItem{TKey,TItem}"/> structure.
    /// </summary>
    /// <param name="pair">The key-and-item pairing to be converted.</param>
    /// <returns>A <see cref="KeyedItem{TKey,TItem}"/> containing the key-and-item pairing given by <paramref name="pair"/>.</returns>
    public static implicit operator KeyedItem<TKey, TItem>(KeyValuePair<TKey, TItem> pair)
        => new()
        {
            Item    = pair.Value,
            Key     = pair.Key
        };

    /// <summary>
    /// Converts a given key-and-item pairing to a <see cref="KeyedItem{TKey,TItem}"/> structure.
    /// </summary>
    /// <param name="pair">The key-and-item pairing to be converted.</param>
    /// <returns>A <see cref="KeyedItem{TKey,TItem}"/> containing the key-and-item pairing given by <paramref name="pair"/>.</returns>
    public static implicit operator KeyValuePair<TKey, TItem>(KeyedItem<TKey, TItem> pair)
        => new(
            key:    pair.Key,
            value:  pair.Item);
}
