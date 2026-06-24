using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DynamicDataVNext;

public static partial class OrderedChangeSet
{
    /// <inheritdoc cref="OrderedChangeSet{T}.CreateForClear(IReadOnlyList{T})"/>
    /// <typeparam name="T">The type of the removed items.</typeparam>
    public static OrderedChangeSet<T> CreateForClear<T>(IReadOnlyList<T> items)
        => OrderedChangeSet<T>.CreateForClear(items);

    /// <inheritdoc cref="OrderedChangeSet{T}.CreateForClear(ReadOnlySpan{T})"/>
    /// <typeparam name="T">The type of the removed items.</typeparam>
    public static OrderedChangeSet<T> CreateForClear<T>(ReadOnlySpan<T> items)
        => OrderedChangeSet<T>.CreateForClear(items);
}

public readonly partial record struct OrderedChangeSet<T>
{
    /// <inheritdoc cref="CreateForClear(ReadOnlySpan{T})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    public static OrderedChangeSet<T> CreateForClear(IReadOnlyList<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        if (items.Count is 0)
            return Empty;

        var changes = ImmutableArray.CreateBuilder<OrderedChange<T>>(initialCapacity: items.Count);

        for(var index = items.Count - 1; index >= 0; --index)
            changes.Add(OrderedChange.CreateRemoval(
                index:  index,
                item:   items[index]));

        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Clear
        };
    }

    /// <summary>
    /// Creates a new <see cref="OrderedChangeSet{T}"/> representing a <see cref="ChangeSetType.Clear"/> operation.
    /// </summary>
    /// <param name="items">The removed items, listed in their source collection order.</param>
    /// <returns>An <see cref="OrderedChangeSet{T}"/> describing the clear operation, or <see cref="Empty"/> if no items were given.</returns>
    /// <remarks>
    /// The generated sequence of changes will involve removing the items in the reverse order that they are listed in <paramref name="items"/>. This optimizes the removal of items from <see cref="List{T}"/> or similar data structures, should consumers be forced to process the removals one-at-a-time. The recommendation, however, is for consumers to check for a changeset type of <see cref="ChangeSetType.Clear"/> and simply call a .Clear() method. 
    /// </remarks>
    public static OrderedChangeSet<T> CreateForClear(ReadOnlySpan<T> items)
    {
        if (items.Length is 0)
            return Empty;

        var changes = ImmutableArray.CreateBuilder<OrderedChange<T>>(initialCapacity: items.Length);

        for(var index = items.Length - 1; index >= 0; --index)
            changes.Add(OrderedChange.CreateRemoval(
                index:  index,
                item:   items[index]));

        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Clear
        };
    }
}
