using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DynamicDataVNext;

public static partial class DistinctChangeSet
{
    /// <inheritdoc cref="DistinctChangeSet{T}.CreateForRemovals(IEnumerable{T})"/>
    /// <typeparam name="T">The type of the removed items.</typeparam>
    public static DistinctChangeSet<T> CreateForRemovals<T>(IEnumerable<T> items)
        => DistinctChangeSet<T>.CreateForRemovals(items);

    /// <inheritdoc cref="DistinctChangeSet{T}.CreateForRemovals(ReadOnlySpan{T})"/>
    /// <typeparam name="T">The type of the removed items.</typeparam>
    public static DistinctChangeSet<T> CreateForRemovals<T>(ReadOnlySpan<T> items)
        => DistinctChangeSet<T>.CreateForRemovals(items);
}

public readonly partial record struct DistinctChangeSet<T>
{
    /// <inheritdoc cref="CreateForRemovals(ReadOnlySpan{T})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    public static DistinctChangeSet<T> CreateForRemovals(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        var isItemCountValid = items.TryGetNonEnumeratedCount(out var itemCount);
        
        if (isItemCountValid && (itemCount is 0))
            return Empty;
        
        var changes = isItemCountValid
            ? ImmutableArray.CreateBuilder<DistinctChange<T>>(initialCapacity: itemCount)
            : ImmutableArray.CreateBuilder<DistinctChange<T>>();
        
        foreach (var item in items)
            changes.Add(DistinctChange.CreateRemoval(item));
        
        if (changes.Count is 0)
            return Empty;
        
        return new()
        {
            Changes = changes.MoveToOrCreateImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <summary>
    /// Creates a new <see cref="DistinctChangeSet{T}"/> representing the <see cref="DistinctChangeType.Removal"/> of a range of items.
    /// </summary>
    /// <param name="items">The removed items.</param>
    /// <returns>A <see cref="DistinctChangeSet{T}"/> describing the removal of the given items, or <see cref="Empty"/> if no items were given.</returns>
    public static DistinctChangeSet<T> CreateForRemovals(ReadOnlySpan<T> items)
    {
        if (items.Length is 0)
            return Empty;
        
        var changes = ImmutableArray.CreateBuilder<DistinctChange<T>>(initialCapacity: items.Length);
        
        foreach (var item in items)
            changes.Add(DistinctChange.CreateRemoval(item));
        
        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Update
        };
    }
}
