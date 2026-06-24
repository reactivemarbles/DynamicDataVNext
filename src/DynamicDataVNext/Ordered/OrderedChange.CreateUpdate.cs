namespace DynamicDataVNext;

public static partial class OrderedChange
{
    /// <inheritdoc cref="OrderedChange{T}.CreateUpdate(int, T, int, T)"/>
    /// <typeparam name="T">The type of the items involved.</typeparam>
    public static OrderedChange<T> CreateUpdate<T>(
            int oldIndex,
            T   oldItem,
            int newIndex,
            T   newItem)
        => OrderedChange<T>.CreateUpdate(
            oldIndex:   oldIndex,
            oldItem:    oldItem,
            newIndex:   newIndex,
            newItem:    newItem);

    /// <inheritdoc cref="OrderedChange{T}.CreateUpdate(OrderedUpdate{T})"/>
    /// <typeparam name="T">The type of the items involved.</typeparam>
    public static OrderedChange<T> CreateUpdate<T>(OrderedUpdate<T> update)
        => OrderedChange<T>.CreateUpdate(update);
}

public readonly partial record struct OrderedChange<T>
{
    /// <summary>
    /// Creates a new <see cref="OrderedChange{T}"/> representing an <see cref="OrderedChangeType.Update"/> operation.
    /// </summary>
    /// <param name="oldIndex">The index of <paramref name="oldItem"/>, before the update occurs.</param>
    /// <param name="oldItem">The replaced item.</param>
    /// <param name="newIndex">The index of <paramref name="newItem"/>, after the update occurs.</param>
    /// <param name="newItem">The replacement item.</param>
    /// <returns>An <see cref="OrderedChange{T}"/> describing the update operation involving the given items.</returns>
    public static OrderedChange<T> CreateUpdate(
            int oldIndex,
            T   oldItem,
            int newIndex,
            T   newItem)
        => new()
        {
            PrimaryIndex    = newIndex,
            PrimaryItem     = newItem,
            SecondaryIndex  = oldIndex,
            SecondaryItem   = oldItem,
            Type            = OrderedChangeType.Update
        };

    /// <summary>
    /// Creates a new <see cref="OrderedChange{T}"/> representing an <see cref="OrderedChangeType.Update"/> operation.
    /// </summary>
    /// <param name="update">The update operation to be described.</param>
    /// <returns>An <see cref="OrderedChange{T}"/> describing the given update operation.</returns>
    public static OrderedChange<T> CreateUpdate(OrderedUpdate<T> update)
        => new()
        {
            PrimaryIndex    = update.NewIndex,
            PrimaryItem     = update.NewItem,
            SecondaryIndex  = update.OldIndex,
            SecondaryItem   = update.OldItem,
            Type            = OrderedChangeType.Update
        };
}
