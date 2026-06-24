namespace DynamicDataVNext;

public static partial class KeyedChange
{
    /// <inheritdoc cref="KeyedChange{TKey, TItem}.CreateReplacement(TKey, TItem, TItem)"/>
    /// <typeparam name="TKey">The type of the item's key.</typeparam>
    /// <typeparam name="TItem">The type of the added item.</typeparam>
    public static KeyedChange<TKey, TItem> CreateReplacement<TKey, TItem>(
            TKey    key,
            TItem   oldItem,
            TItem   newItem)
        => KeyedChange<TKey, TItem>.CreateReplacement(
            key:        key,
            oldItem:    oldItem,
            newItem:    newItem);

    /// <inheritdoc cref="KeyedChange{TKey, TItem}.CreateReplacement(KeyedReplacement{TKey, TItem})"/>
    /// <typeparam name="TKey">The type of the item's key.</typeparam>
    /// <typeparam name="TItem">The type of the added item.</typeparam>
    public static KeyedChange<TKey, TItem> CreateReplacement<TKey, TItem>(KeyedReplacement<TKey, TItem> replacement)
        => KeyedChange<TKey, TItem>.CreateReplacement(replacement);
}

public readonly partial record struct KeyedChange<TKey, TItem>
{
    /// <summary>
    /// Creates a new <see cref="KeyedChange{TKey, TItem}"/> representing the <see cref="KeyedChangeType.Replacement"/> of a given item by another with the same key.
    /// </summary>
    /// <param name="key">The items' key.</param>
    /// <param name="oldItem">The item being replaced.</param>
    /// <param name="newItem">The replacement item.</param>
    /// <returns>A <see cref="KeyedChange{TKey, TItem}"/> describing the replacement involving the given items.</returns>
    public static KeyedChange<TKey, TItem> CreateReplacement(
            TKey    key,
            TItem   oldItem,
            TItem   newItem)
        => new()
        {
            Key             = key,
            PrimaryItem     = newItem,
            SecondaryItem   = oldItem,
            Type            = KeyedChangeType.Replacement
        };

    /// <summary>
    /// Creates a new <see cref="KeyedChange{TKey, TItem}"/> representing the <see cref="KeyedChangeType.Replacement"/> of a given item by another with the same key.
    /// </summary>
    /// <param name="replacement">The replacement operation to be described.</param>
    /// <returns>A <see cref="KeyedChange{TKey, TItem}"/> describing the given replacement operation.</returns>
    public static KeyedChange<TKey, TItem> CreateReplacement(KeyedReplacement<TKey, TItem> replacement)
        => new()
        {
            Key             = replacement.Key,
            PrimaryItem     = replacement.NewItem,
            SecondaryItem   = replacement.OldItem,
            Type            = KeyedChangeType.Replacement
        };
}
