using System;

namespace DynamicDataVNext.Kernel;

/// <summary>
/// Contains convenience methods for creating <see cref="KeyedChange{TKey, TItem}"/> values.
/// </summary>
public static class KeyedChange
{
    /// <inheritdoc cref="KeyedChange{TKey, TItem}.Addition(TKey, TItem)"/>
    public static KeyedChange<TKey, TItem> Addition<TKey, TItem>(
            TKey    key,
            TItem   item)
        => KeyedChange<TKey, TItem>.Addition(
            key:    key,
            item:   item);

    /// <inheritdoc cref="KeyedChange{TKey, TItem}.Removal(TKey, TItem)"/>
    public static KeyedChange<TKey, TItem> Removal<TKey, TItem>(
            TKey    key,
            TItem   item)
        => KeyedChange<TKey, TItem>.Removal(
            key:    key,
            item:   item);

    /// <inheritdoc cref="KeyedChange{TKey, TItem}.Replacement(TKey, TItem, TItem)"/>
    public static KeyedChange<TKey, TItem> Replacement<TKey, TItem>(
            TKey    key,
            TItem   oldItem,
            TItem   newItem)
        => KeyedChange<TKey, TItem>.Replacement(
            key:        key,
            oldItem:    oldItem,
            newItem:    newItem);
}

/// <summary>
/// Describes a single-item change made (or to be made) upon a keyed collection.
/// </summary>
/// <typeparam name="TKey">The type of the keys of items in the collection.</typeparam>
/// <typeparam name="TItem">The type of the items in the collection.</typeparam>
public readonly record struct KeyedChange<TKey, TItem>
{
    /// <summary>
    /// Creates a new <see cref="KeyedChange{TKey, TItem}"/> representing the addition of a given item.
    /// </summary>
    /// <param name="key">The item's key.</param>
    /// <param name="item">The item being added.</param>
    /// <returns>A <see cref="KeyedChange{TKey, TItem}"/> describing the addition of the given item.</returns>
    public static KeyedChange<TKey, TItem> Addition(
            TKey    key,
            TItem   item)
        => new()
        {
            Key     = key,
            NewItem = item,
            Type    = KeyedChangeType.Addition
        };

    /// <summary>
    /// Creates a new <see cref="KeyedChange{TKey, TItem}"/> representing the removal of a given item.
    /// </summary>
    /// <param name="key">The item's key.</param>
    /// <param name="item">The item being removed.</param>
    /// <returns>A <see cref="KeyedChange{TKey, TItem}"/> describing the removal of the given item.</returns>
    public static KeyedChange<TKey, TItem> Removal(
            TKey    key,
            TItem   item)
        => new()
        {
            Key     = key,
            OldItem = item,
            Type    = KeyedChangeType.Removal
        };

    /// <summary>
    /// Creates a new <see cref="KeyedChange{TKey, TItem}"/> representing the replacement of a given item by another with the same key.
    /// </summary>
    /// <param name="key">The items' key.</param>
    /// <param name="oldItem">The item being replaced.</param>
    /// <param name="newItem">The replacement item.</param>
    /// <returns>A <see cref="KeyedChange{TKey, TItem}"/> describing the replacement of the given items.</returns>
    public static KeyedChange<TKey, TItem> Replacement(
            TKey    key,
            TItem   oldItem,
            TItem   newItem)
        => new()
        {
            Key     = key,
            OldItem = oldItem,
            NewItem = newItem,
            Type    = KeyedChangeType.Replacement
        };

    private readonly TKey               _key;
    private readonly TItem              _newItem;
    private readonly TItem              _oldItem;
    private readonly KeyedChangeType    _type;

    /// <summary>
    /// The type of single-item change being made.
    /// </summary>
    public KeyedChangeType Type
    {
        get => _type;
        private init => _type = value;
    }

    /// <summary>
    /// Interprets the information within this change as an addition operation.
    /// </summary>
    /// <returns>A <see cref="KeyedItem{TKey, TItem}"/> describing this change.</returns>
    /// <exception cref="InvalidOperationException"><see cref="Type"/> is not <see cref="KeyedChangeType.Addition"/>.</exception>
    public KeyedItem<TKey, TItem> AsAddition()
        => (Type is KeyedChangeType.Addition)
            ? new()
            {
                Item    = NewItem,
                Key     = Key
            }
            : throw new InvalidOperationException($"Invalid attempt to interpret a {nameof(KeyedChange)} of type {Type} as type {KeyedChangeType.Addition}");

    /// <summary>
    /// Interprets the information within this change as a removal operation.
    /// </summary>
    /// <returns>A <see cref="KeyedRemoval{TKey, TItem}"/> describing this change.</returns>
    /// <exception cref="InvalidOperationException"><see cref="Type"/> is not <see cref="KeyedChangeType.Removal"/>.</exception>
    public KeyedItem<TKey, TItem> AsRemoval()
        => (Type is KeyedChangeType.Removal)
            ? new()
            {
                Item    = OldItem,
                Key     = Key
            }
            : throw new InvalidOperationException($"Invalid attempt to interpret a {nameof(KeyedChange)} of type {Type} as type {KeyedChangeType.Removal}");

    /// <summary>
    /// Interprets the information within this change as a replacement operation.
    /// </summary>
    /// <returns>A <see cref="KeyedReplacement{TKey, TItem}"/> describing this change.</returns>
    /// <exception cref="InvalidOperationException"><see cref="Type"/> is not <see cref="KeyedChangeType.Replacement"/>.</exception>
    public KeyedReplacement<TKey, TItem> AsReplacement()
        => (Type is KeyedChangeType.Replacement)
            ? new()
            {
                Key     = Key,
                NewItem = NewItem,
                OldItem = OldItem
            }
            : throw new InvalidOperationException($"Invalid attempt to interpret a {nameof(KeyedChange)} of type {Type} as type {KeyedChangeType.Replacement}");

    private TKey Key
    {
        get => _key;
        init => _key = value;
    }

    private TItem NewItem
    {
        get => _newItem;
        init => _newItem = value;
    }
        
    private TItem OldItem
    {
        get => _oldItem;
        init => _oldItem = value;
    }
}
