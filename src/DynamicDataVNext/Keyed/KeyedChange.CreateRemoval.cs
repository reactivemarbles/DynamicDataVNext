using System.Collections.Generic;

namespace DynamicDataVNext;

public static partial class KeyedChange
{
    /// <inheritdoc cref="KeyedChange{TKey, TItem}.CreateRemoval(TKey, TItem)"/>
    /// <typeparam name="TKey">The type of the item's key.</typeparam>
    /// <typeparam name="TItem">The type of the added item.</typeparam>
    public static KeyedChange<TKey, TItem> CreateRemoval<TKey, TItem>(
            TKey    key,
            TItem   item)
        => KeyedChange<TKey, TItem>.CreateRemoval(
            key:    key,
            item:   item);

    /// <inheritdoc cref="KeyedChange{TKey, TItem}.CreateRemoval(KeyedItem{TKey, TItem})"/>
    /// <typeparam name="TKey">The type of the item's key.</typeparam>
    /// <typeparam name="TItem">The type of the added item.</typeparam>
    public static KeyedChange<TKey, TItem> CreateRemoval<TKey, TItem>(KeyedItem<TKey, TItem> removal)
        => KeyedChange<TKey, TItem>.CreateRemoval(removal);

    /// <inheritdoc cref="KeyedChange{TKey, TItem}.CreateRemoval(KeyValuePair{TKey, TItem})"/>
    /// <typeparam name="TKey">The type of the item's key.</typeparam>
    /// <typeparam name="TItem">The type of the added item.</typeparam>
    public static KeyedChange<TKey, TItem> CreateRemoval<TKey, TItem>(KeyValuePair<TKey, TItem> removal)
        => KeyedChange<TKey, TItem>.CreateRemoval(removal);
}

public readonly partial record struct KeyedChange<TKey, TItem>
{
    /// <summary>
    /// Creates a new <see cref="KeyedChange{TKey, TItem}"/> representing the <see cref="KeyedChangeType.Removal"/> of a given item.
    /// </summary>
    /// <param name="key">The item's key.</param>
    /// <param name="item">The removed item.</param>
    /// <returns>A <see cref="KeyedChange{TKey, TItem}"/> describing the removal of the given item.</returns>
    public static KeyedChange<TKey, TItem> CreateRemoval(
            TKey    key,
            TItem   item)
        => new()
        {
            Key         = key,
            PrimaryItem = item,
            Type        = KeyedChangeType.Removal
        };

    /// <summary>
    /// Creates a new <see cref="KeyedChange{TKey, TItem}"/> representing the <see cref="KeyedChangeType.Removal"/> of a given item.
    /// </summary>
    /// <param name="removal">The removed item, paired with its key.</param>
    /// <returns>A <see cref="KeyedChange{TKey, TItem}"/> describing the given removal operation.</returns>
    public static KeyedChange<TKey, TItem> CreateRemoval(KeyedItem<TKey, TItem> removal)
        => new()
        {
            Key         = removal.Key,
            PrimaryItem = removal.Item,
            Type        = KeyedChangeType.Removal
        };

    /// <inheritdoc cref="CreateRemoval(KeyValuePair{TKey, TItem})"/>
    public static KeyedChange<TKey, TItem> CreateRemoval(KeyValuePair<TKey, TItem> removal)
        => new()
        {
            Key         = removal.Key,
            PrimaryItem = removal.Value,
            Type        = KeyedChangeType.Removal
        };
}
