using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DynamicDataVNext;

public static partial class DistinctChangeSet
{
    /// <inheritdoc cref="DistinctChangeSet{T}.CreateForReset(IEnumerable{T})"/>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    public static DistinctChangeSet<T> CreateForReset<T>(IEnumerable<T> addedItems)
        => DistinctChangeSet<T>.CreateForReset(addedItems);
    
    /// <inheritdoc cref="DistinctChangeSet{T}.CreateForReset(ReadOnlySpan{T})"/>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    public static DistinctChangeSet<T> CreateForReset<T>(ReadOnlySpan<T> addedItems)
        => DistinctChangeSet<T>.CreateForReset(addedItems);
    
    /// <inheritdoc cref="DistinctChangeSet{T}.CreateForReset(IEnumerable{T}, IEnumerable{T})"/>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    public static DistinctChangeSet<T> CreateForReset<T>(
            IEnumerable<T> removedItems,
            IEnumerable<T> addedItems)
        => DistinctChangeSet<T>.CreateForReset(
            removedItems:   removedItems,
            addedItems:     addedItems);

    /// <inheritdoc cref="DistinctChangeSet{T}.CreateForReset(ReadOnlySpan{T}, ReadOnlySpan{T})"/>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    public static DistinctChangeSet<T> CreateForReset<T>(
            ReadOnlySpan<T> removedItems,
            ReadOnlySpan<T> addedItems)
        => DistinctChangeSet<T>.CreateForReset(
            removedItems:   removedItems,
            addedItems:     addedItems);
}

public readonly partial record struct DistinctChangeSet<T>
{
    /// <inheritdoc cref="CreateForReset(ReadOnlySpan{T})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="addedItems"/>.</exception>
    public static DistinctChangeSet<T> CreateForReset(IEnumerable<T> addedItems)
    {
        ArgumentNullException.ThrowIfNull(addedItems);
        
        var isAddedItemCountValid = addedItems.TryGetNonEnumeratedCount(out var addedItemCount);
        if (isAddedItemCountValid && (addedItemCount is 0))
            return Empty;
        
        var changes = isAddedItemCountValid
            ? ImmutableArray.CreateBuilder<DistinctChange<T>>(initialCapacity: addedItemCount)
            : ImmutableArray.CreateBuilder<DistinctChange<T>>();
        
        foreach (var item in addedItems)
            changes.Add(DistinctChange.CreateAddition(item));
        
        if (changes.Count is 0)
            return Empty;
        
        return new()
        {
            Changes             = changes.MoveToOrCreateImmutable(),
            Type                = ChangeSetType.Reset,
            FirstAdditionIndex  = 0
        };
    }

    /// <summary>
    /// Creates a new <see cref="DistinctChangeSet{T}"/> representing a <see cref="ChangeSetType.Reset"/> operation, upon an empty collection.
    /// </summary>
    /// <param name="addedItems">The items added to the collection, by the reset.</param>
    /// <returns>A <see cref="DistinctChangeSet{T}"/> describing the given reset operation, or <see cref="Empty"/> if no items were given.</returns>
    public static DistinctChangeSet<T> CreateForReset(ReadOnlySpan<T> addedItems)
    {
        if (addedItems.Length is 0)
            return Empty;
        
        var changes = ImmutableArray.CreateBuilder<DistinctChange<T>>(initialCapacity: addedItems.Length);
        
        foreach (var item in addedItems)
            changes.Add(DistinctChange.CreateAddition(item));
        
        return new()
        {
            Changes             = changes.MoveToImmutable(),
            Type                = ChangeSetType.Reset,
            FirstAdditionIndex  = 0
        };
    }

    /// <inheritdoc cref="CreateForReset(ReadOnlySpan{T}, ReadOnlySpan{T})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="removedItems"/> and <paramref name="addedItems"/>.</exception>
    public static DistinctChangeSet<T> CreateForReset(
        IEnumerable<T> removedItems,
        IEnumerable<T> addedItems)
    {
        ArgumentNullException.ThrowIfNull(removedItems);
        ArgumentNullException.ThrowIfNull(addedItems);
        
        var isRemovedItemCountValid = removedItems  .TryGetNonEnumeratedCount(out var removedItemCount);
        var isAddedItemCountValid   = addedItems    .TryGetNonEnumeratedCount(out var addedItemCount);
        
        if (        isRemovedItemCountValid && (removedItemCount is 0)
                &&  isAddedItemCountValid   && (addedItemCount is 0))
            return Empty;
        
        var changes = (isRemovedItemCountValid || isAddedItemCountValid)
            ? ImmutableArray.CreateBuilder<DistinctChange<T>>(initialCapacity: removedItemCount + addedItemCount)
            : ImmutableArray.CreateBuilder<DistinctChange<T>>();
        
        foreach (var item in removedItems)
            changes.Add(DistinctChange.CreateRemoval(item));
        var removalCount = changes.Count;
        
        foreach (var item in addedItems)
            changes.Add(DistinctChange.CreateAddition(item));
        var additionCount = changes.Count - removalCount;
        
        if (changes.Count is 0)
            return Empty;
        
        return new()
        {
            Changes             = changes.MoveToOrCreateImmutable(),
            Type                = (removalCount, additionCount) switch
            {
                (>0, 0) => ChangeSetType.Clear,
                _       => ChangeSetType.Reset
            },
            FirstAdditionIndex  = removalCount
        };
    }

    /// <inheritdoc cref="CreateForReset(ReadOnlySpan{T})"/>
    /// <param name="removedItems">The items removed by the reset.</param>
    /// <param name="addedItems">The items added by the reset.</param>
    /// <returns>A <see cref="DistinctChangeSet{T}"/> describing the given reset operation, or a <see cref="ChangeSetType.Clear"/> operation, if <paramref name="removedItems"/> is empty, or <see cref="Empty"/> if no items were given.</returns>
    public static DistinctChangeSet<T> CreateForReset(
        ReadOnlySpan<T> removedItems,
        ReadOnlySpan<T> addedItems)
    {
        if (        (removedItems.Length is 0)
                &&  (addedItems.Length is 0))
            return Empty;
        
        var changes = ImmutableArray.CreateBuilder<DistinctChange<T>>(initialCapacity: removedItems.Length + addedItems.Length);
        
        foreach (var item in removedItems)
            changes.Add(DistinctChange.CreateRemoval(item));
        
        foreach (var item in addedItems)
            changes.Add(DistinctChange.CreateAddition(item));
        
        return new()
        {
            Changes             = changes.MoveToImmutable(),
            Type                = (removedItems.Length, addedItems.Length) switch
            {
                (>0, 0) => ChangeSetType.Clear,
                _       => ChangeSetType.Reset
            },
            FirstAdditionIndex  = removedItems.Length
        };
    }
}
