using System.Collections.Generic;

namespace DynamicDataVNext;

public static partial class KeyedChangeSet
{
    /// <inheritdoc cref="KeyedChangeSet{TKey,TItem}.CreateForRemoval(TKey, TItem)"/>
    /// <typeparam name="TKey">The type of the item's key.</typeparam>
    /// <typeparam name="TItem">The type of the removed item.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForRemoval<TKey, TItem>(
            TKey    key,
            TItem   item)
        => KeyedChangeSet<TKey, TItem>.CreateForRemoval(
            key:    key,
            item:   item);

    /// <inheritdoc cref="KeyedChangeSet{TKey,TItem}.CreateForRemoval(KeyedItem{TKey, TItem})"/>
    /// <typeparam name="TKey">The type of the item's key.</typeparam>
    /// <typeparam name="TItem">The type of the removed item.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForRemoval<TKey, TItem>(KeyedItem<TKey, TItem> removal)
        => KeyedChangeSet<TKey, TItem>.CreateForRemoval(removal);

    /// <inheritdoc cref="KeyedChangeSet{TKey,TItem}.CreateForRemoval(KeyValuePair{TKey, TItem})"/>
    /// <typeparam name="TKey">The type of the item's key.</typeparam>
    /// <typeparam name="TItem">The type of the removed item.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForRemoval<TKey, TItem>(KeyValuePair<TKey, TItem> removal)
        => KeyedChangeSet<TKey, TItem>.CreateForRemoval(removal);
}

public readonly partial record struct KeyedChangeSet<TKey, TItem>
{
    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the <see cref="KeyedChangeType.Removal"/> of a single item.
    /// </summary>
    /// <param name="key">The item's key.</param>
    /// <param name="item">The removed item.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the removal of the given item.</returns>
    public static KeyedChangeSet<TKey, TItem> CreateForRemoval(
            TKey    key,
            TItem   item)
        => CreateForUpdate(KeyedChange.CreateRemoval(
            key:    key,
            item:   item));

    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the <see cref="KeyedChangeType.Removal"/> of a single item.
    /// </summary>
    /// <param name="removal">The removed item and its key.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the removal of the given item.</returns>
    public static KeyedChangeSet<TKey, TItem> CreateForRemoval(KeyedItem<TKey, TItem> removal)
        => CreateForUpdate(KeyedChange.CreateRemoval(removal));

    /// <inheritdoc cref="CreateForRemoval(KeyedItem{TKey, TItem})"/>
    public static KeyedChangeSet<TKey, TItem> CreateForRemoval(KeyValuePair<TKey, TItem> removal)
        => CreateForUpdate(KeyedChange.CreateRemoval(removal));
}
