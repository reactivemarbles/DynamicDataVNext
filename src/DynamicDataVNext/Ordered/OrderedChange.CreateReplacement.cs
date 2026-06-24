namespace DynamicDataVNext;

public static partial class OrderedChange
{
    /// <inheritdoc cref="OrderedChange{T}.CreateReplacement(int, T, T)"/>
    /// <typeparam name="T">The type of the items involved.</typeparam>
    public static OrderedChange<T> CreateReplacement<T>(
            int index,
            T   oldItem,
            T   newItem)
        => OrderedChange<T>.CreateReplacement(
            index:      index,
            oldItem:    oldItem,
            newItem:    newItem);

    /// <inheritdoc cref="OrderedChange{T}.CreateReplacement(OrderedReplacement{T})"/>
    /// <typeparam name="T">The type of the items involved.</typeparam>
    public static OrderedChange<T> CreateReplacement<T>(OrderedReplacement<T> replacement)
        => OrderedChange<T>.CreateReplacement(replacement);
}

public readonly partial record struct OrderedChange<T>
{
    /// <summary>
    /// Creates a new <see cref="OrderedChange{T}"/> representing the <see cref="OrderedChangeType.Replacement"/> of a given item with another.
    /// </summary>
    /// <param name="index">The index at which the replacement occurs.</param>
    /// <param name="oldItem">The replaced item.</param>
    /// <param name="newItem">The replacement item.</param>
    /// <returns>An <see cref="OrderedChange{T}"/> describing the replacement involving the given items.</returns>
    public static OrderedChange<T> CreateReplacement(
            int index,
            T   oldItem,
            T   newItem)
        => new()
        {
            PrimaryIndex    = index,
            PrimaryItem     = newItem,
            SecondaryItem   = oldItem, 
            Type            = OrderedChangeType.Replacement
        };

    /// <summary>
    /// Creates a new <see cref="OrderedChange{T}"/> representing the <see cref="OrderedChangeType.Replacement"/> of a given item with another.
    /// </summary>
    /// <param name="replacement">The replacement operation to be described.</param>
    /// <returns>An <see cref="OrderedChange{T}"/> describing the given replacement operation.</returns>
    public static OrderedChange<T> CreateReplacement(OrderedReplacement<T> replacement)
        => new()
        {
            PrimaryIndex    = replacement.Index,
            PrimaryItem     = replacement.NewItem,
            SecondaryItem   = replacement.OldItem, 
            Type            = OrderedChangeType.Replacement
        };
}
