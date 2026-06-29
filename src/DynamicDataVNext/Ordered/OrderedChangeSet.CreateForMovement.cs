using System;

namespace DynamicDataVNext;

public static partial class OrderedChangeSet
{
    /// <inheritdoc cref="OrderedChangeSet{T}.CreateForMovement(int, int, T)"/>
    /// <typeparam name="T">The type of the moved item.</typeparam>
    public static OrderedChangeSet<T> CreateForMovement<T>(
            int oldIndex,
            int newIndex,
            T   item)
        => OrderedChangeSet<T>.CreateForMovement(
            oldIndex:   oldIndex,
            newIndex:   newIndex,
            item:       item);

    /// <inheritdoc cref="OrderedChangeSet{T}.CreateForMovement(OrderedMovement{T})"/>
    /// <typeparam name="T">The type of the moved item.</typeparam>
    public static OrderedChangeSet<T> CreateForMovement<T>(OrderedMovement<T> movement)
        => OrderedChangeSet<T>.CreateForMovement(movement);
}

public readonly partial record struct OrderedChangeSet<T>
{
    /// <summary>
    /// Creates a new <see cref="OrderedChangeSet{T}"/> representing the <see cref="OrderedChangeType.Movement"/> of a single item.
    /// </summary>
    /// <param name="oldIndex">The index of the item, before the move occurs.</param>
    /// <param name="newIndex">The index of the item, after the move occurs.</param>
    /// <param name="item">The moved item.</param>
    /// <returns>An <see cref="OrderedChangeSet{T}"/> describing the movement of the given item.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if <paramref name="oldIndex"/> or <paramref name="newIndex"/> is negative.</exception>
    public static OrderedChangeSet<T> CreateForMovement(
            int oldIndex,
            int newIndex,
            T   item)
        => CreateForUpdate(OrderedChange.CreateMovement(
            oldIndex:   oldIndex,
            newIndex:   newIndex,
            item:       item));

    /// <summary>
    /// Creates a new <see cref="OrderedChangeSet{T}"/> representing the <see cref="OrderedChangeType.Movement"/> of a single item.
    /// </summary>
    /// <param name="movement">The movement operation to be described.</param>
    /// <returns>An <see cref="OrderedChangeSet{T}"/> describing the given movement operation.</returns>
    public static OrderedChangeSet<T> CreateForMovement(OrderedMovement<T> movement)
        => CreateForUpdate(OrderedChange.CreateMovement(movement));
}
