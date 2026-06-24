using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DynamicDataVNext;

public static partial class OrderedChangeSet
{
    /// <inheritdoc cref="OrderedChangeSet{T}.CreateForReset(IEnumerable{T})"/>
    /// <typeparam name="T">The type of the involved items.</typeparam>
    public static OrderedChangeSet<T> CreateForReset<T>(IEnumerable<T> addedItems)
        => OrderedChangeSet<T>.CreateForReset(addedItems);
        
    /// <inheritdoc cref="OrderedChangeSet{T}.CreateForReset(ReadOnlySpan{T})"/>
    /// <typeparam name="T">The type of the involved items.</typeparam>
    public static OrderedChangeSet<T> CreateForReset<T>(ReadOnlySpan<T> addedItems)
        => OrderedChangeSet<T>.CreateForReset(addedItems);

    /// <inheritdoc cref="OrderedChangeSet{T}.CreateForReset(IReadOnlyList{T}, IEnumerable{T})"/>
    /// <typeparam name="T">The type of the involved items.</typeparam>
    public static OrderedChangeSet<T> CreateForReset<T>(
            IReadOnlyList<T>    removedItems,
            IEnumerable<T>      addedItems)
        => OrderedChangeSet<T>.CreateForReset(
            removedItems:   removedItems,
            addedItems:     addedItems);

    /// <inheritdoc cref="OrderedChangeSet{T}.CreateForReset(ReadOnlySpan{T}, ReadOnlySpan{T})"/>
    /// <typeparam name="T">The type of the involved items.</typeparam>
    public static OrderedChangeSet<T> CreateForReset<T>(
            ReadOnlySpan<T> removedItems,
            ReadOnlySpan<T> addedItems)
        => OrderedChangeSet<T>.CreateForReset(
            removedItems:   removedItems,
            addedItems:     addedItems);
}

public readonly partial record struct OrderedChangeSet<T>
{
    /// <inheritdoc cref="CreateForReset(ReadOnlySpan{T})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="addedItems"/>.</exception>
    public static OrderedChangeSet<T> CreateForReset(IEnumerable<T> addedItems)
    {
        ArgumentNullException.ThrowIfNull(addedItems);

        var isAddedItemCountValid = addedItems.TryGetNonEnumeratedCount(out var addedItemCount);
        if (isAddedItemCountValid && (addedItemCount is 0))
            return Empty;

        var changes = isAddedItemCountValid
            ? ImmutableArray.CreateBuilder<OrderedChange<T>>(initialCapacity: addedItemCount)
            : ImmutableArray.CreateBuilder<OrderedChange<T>>();

        foreach(var item in addedItems)
            changes.Add(OrderedChange.CreateInsertion(
                index:  changes.Count,
                item:   item));

        return new()
        {
            Changes             = changes.MoveToOrCreateImmutable(),
            Type                = ChangeSetType.Reset,
            FirstAdditionIndex  = 0
        };
    }

    /// <summary>
    /// Creates a new <see cref="OrderedChangeSet{T}"/> representing a <see cref="ChangeSetType.Reset"/> operation upon an empty collection.
    /// </summary>
    /// <param name="addedItems">The items added by the reset.</param>
    /// <returns>An <see cref="OrderedChangeSet{T}"/> describing the given reset operation, or <see cref="Empty"/> if no items were given.</returns>
    public static OrderedChangeSet<T> CreateForReset(ReadOnlySpan<T> addedItems)
    {
        if (addedItems.Length is 0)
            return Empty;
    
        var changes = ImmutableArray.CreateBuilder<OrderedChange<T>>(initialCapacity: addedItems.Length);

        foreach(var item in addedItems)
            changes.Add(OrderedChange.CreateInsertion(
                index:  changes.Count,
                item:   item));

        return new()
        {
            Changes             = changes.MoveToImmutable(),
            Type                = ChangeSetType.Reset,
            FirstAdditionIndex  = 0
        };
    }

    /// <inheritdoc cref="CreateForReset(ReadOnlySpan{T}, ReadOnlySpan{T})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="removedItems"/> and <paramref name="addedItems"/>.</exception>
    public static OrderedChangeSet<T> CreateForReset(
        IReadOnlyList<T>    removedItems,
        IEnumerable<T>      addedItems)
    {
        ArgumentNullException.ThrowIfNull(removedItems);
        ArgumentNullException.ThrowIfNull(addedItems);

        var isAddedItemCountValid = addedItems.TryGetNonEnumeratedCount(out var addedItemCount);
        if (        (removedItems.Count is 0)
                &&  isAddedItemCountValid && (addedItemCount is 0))
            return Empty;

        var changes = (removedItems.Count, isAddedItemCountValid) switch
        {
            (_,     true)   => ImmutableArray.CreateBuilder<OrderedChange<T>>(initialCapacity: removedItems.Count + addedItemCount),
            (>0,    false)  => ImmutableArray.CreateBuilder<OrderedChange<T>>(initialCapacity: removedItems.Count),
            _               => ImmutableArray.CreateBuilder<OrderedChange<T>>()
        };

        int index;
        for(index = removedItems.Count - 1; index >= 0; --index)
            changes.Add(OrderedChange.CreateRemoval(
                index:  index,
                item:   removedItems[index]));

        index = 0;
        foreach(var item in addedItems)
            changes.Add(OrderedChange.CreateInsertion(
                index:  index++,
                item:   item));

        if (changes.Count is 0)
            return Empty;

        return new()
        {
            Changes             = changes.MoveToOrCreateImmutable(),
            Type                = (changes.Count == removedItems.Count)
                ? ChangeSetType.Clear
                : ChangeSetType.Reset,
            FirstAdditionIndex  = removedItems.Count
        };
    }

    /// <summary>
    /// Creates a new <see cref="OrderedChangeSet{T}"/> representing a <see cref="ChangeSetType.Reset"/> operation.
    /// </summary>
    /// <param name="removedItems">The items removed by the reset.</param>
    /// <param name="addedItems">The items added by the reset.</param>
    /// <returns>An <see cref="OrderedChangeSet{T}"/> describing the given reset operation, or a <see cref="ChangeSetType.Clear"/> operation if <paramref name="addedItems"/> is empty, or <see cref="Empty"/> if no items were given.</returns>
    /// <remarks>
    /// The generated sequence of changes will involve removing the items in the reverse order that they are listed in <paramref name="removedItems"/>. This helps optimize the removal of items from <see cref="List{T}"/> or similar data structures, should consumers be forced to process the removals one-at-a-time, instead of calling a .RemoveRange() method. 
    /// </remarks>
    public static OrderedChangeSet<T> CreateForReset(
        ReadOnlySpan<T> removedItems,
        ReadOnlySpan<T> addedItems)
    {
        if ((removedItems.Length is 0) && (addedItems.Length is 0))
            return Empty;
    
        var changes = ImmutableArray.CreateBuilder<OrderedChange<T>>(initialCapacity: removedItems.Length + addedItems.Length);

        int index;
        for(index = removedItems.Length - 1; index >= 0; --index)
            changes.Add(OrderedChange.CreateRemoval(
                index:  index,
                item:   removedItems[index]));

        index = 0;
        foreach(var item in addedItems)
            changes.Add(OrderedChange.CreateInsertion(
                index:  index++,
                item:   item));

        return new()
        {
            Changes             = changes.MoveToImmutable(),
            Type                = (addedItems.Length is 0)
                ? ChangeSetType.Clear
                : ChangeSetType.Reset,
            FirstAdditionIndex  = removedItems.Length
        };
    }
}
