using System.Collections.Generic;

namespace DynamicDataVNext;

public static partial class KeyedChangeSet
{
    /// <inheritdoc cref="KeyedChangeSet{TKey,TItem}.CreateForRefreshment(TKey, TItem)"/>
    /// <typeparam name="TKey">The type of the item's key.</typeparam>
    /// <typeparam name="TItem">The type of the refreshed item.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForRefreshment<TKey, TItem>(
            TKey    key,
            TItem   item)
        => KeyedChangeSet<TKey, TItem>.CreateForRefreshment(
            key:    key,
            item:   item);

    /// <inheritdoc cref="KeyedChangeSet{TKey,TItem}.CreateForRefreshment(KeyedItem{TKey, TItem})"/>
    /// <typeparam name="TKey">The type of the item's key.</typeparam>
    /// <typeparam name="TItem">The type of the refreshed item.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForRefreshment<TKey, TItem>(KeyedItem<TKey, TItem> refreshment)
        => KeyedChangeSet<TKey, TItem>.CreateForRefreshment(refreshment);

    /// <inheritdoc cref="KeyedChangeSet{TKey,TItem}.CreateForRefreshment(KeyValuePair{TKey, TItem})"/>
    /// <typeparam name="TKey">The type of the item's key.</typeparam>
    /// <typeparam name="TItem">The type of the refreshed item.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForRefreshment<TKey, TItem>(KeyValuePair<TKey, TItem> refreshment)
        => KeyedChangeSet<TKey, TItem>.CreateForRefreshment(refreshment);
}

public readonly partial record struct KeyedChangeSet<TKey, TItem>
{
    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the refreshment of a single item.
    /// </summary>
    /// <param name="key">The item's key.</param>
    /// <param name="item">The refreshed item.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the refreshment of the given item.</returns>
    public static KeyedChangeSet<TKey, TItem> CreateForRefreshment(
            TKey    key,
            TItem   item)
        => CreateForUpdate(KeyedChange.CreateRefreshment(
            key:    key,
            item:   item));

    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the <see cref="KeyedChangeType.Refreshment"/> of a single item.
    /// </summary>
    /// <param name="refreshment">The refreshed item and its key.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the refreshment of the given item.</returns>
    public static KeyedChangeSet<TKey, TItem> CreateForRefreshment(KeyedItem<TKey, TItem> refreshment)
        => CreateForUpdate(KeyedChange.CreateRefreshment(refreshment));

    /// <inheritdoc cref="CreateForRefreshment(KeyedItem{TKey, TItem})"/>
    public static KeyedChangeSet<TKey, TItem> CreateForRefreshment(KeyValuePair<TKey, TItem> refreshment)
        => CreateForUpdate(KeyedChange.CreateRefreshment(refreshment));
}
