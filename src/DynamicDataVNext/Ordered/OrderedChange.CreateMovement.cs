namespace DynamicDataVNext;

public static partial class OrderedChange
{
    /// <inheritdoc cref="OrderedChange{T}.CreateMovement(int, int, T)"/>
    /// <typeparam name="T">The type of the moved item.</typeparam>
    public static OrderedChange<T> CreateMovement<T>(
            int oldIndex,
            int newIndex,
            T   item)
        => OrderedChange<T>.CreateMovement(
            oldIndex:   oldIndex,
            newIndex:   newIndex,
            item:       item);

    /// <inheritdoc cref="OrderedChange{T}.CreateMovement(OrderedMovement{T})"/>
    /// <typeparam name="T">The type of the moved item.</typeparam>
    public static OrderedChange<T> CreateMovement<T>(OrderedMovement<T> movement)
        => OrderedChange<T>.CreateMovement(movement);
}

public readonly partial record struct OrderedChange<T>
{
    /// <summary>
    /// Creates a new <see cref="OrderedChange{T}"/> representing the <see cref="OrderedChangeType.Movement"/> of a given item.
    /// </summary>
    /// <param name="oldIndex">The index of the item, before the move occurs.</param>
    /// <param name="newIndex">The index of the item, after the move occurs.</param>
    /// <param name="item">The moved item.</param>
    /// <returns>An <see cref="OrderedChange{T}"/> describing the insertion of the given item.</returns>
    public static OrderedChange<T> CreateMovement(
            int oldIndex,
            int newIndex,
            T   item)
        => new()
        {
            PrimaryIndex    = newIndex,
            PrimaryItem     = item,
            SecondaryIndex  = oldIndex, 
            Type            = OrderedChangeType.Movement
        };

    /// <summary>
    /// Creates a new <see cref="OrderedChange{T}"/> representing the <see cref="OrderedChangeType.Movement"/> of a given item.
    /// </summary>
    /// <param name="movement">The operation to be described.</param>
    /// <returns>An <see cref="OrderedChange{T}"/> describing the given movement operation.</returns>
    public static OrderedChange<T> CreateMovement(OrderedMovement<T> movement)
        => new()
        {
            PrimaryIndex    = movement.NewIndex,
            PrimaryItem     = movement.Item,
            SecondaryIndex  = movement.OldIndex, 
            Type            = OrderedChangeType.Movement
        };
}
