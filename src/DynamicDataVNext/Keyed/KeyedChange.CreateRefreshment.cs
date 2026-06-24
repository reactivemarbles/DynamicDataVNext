using System.Collections.Generic;

namespace DynamicDataVNext;

public static partial class KeyedChange
{
    /// <inheritdoc cref="KeyedChange{TKey, TItem}.CreateRefreshment(TKey, TItem)"/>
    /// <typeparam name="TKey">The type of the item's key.</typeparam>
    /// <typeparam name="TItem">The type of the added item.</typeparam>
    public static KeyedChange<TKey, TItem> CreateRefreshment<TKey, TItem>(
            TKey    key,
            TItem   item)
        => KeyedChange<TKey, TItem>.CreateRefreshment(
            key:    key,
            item:   item);

    /// <inheritdoc cref="KeyedChange{TKey, TItem}.CreateRefreshment(KeyedItem{TKey, TItem})"/>
    /// <typeparam name="TKey">The type of the item's key.</typeparam>
    /// <typeparam name="TItem">The type of the added item.</typeparam>
    public static KeyedChange<TKey, TItem> CreateRefreshment<TKey, TItem>(KeyedItem<TKey, TItem> refreshment)
        => KeyedChange<TKey, TItem>.CreateRefreshment(refreshment);

    /// <inheritdoc cref="KeyedChange{TKey, TItem}.CreateRefreshment(KeyValuePair{TKey, TItem})"/>
    /// <typeparam name="TKey">The type of the item's key.</typeparam>
    /// <typeparam name="TItem">The type of the added item.</typeparam>
    public static KeyedChange<TKey, TItem> CreateRefreshment<TKey, TItem>(KeyValuePair<TKey, TItem> refreshment)
        => KeyedChange<TKey, TItem>.CreateRefreshment(refreshment);
}

public readonly partial record struct KeyedChange<TKey, TItem>
{
    /// <summary>
    /// Creates a new <see cref="KeyedChange{TKey, TItem}"/> representing the <see cref="KeyedChangeType.Refreshment"/> of a given item.
    /// </summary>
    /// <param name="key">The item's key.</param>
    /// <param name="item">The refreshed item.</param>
    /// <returns>A <see cref="KeyedChange{TKey, TItem}"/> describing the refreshment of the given item.</returns>
    public static KeyedChange<TKey, TItem> CreateRefreshment(
            TKey    key,
            TItem   item)
        => new()
        {
            Key         = key,
            PrimaryItem = item,
            Type        = KeyedChangeType.Refreshment
        };

    /// <summary>
    /// Creates a new <see cref="KeyedChange{TKey, TItem}"/> representing the <see cref="KeyedChangeType.Refreshment"/> of a given item.
    /// </summary>
    /// <param name="refreshment">The refreshed item, paired with its key.</param>
    /// <returns>A <see cref="KeyedChange{TKey, TItem}"/> describing the given refreshment operation.</returns>
    public static KeyedChange<TKey, TItem> CreateRefreshment(KeyedItem<TKey, TItem> refreshment)
        => new()
        {
            Key         = refreshment.Key,
            PrimaryItem = refreshment.Item,
            Type        = KeyedChangeType.Refreshment
        };

    /// <inheritdoc cref="CreateRefreshment(KeyedItem{TKey, TItem})"/>
    public static KeyedChange<TKey, TItem> CreateRefreshment(KeyValuePair<TKey, TItem> refreshment)
        => new()
        {
            Key         = refreshment.Key,
            PrimaryItem = refreshment.Value,
            Type        = KeyedChangeType.Refreshment
        };
}
