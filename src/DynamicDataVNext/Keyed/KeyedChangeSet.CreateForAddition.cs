using System.Collections.Generic;

namespace DynamicDataVNext;

public static partial class KeyedChangeSet
{
    /// <inheritdoc cref="KeyedChangeSet{TKey,TItem}.CreateForAddition(TKey, TItem)"/>
    /// <typeparam name="TKey">The type of the item's key.</typeparam>
    /// <typeparam name="TItem">The type of the added item.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForAddition<TKey, TItem>(
            TKey    key,
            TItem   item)
        => KeyedChangeSet<TKey, TItem>.CreateForAddition(
            key:    key,
            item:   item);

    /// <inheritdoc cref="KeyedChangeSet{TKey,TItem}.CreateForAddition(KeyedItem{TKey, TItem})"/>
    /// <typeparam name="TKey">The type of the item's key.</typeparam>
    /// <typeparam name="TItem">The type of the added item.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForAddition<TKey, TItem>(KeyedItem<TKey, TItem> addition)
        => KeyedChangeSet<TKey, TItem>.CreateForAddition(addition);

    /// <inheritdoc cref="KeyedChangeSet{TKey,TItem}.CreateForAddition(KeyValuePair{TKey, TItem})"/>
    /// <typeparam name="TKey">The type of the item's key.</typeparam>
    /// <typeparam name="TItem">The type of the added item.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForAddition<TKey, TItem>(KeyValuePair<TKey, TItem> addition)
        => KeyedChangeSet<TKey, TItem>.CreateForAddition(addition);
}

public readonly partial record struct KeyedChangeSet<TKey, TItem>
{
    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the <see cref="KeyedChangeType.Addition"/> of a single item.
    /// </summary>
    /// <param name="key">The item's key.</param>
    /// <param name="item">The added item.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the addition of the given item.</returns>
    public static KeyedChangeSet<TKey, TItem> CreateForAddition(
            TKey    key,
            TItem   item)
        => CreateForUpdate(KeyedChange.CreateAddition(
            key:    key,
            item:   item));

    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the <see cref="KeyedChangeType.Addition"/> of a single item.
    /// </summary>
    /// <param name="addition">The added item and its key.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the addition of the given item.</returns>
    public static KeyedChangeSet<TKey, TItem> CreateForAddition(KeyedItem<TKey, TItem> addition)
        => CreateForUpdate(KeyedChange.CreateAddition(addition));

    /// <inheritdoc cref="CreateForAddition(KeyedItem{TKey, TItem})"/>
    public static KeyedChangeSet<TKey, TItem> CreateForAddition(KeyValuePair<TKey, TItem> addition)
        => CreateForUpdate(KeyedChange.CreateAddition(addition));
}
