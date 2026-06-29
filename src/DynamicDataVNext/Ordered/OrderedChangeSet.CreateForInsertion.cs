using System;

namespace DynamicDataVNext;

public static partial class OrderedChangeSet
{
    /// <inheritdoc cref="OrderedChangeSet{T}.CreateForInsertion(int, T)"/>
    /// <typeparam name="T">The type of the inserted item.</typeparam>
    public static OrderedChangeSet<T> CreateForInsertion<T>(
            int index,
            T   item)
        => OrderedChangeSet<T>.CreateForInsertion(
            index:  index,
            item:   item);

    /// <inheritdoc cref="OrderedChangeSet{T}.CreateForInsertion(OrderedItem{T})"/>
    /// <typeparam name="T">The type of the inserted item.</typeparam>
    public static OrderedChangeSet<T> CreateForInsertion<T>(OrderedItem<T> insertion)
        => OrderedChangeSet<T>.CreateForInsertion(insertion);
}

public readonly partial record struct OrderedChangeSet<T>
{
    /// <summary>
    /// Creates a new <see cref="OrderedChangeSet{T}"/> representing the <see cref="OrderedChangeType.Insertion"/> of a single item.
    /// </summary>
    /// <param name="index">The index at which the insertion occurs.</param>
    /// <param name="item">The inserted item.</param>
    /// <returns>An <see cref="OrderedChangeSet{T}"/> describing the insertion of the given item.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if <paramref name="index"/> is negative.</exception>
    public static OrderedChangeSet<T> CreateForInsertion(
            int index,
            T   item)
        => CreateForUpdate(OrderedChange.CreateInsertion(
            index:  index,
            item:   item));

    /// <summary>
    /// Creates a new <see cref="OrderedChangeSet{T}"/> representing the <see cref="OrderedChangeType.Insertion"/> of a single item.
    /// </summary>
    /// <param name="insertion">The inserted item, and its insertion index.</param>
    /// <returns>An <see cref="OrderedChangeSet{T}"/> describing the given insertion operation.</returns>
    public static OrderedChangeSet<T> CreateForInsertion(OrderedItem<T> insertion)
        => CreateForUpdate(OrderedChange.CreateInsertion(insertion));
}
