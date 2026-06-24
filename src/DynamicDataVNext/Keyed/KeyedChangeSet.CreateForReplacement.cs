namespace DynamicDataVNext;

public static partial class KeyedChangeSet
{
    /// <inheritdoc cref="KeyedChangeSet{TKey,TItem}.CreateForReplacement(TKey, TItem, TItem)"/>
    /// <typeparam name="TKey">The type of the items' key.</typeparam>
    /// <typeparam name="TItem">The type of the involved items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForReplacement<TKey, TItem>(
            TKey    key,
            TItem   oldItem,
            TItem   newItem)
        => CreateForUpdate(KeyedChange.CreateReplacement(
            key:        key,
            oldItem:    oldItem,
            newItem:    newItem));

    /// <inheritdoc cref="KeyedChangeSet{TKey,TItem}.CreateForReplacement(KeyedReplacement{TKey, TItem})"/>
    /// <typeparam name="TKey">The type of the items' key.</typeparam>
    /// <typeparam name="TItem">The type of the involved items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForReplacement<TKey, TItem>(KeyedReplacement<TKey, TItem> replacement)
        => CreateForUpdate(KeyedChange.CreateReplacement(
            key:        replacement.Key,
            oldItem:    replacement.OldItem,
            newItem:    replacement.NewItem));
}

public readonly partial record struct KeyedChangeSet<TKey, TItem>
{
    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the <see cref="KeyedChangeType.Replacement"/> of a single item.
    /// </summary>
    /// <param name="key">The items' key.</param>
    /// <param name="oldItem">The replaced item.</param>
    /// <param name="newItem">The replacement item.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the replacement involving the given items.</returns>
    public static KeyedChangeSet<TKey, TItem> CreateForReplacement(
            TKey    key,
            TItem   oldItem,
            TItem   newItem)
        => CreateForUpdate(KeyedChange.CreateReplacement(
            key:        key,
            oldItem:    oldItem,
            newItem:    newItem));

    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the <see cref="KeyedChangeType.Replacement"/> of a single item.
    /// </summary>
    /// <param name="replacement">The replacement operation to be described.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing <paramref name="replacement"/>.</returns>
    public static KeyedChangeSet<TKey, TItem> CreateForReplacement(KeyedReplacement<TKey, TItem> replacement)
        => CreateForUpdate(KeyedChange.CreateReplacement(
            key:        replacement.Key,
            oldItem:    replacement.OldItem,
            newItem:    replacement.NewItem));
}
