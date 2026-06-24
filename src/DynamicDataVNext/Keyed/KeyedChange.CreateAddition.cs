using System.Collections.Generic;

namespace DynamicDataVNext;

public static partial class KeyedChange
{
    /// <inheritdoc cref="KeyedChange{TKey, TItem}.CreateAddition(KeyedItem{TKey, TItem})"/>
    /// <typeparam name="TKey">The type of the item's key.</typeparam>
    /// <typeparam name="TItem">The type of the added item.</typeparam>
    public static KeyedChange<TKey, TItem> CreateAddition<TKey, TItem>(KeyedItem<TKey, TItem> addition)
        => KeyedChange<TKey, TItem>.CreateAddition(addition);

    /// <inheritdoc cref="KeyedChange{TKey, TItem}.CreateAddition(KeyValuePair{TKey, TItem})"/>
    /// <typeparam name="TKey">The type of the item's key.</typeparam>
    /// <typeparam name="TItem">The type of the added item.</typeparam>
    public static KeyedChange<TKey, TItem> CreateAddition<TKey, TItem>(KeyValuePair<TKey, TItem> addition)
        => KeyedChange<TKey, TItem>.CreateAddition(addition);

    /// <inheritdoc cref="KeyedChange{TKey, TItem}.CreateAddition(TKey, TItem)"/>
    /// <typeparam name="TKey">The type of the item's key.</typeparam>
    /// <typeparam name="TItem">The type of the added item.</typeparam>
    public static KeyedChange<TKey, TItem> CreateAddition<TKey, TItem>(
        TKey    key,
        TItem   item)
        => KeyedChange<TKey, TItem>.CreateAddition(
            key:    key,
            item:   item);
}

public readonly partial record struct KeyedChange<TKey, TItem>
{
    /// <summary>
    /// Creates a new <see cref="KeyedChange{TKey, TItem}"/> representing the <see cref="KeyedChangeType.Addition"/> of a given item.
    /// </summary>
    /// <param name="addition">The added item, paired with its key.</param>
    /// <returns>A <see cref="KeyedChange{TKey, TItem}"/> describing the given addition operation.</returns>
    public static KeyedChange<TKey, TItem> CreateAddition(KeyedItem<TKey, TItem> addition)
        => new()
        {
            Key         = addition.Key,
            PrimaryItem = addition.Item,
            Type        = KeyedChangeType.Addition
        };

    /// <inheritdoc cref="CreateAddition(KeyedItem{TKey, TItem})"/>
    public static KeyedChange<TKey, TItem> CreateAddition(KeyValuePair<TKey, TItem> addition)
        => new()
        {
            Key         = addition.Key,
            PrimaryItem = addition.Value,
            Type        = KeyedChangeType.Addition
        };

    /// <summary>
    /// Creates a new <see cref="KeyedChange{TKey, TItem}"/> representing the <see cref="KeyedChangeType.Addition"/> of a given item.
    /// </summary>
    /// <param name="key">The item's key.</param>
    /// <param name="item">The added item.</param>
    /// <returns>A <see cref="KeyedChange{TKey, TItem}"/> describing the addition of the given item.</returns>
    public static KeyedChange<TKey, TItem> CreateAddition(
        TKey    key,
        TItem   item)
        => new()
        {
            Key         = key,
            PrimaryItem = item,
            Type        = KeyedChangeType.Addition
        };
}
