using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DynamicDataVNext;

public static partial class DistinctChangeSet
{
    /// <inheritdoc cref="DistinctChangeSet{T}.CreateForAdditions(IEnumerable{T})"/>
    /// <typeparam name="T">The type of the added items.</typeparam>
    public static DistinctChangeSet<T> CreateForAdditions<T>(IEnumerable<T> items)
        => DistinctChangeSet<T>.CreateForAdditions(items);

    /// <inheritdoc cref="DistinctChangeSet{T}.CreateForAdditions(ReadOnlySpan{T})"/>
    /// <typeparam name="T">The type of the added items.</typeparam>
    public static DistinctChangeSet<T> CreateForAdditions<T>(ReadOnlySpan<T> items)
        => DistinctChangeSet<T>.CreateForAdditions(items);
}

public readonly partial record struct DistinctChangeSet<T>
{
    /// <inheritdoc cref="CreateForAdditions(ReadOnlySpan{T})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    public static DistinctChangeSet<T> CreateForAdditions(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        
        var isItemCountValid = items.TryGetNonEnumeratedCount(out var itemCount);
        if (isItemCountValid && (itemCount is 0))
            return Empty;
        
        var changes = isItemCountValid
            ? ImmutableArray.CreateBuilder<DistinctChange<T>>(initialCapacity: itemCount)
            : ImmutableArray.CreateBuilder<DistinctChange<T>>();
        
        foreach (var item in items)
            changes.Add(DistinctChange.CreateAddition(item));
        
        if (changes.Count is 0)
            return Empty;
        
        return new()
        {
            Changes = changes.MoveToOrCreateImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <summary>
    /// Creates a new <see cref="DistinctChangeSet{T}"/> representing the <see cref="DistinctChangeType.Addition"/> of a range of items to a collection.
    /// </summary>
    /// <param name="items">The added items.</param>
    /// <returns>A <see cref="DistinctChangeSet{T}"/> describing the addition of the given items, or <see cref="Empty"/> if no items were given.</returns>
    public static DistinctChangeSet<T> CreateForAdditions(ReadOnlySpan<T> items)
    {
        if (items.Length is 0)
            return Empty;
        
        var changes = ImmutableArray.CreateBuilder<DistinctChange<T>>(initialCapacity: items.Length);
        
        foreach (var item in items)
            changes.Add(DistinctChange.CreateAddition(item));
        
        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Update
        };
    }
}
