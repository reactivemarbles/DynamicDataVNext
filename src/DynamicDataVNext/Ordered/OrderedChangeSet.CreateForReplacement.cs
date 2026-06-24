namespace DynamicDataVNext;

public static partial class OrderedChangeSet
{
    /// <inheritdoc cref="OrderedChangeSet{T}.CreateForReplacement(int, T, T)"/>
    /// <typeparam name="T">The type of the involved items.</typeparam>
    public static OrderedChangeSet<T> CreateForReplacement<T>(
            int index,
            T   oldItem,
            T   newItem)
        => OrderedChangeSet<T>.CreateForReplacement(
            index:      index,
            oldItem:    oldItem,
            newItem:    newItem);

    /// <inheritdoc cref="OrderedChangeSet{T}.CreateForReplacement(OrderedReplacement{T})"/>
    /// <typeparam name="T">The type of the involved items.</typeparam>
    public static OrderedChangeSet<T> CreateForReplacement<T>(OrderedReplacement<T> replacement)
        => CreateForUpdate(OrderedChange.CreateReplacement(replacement));
}

public readonly partial record struct OrderedChangeSet<T>
{
    /// <summary>
    /// Creates a new <see cref="OrderedChangeSet{T}"/> representing the <see cref="OrderedChangeType.Replacement"/> of a single item.
    /// </summary>
    /// <param name="index">The index at which the replacement occurs.</param>
    /// <param name="oldItem">The replaced item.</param>
    /// <param name="newItem">The replacement item.</param>
    /// <returns>An <see cref="OrderedChangeSet{T}"/> describing the replacement of the given items.</returns>
    public static OrderedChangeSet<T> CreateForReplacement(
            int index,
            T   oldItem,
            T   newItem)
        => CreateForUpdate(OrderedChange.CreateReplacement(
            index:      index,
            oldItem:    oldItem,
            newItem:    newItem));

    /// <summary>
    /// Creates a new <see cref="OrderedChangeSet{T}"/> representing the <see cref="OrderedChangeType.Replacement"/> of a single item.
    /// </summary>
    /// <param name="replacement">The replacement operation to be described.</param>
    /// <returns>An <see cref="OrderedChangeSet{T}"/> describing the given replacement operation.</returns>
    public static OrderedChangeSet<T> CreateForReplacement(OrderedReplacement<T> replacement)
        => CreateForUpdate(OrderedChange.CreateReplacement(replacement));
}
