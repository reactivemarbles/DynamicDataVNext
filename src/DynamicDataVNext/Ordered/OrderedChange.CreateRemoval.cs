using System;

namespace DynamicDataVNext;

public static partial class OrderedChange
{
    /// <inheritdoc cref="OrderedChange{T}.CreateRemoval(int, T)"/>
    /// <typeparam name="T">The type of the removed item.</typeparam>
    public static OrderedChange<T> CreateRemoval<T>(
            int index,
            T   item)
        => OrderedChange<T>.CreateRemoval(
            index:  index,
            item:   item);

    /// <inheritdoc cref="OrderedChange{T}.CreateRemoval(OrderedItem{T})"/>
    /// <typeparam name="T">The type of the removed item.</typeparam>
    public static OrderedChange<T> CreateRemoval<T>(OrderedItem<T> removal)
        => OrderedChange<T>.CreateRemoval(removal);
}

public readonly partial record struct OrderedChange<T>
{
    /// <summary>
    /// Creates a new <see cref="OrderedChange{T}"/> representing the <see cref="OrderedChangeType.Removal"/> of a given item.
    /// </summary>
    /// <param name="index">The index at which the removal occurs.</param>
    /// <param name="item">The removed item.</param>
    /// <returns>An <see cref="OrderedChange{T}"/> describing the removal of the given item.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if <paramref name="index"/> is negative.</exception>
    public static OrderedChange<T> CreateRemoval(
        int index,
        T   item)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index);

        return new()
        {
            PrimaryIndex    = index,
            PrimaryItem     = item,
            Type            = OrderedChangeType.Removal
        };
    }

    /// <summary>
    /// Creates a new <see cref="OrderedChange{T}"/> representing the <see cref="OrderedChangeType.Removal"/> of a given item.
    /// </summary>
    /// <param name="removal">The removed item, and its index within its collection.</param>
    /// <returns>An <see cref="OrderedChange{T}"/> describing the given removal operation.</returns>
    public static OrderedChange<T> CreateRemoval(OrderedItem<T> removal)
        => new()
        {
            PrimaryIndex    = removal.Index,
            PrimaryItem     = removal.Item,
            Type            = OrderedChangeType.Removal
        };
}
