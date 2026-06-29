using System;

namespace DynamicDataVNext;

public static partial class OrderedChange
{
    /// <inheritdoc cref="OrderedChange{T}.CreateInsertion(int, T)"/>
    /// <typeparam name="T">The type of the inserted item.</typeparam>
    public static OrderedChange<T> CreateInsertion<T>(
            int index,
            T   item)
        => OrderedChange<T>.CreateInsertion(
            index:  index,
            item:   item);

    /// <inheritdoc cref="OrderedChange{T}.CreateInsertion(OrderedItem{T})"/>
    /// <typeparam name="T">The type of the inserted item.</typeparam>
    public static OrderedChange<T> CreateInsertion<T>(OrderedItem<T> insertion)
        => OrderedChange<T>.CreateInsertion(insertion);
}

public readonly partial record struct OrderedChange<T>
{
    /// <summary>
    /// Creates a new <see cref="OrderedChange{T}"/> representing the <see cref="OrderedChangeType.Insertion"/> of a given item.
    /// </summary>
    /// <param name="index">The index at which the insertion occurs.</param>
    /// <param name="item">The inserted item.</param>
    /// <returns>An <see cref="OrderedChange{T}"/> describing the given insertion operation.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if <paramref name="index"/> is negative.</exception>
    public static OrderedChange<T> CreateInsertion(
        int index,
        T   item)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index);
    
        return new()
        {
            PrimaryIndex    = index,
            PrimaryItem     = item,
            Type            = OrderedChangeType.Insertion
        };
    }

    /// <summary>
    /// Creates a new <see cref="OrderedChange{T}"/> representing the <see cref="OrderedChangeType.Insertion"/> of a given item.
    /// </summary>
    /// <param name="insertion">The inserted item, and its insertion location.</param>
    /// <returns>An <see cref="OrderedChange{T}"/> describing the insertion of the given item.</returns>
    public static OrderedChange<T> CreateInsertion(OrderedItem<T> insertion)
        => new()
        {
            PrimaryIndex    = insertion.Index,
            PrimaryItem     = insertion.Item,
            Type            = OrderedChangeType.Insertion
        };
}
