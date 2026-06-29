using System;

namespace DynamicDataVNext;

public static partial class OrderedChangeSet
{
    /// <inheritdoc cref="OrderedChangeSet{T}.CreateForRemoval(int, T)"/>
    /// <typeparam name="T">The type of the removed item.</typeparam>
    public static OrderedChangeSet<T> CreateForRemoval<T>(
            int index,
            T   item)
        => OrderedChangeSet<T>.CreateForRemoval(
            index:  index,
            item:   item);

    /// <inheritdoc cref="OrderedChangeSet{T}.CreateForRemoval(OrderedItem{T})"/>
    /// <typeparam name="T">The type of the removed item.</typeparam>
    public static OrderedChangeSet<T> CreateForRemoval<T>(OrderedItem<T> removal)
        => OrderedChangeSet<T>.CreateForRemoval(removal);
}

public readonly partial record struct OrderedChangeSet<T>
{
    /// <summary>
    /// Creates a new <see cref="OrderedChangeSet{T}"/> representing the <see cref="OrderedChangeType.Removal"/> of a single item.
    /// </summary>
    /// <param name="index">The index at which the removal occurs.</param>
    /// <param name="item">The removed item.</param>
    /// <returns>An <see cref="OrderedChangeSet{T}"/> describing the removal of the given item.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if <paramref name="index"/> is negative.</exception>
    public static OrderedChangeSet<T> CreateForRemoval(
            int index,
            T   item)
        => CreateForUpdate(OrderedChange.CreateRemoval(
            index:  index,
            item:   item));

    /// <summary>
    /// Creates a new <see cref="OrderedChangeSet{T}"/> representing the <see cref="OrderedChangeType.Removal"/> of a single item.
    /// </summary>
    /// <param name="removal">The removed item, and its index within its collection.</param>
    /// <returns>An <see cref="OrderedChangeSet{T}"/> describing the given removal operation.</returns>
    public static OrderedChangeSet<T> CreateForRemoval(OrderedItem<T> removal)
        => CreateForUpdate(OrderedChange.CreateRemoval(removal));
}
