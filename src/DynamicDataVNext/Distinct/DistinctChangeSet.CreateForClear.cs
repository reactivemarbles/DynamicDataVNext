using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DynamicDataVNext;

public static partial class DistinctChangeSet
{
    /// <inheritdoc cref="DistinctChangeSet{T}.CreateForClear(IEnumerable{T})"/>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    public static DistinctChangeSet<T> CreateForClear<T>(IEnumerable<T> items)
        => DistinctChangeSet<T>.CreateForClear(items);

    /// <inheritdoc cref="DistinctChangeSet{T}.CreateForClear(ReadOnlySpan{T})"/>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    public static DistinctChangeSet<T> CreateForClear<T>(ReadOnlySpan<T> items)
        => DistinctChangeSet<T>.CreateForClear(items);
}

public readonly partial record struct DistinctChangeSet<T>
{
    /// <inheritdoc cref="CreateForClear(ReadOnlySpan{T})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    public static DistinctChangeSet<T> CreateForClear(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        var isItemCountValid = items.TryGetNonEnumeratedCount(out var itemCount);
        
        if (isItemCountValid && (itemCount is 0))
            return Empty;
        
        var changes = isItemCountValid
            ? ImmutableArray.CreateBuilder<DistinctChange<T>>(initialCapacity: itemCount)
            : ImmutableArray.CreateBuilder<DistinctChange<T>>();
        
        foreach (var item in items)
            changes.Add(new()
            {
                Item = item,
                Type = DistinctChangeType.Removal
            });
        
        if (changes.Count is 0)
            return Empty;
        
        return new()
        {
            Changes = changes.MoveToOrCreateImmutable(),
            Type    = ChangeSetType.Clear
        };
    }

    /// <summary>
    /// Creates a new <see cref="DistinctChangeSet{T}"/> representing a <see cref="ChangeSetType.Clear"/> operation.
    /// </summary>
    /// <param name="items">The cleared items.</param>
    /// <returns>A <see cref="DistinctChangeSet{T}"/> describing the given clear operation, or <see cref="Empty"/> if no items were given.</returns>
    public static DistinctChangeSet<T> CreateForClear(ReadOnlySpan<T> items)
    {
        if (items.Length is 0)
            return Empty;
        
        var changes = ImmutableArray.CreateBuilder<DistinctChange<T>>(initialCapacity: items.Length);
        
        foreach (var item in items)
            changes.Add(DistinctChange.CreateRemoval(item));
        
        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Clear
        };
    }
}
