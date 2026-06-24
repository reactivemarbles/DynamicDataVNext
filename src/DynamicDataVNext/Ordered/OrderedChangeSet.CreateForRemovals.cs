using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DynamicDataVNext;

public static partial class OrderedChangeSet
{
    /// <inheritdoc cref="OrderedChangeSet{T}.CreateForRemovals(int, IReadOnlyList{T})"/>
    /// <typeparam name="T">The type of items being removed.</typeparam>
    public static OrderedChangeSet<T> CreateForRemovals<T>(
            int                 index,
            IReadOnlyList<T>    items)
        => OrderedChangeSet<T>.CreateForRemovals(
            index:  index,
            items:  items);

    /// <inheritdoc cref="OrderedChangeSet{T}.CreateForRemovals(int, ReadOnlySpan{T})"/>
    /// <typeparam name="T">The type of items being removed.</typeparam>
    public static OrderedChangeSet<T> CreateForRemovals<T>(
            int             index,
            ReadOnlySpan<T> items)
        => OrderedChangeSet<T>.CreateForRemovals(
            index:  index,
            items:  items);
}

public readonly partial record struct OrderedChangeSet<T>
{
    /// <inheritdoc cref="CreateForRemovals(int, ReadOnlySpan{T})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    public static OrderedChangeSet<T> CreateForRemovals(
        int                 index,
        IReadOnlyList<T>    items)
    {
        ArgumentNullException.ThrowIfNull(items);

        if (items.Count is 0)
            return Empty;

        var changes = ImmutableArray.CreateBuilder<OrderedChange<T>>(initialCapacity: items.Count);

        for (var itemIndex = items.Count - 1; itemIndex >= 0; --itemIndex)
            changes.Add(OrderedChange.CreateRemoval(
                index:  itemIndex + index,
                item:   items[itemIndex]));

        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <summary>
    /// Creates a new <see cref="OrderedChangeSet{T}"/> representing the <see cref="OrderedChangeType.Insertion"/> of a range of items.
    /// </summary>
    /// <param name="index">The index at which the removal occurs.</param>
    /// <param name="items">The removed items.</param>
    /// <returns>An <see cref="OrderedChangeSet{T}"/> describing the removal of the given items.</returns>
    /// <remarks>
    /// The generated sequence of changes will involve removing the items in the reverse order that they are listed in <paramref name="items"/>. This helps optimize the removal of items from <see cref="List{T}"/> or similar data structures, should consumers be forced to process the removals one-at-a-time, instead of calling a .RemoveRange() method. 
    /// </remarks>
    public static OrderedChangeSet<T> CreateForRemovals(
        int             index,
        ReadOnlySpan<T> items)
    {
        if (items.Length is 0)
            return Empty;

        var changes = ImmutableArray.CreateBuilder<OrderedChange<T>>(initialCapacity: items.Length);

        for (var itemIndex = items.Length - 1; itemIndex >= 0; --itemIndex)
            changes.Add(OrderedChange.CreateRemoval(
                index:  itemIndex + index,
                item:   items[itemIndex]));

        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Update
        };
    }
}
