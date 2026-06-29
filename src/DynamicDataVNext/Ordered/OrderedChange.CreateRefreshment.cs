using System;

namespace DynamicDataVNext;

public static partial class OrderedChange
{
    /// <inheritdoc cref="OrderedChange{T}.CreateRefreshment(int, T)"/>
    /// <typeparam name="T">The type of the refreshed item.</typeparam>
    public static OrderedChange<T> CreateRefreshment<T>(
            int index,
            T   item)
        => OrderedChange<T>.CreateRefreshment(
            index:  index,
            item:   item);

    /// <inheritdoc cref="OrderedChange{T}.CreateRefreshment(OrderedItem{T})"/>
    /// <typeparam name="T">The type of the refreshed item.</typeparam>
    public static OrderedChange<T> CreateRefreshment<T>(OrderedItem<T> refreshment)
        => OrderedChange<T>.CreateRefreshment(refreshment);
}

public readonly partial record struct OrderedChange<T>
{
    /// <summary>
    /// Creates a new <see cref="OrderedChange{T}"/> representing the <see cref="OrderedChangeType.Refreshment"/> of a given item.
    /// </summary>
    /// <param name="index">The index at which the refreshment occurs.</param>
    /// <param name="item">The refreshed item.</param>
    /// <returns>An <see cref="OrderedChange{T}"/> describing the refreshment of the given item.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if <paramref name="index"/> is negative.</exception>
    public static OrderedChange<T> CreateRefreshment(
        int index,
        T   item)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index);
    
        return new()
        {
            PrimaryIndex    = index,
            PrimaryItem     = item,
            Type            = OrderedChangeType.Refreshment
        };
    }

    /// <summary>
    /// Creates a new <see cref="OrderedChange{T}"/> representing the <see cref="OrderedChangeType.Refreshment"/> of a given item.
    /// </summary>
    /// <param name="refreshment">The refreshed item, and its index within its collection.</param>
    /// <returns>An <see cref="OrderedChange{T}"/> describing the given refreshment operation.</returns>
    public static OrderedChange<T> CreateRefreshment(OrderedItem<T> refreshment)
        => new()
        {
            PrimaryIndex    = refreshment.Index,
            PrimaryItem     = refreshment.Item,
            Type            = OrderedChangeType.Refreshment
        };
}
