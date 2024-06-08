using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DynamicDataVNext;

/// <summary>
/// Contains convenience methods for creating <see cref="SortedChangeSet{T}"/> values.
/// </summary>
public static partial class SortedChangeSet
{
    /// <summary>
    /// Creates a new <see cref="SortedChangeSet{T}"/> representing the clearing of a sorted collection.
    /// </summary>
    /// <typeparam name="T">The type of items being removed.</typeparam>
    /// <param name="items">The items being removed, and their indexes.</param>
    /// <returns>A <see cref="SortedChangeSet{T}"/> describing the clear operation.</returns>
    public static SortedChangeSet<T> Clear<T>(IReadOnlyList<T> items)
    {
        var changes = ImmutableArray.CreateBuilder<SortedChange<T>>(initialCapacity: items.Count);

        // List changes in reverse order, to preserve correctness of indexing.
        for(var index = items.Count - 1; index >= 0; --index)
            changes.Add(SortedChange.Removal(
                index:  index,
                item:   items[index]));

        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Clear
        };
    }

    /// <inheritdoc cref="Clear{T}(IReadOnlyList{T})"/>
    public static SortedChangeSet<T> Clear<T>(ReadOnlySpan<T> items)
    {
        var changes = ImmutableArray.CreateBuilder<SortedChange<T>>(initialCapacity: items.Length);

        // List changes in reverse order, to preserve correctness of indexing.
        for(var index = items.Length - 1; index >= 0; --index)
            changes.Add(SortedChange.Removal(
                index:  index,
                item:   items[index]));

        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Clear
        };
    }

    /// <summary>
    /// Creates a new <see cref="SortedChangeSet{T}"/> representing the insertion of a single item.
    /// </summary>
    /// <typeparam name="T">The type of item being inserted.</typeparam>
    /// <param name="index">The index at which the item is being inserted.</param>
    /// <param name="item">The item being inserted.</param>
    /// <returns>A <see cref="SortedChangeSet{T}"/> describing the insertion of the given item.</returns>
    public static SortedChangeSet<T> Insertion<T>(
            int index,
            T   item)
        => new()
        {
            Changes = ImmutableArray.Create(SortedChange.Insertion(
                index:  index,
                item:   item)),
            Type    = ChangeSetType.Update
        };

    /// <summary>
    /// Creates a new <see cref="SortedChangeSet{T}"/> representing the movement of a single item.
    /// </summary>
    /// <typeparam name="T">The type of item being moved.</typeparam>
    /// <param name="oldIndex">The index of the item, before being moved.</param>
    /// <param name="newIndex">The index of the item, after being moved..</param>
    /// <param name="item">The item being moved.</param>
    /// <returns>A <see cref="SortedChangeSet{T}"/> describing the movement of the given item.</returns>
    public static SortedChangeSet<T> Movement<T>(
            int oldIndex,
            int newIndex,
            T   item)
        => new()
        {
            Changes = ImmutableArray.Create(SortedChange.Movement(
                oldIndex:   oldIndex,
                newIndex:   newIndex,
                item:       item)),
            Type    = ChangeSetType.Update
        };

    /// <summary>
    /// Creates a new <see cref="SortedChangeSet{T}"/> representing the insertion of a range of items.
    /// </summary>
    /// <typeparam name="T">The type of items being inserted.</typeparam>
    /// <param name="index">The index at which the items are being inserted.</param>
    /// <param name="items">The items being inserted.</param>
    /// <returns>A <see cref="SortedChangeSet{T}"/> describing the insertion of the given items.</returns>
    public static SortedChangeSet<T> RangeInsertion<T>(
        int             index,
        IEnumerable<T>  items)
    {
        if (!items.TryGetNonEnumeratedCount(out var itemsCount))
            itemsCount = 0;

        var changes = ImmutableArray.CreateBuilder<SortedChange<T>>(initialCapacity: itemsCount);

        var insertionIndex = index;
        foreach(var item in items)
            changes.Add(SortedChange.Insertion(
                index:  insertionIndex++,
                item:   item));

        return new()
        {
            Changes = changes.MoveToOrCreateImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <inheritdoc cref="RangeInsertion{T}(int, IEnumerable{T})"/>
    public static SortedChangeSet<T> RangeInsertion<T>(
        int             index,
        ReadOnlySpan<T> items)
    {
        var changes = ImmutableArray.CreateBuilder<SortedChange<T>>(initialCapacity: items.Length);

        var insertionIndex = index;
        foreach(var item in items)
            changes.Add(SortedChange.Insertion(
                index:  insertionIndex++,
                item:   item));

        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <summary>
    /// Creates a new <see cref="SortedChangeSet{T}"/> representing the removal of a range of items.
    /// </summary>
    /// <typeparam name="T">The type of items being inserted.</typeparam>
    /// <typeparam name="TItems">The type of collection or sequence containing the items being removed. Allows JIT optimizations, depending on the type given.</typeparam>
    /// <param name="index">The index at which the sequence of removed items begins.</param>
    /// <param name="items">The items being removed.</param>
    /// <returns>A <see cref="SortedChangeSet{T}"/> describing the removal of the given items.</returns>
    public static SortedChangeSet<T> RangeRemoval<T, TItems>(
        int                 index,
        IReadOnlyList<T>    items)
    {
        var changes = ImmutableArray.CreateBuilder<SortedChange<T>>(initialCapacity: items.Count);

        // List changes in reverse order, to preserve correctness of indexing.
        for(var removalIndex = index + items.Count - 1; removalIndex >= index; --removalIndex)
            changes.Add(SortedChange.Removal(
                index:  index,
                item:   items[index]));

        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <inheritdoc cref="RangeRemoval{T, TItems}(int, IReadOnlyList{T})"/>
    public static SortedChangeSet<T> RangeRemoval<T, TItems>(
        int             index,
        ReadOnlySpan<T> items)
    {
        var changes = ImmutableArray.CreateBuilder<SortedChange<T>>(initialCapacity: items.Length);

        // List changes in reverse order, to preserve correctness of indexing.
        for(var removalIndex = index + items.Length - 1; removalIndex >= index; --removalIndex)
            changes.Add(SortedChange.Removal(
                index:  index,
                item:   items[index]));

        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <summary>
    /// Creates a new <see cref="SortedChangeSet{T}"/> representing the removal of a single item.
    /// </summary>
    /// <typeparam name="T">The type of item being removed.</typeparam>
    /// <param name="index">The index of the item being removed.</param>
    /// <param name="item">The item being removed.</param>
    /// <returns>A <see cref="SortedChangeSet{T}"/> describing the removal of the given item.</returns>
    public static SortedChangeSet<T> Removal<T>(
            int index,
            T   item)
        => new()
        {
            Changes = ImmutableArray.Create(SortedChange.Removal(
                index:  index,
                item:   item)),
            Type    = ChangeSetType.Update
        };

    /// <summary>
    /// Creates a new <see cref="SortedChangeSet{T}"/> representing the replacement of a single item.
    /// </summary>
    /// <typeparam name="T">The type of items being added and removed.</typeparam>
    /// <param name="index">The index of the item being replaced.</param>
    /// <param name="oldItem">The item being replaced.</param>
    /// <param name="newItem">The replacement item.</param>
    /// <returns>A <see cref="SortedChangeSet{T}"/> describing the replacement of the given items.</returns>
    public static SortedChangeSet<T> Replacement<T>(
            int index,
            T   oldItem,
            T   newItem)
        => new()
        {
            Changes = ImmutableArray.Create(SortedChange.Replacement(
                index:      index,
                oldItem:    oldItem,
                newItem:    newItem)),
            Type    = ChangeSetType.Update
        };

    /// <summary>
    /// Creates a new <see cref="SortedChangeSet{T}"/> representing the resetting of items in a sorted collection.
    /// </summary>
    /// <typeparam name="T">The type of items being added and removed.</typeparam>
    /// <param name="oldSortedItems">The items being removed, and their indexes.</param>
    /// <param name="newSortedItems">The items being added, in the order they should appear.</param>
    /// <returns>A <see cref="SortedChangeSet{T}"/> describing the given reset operation.</returns>
    public static SortedChangeSet<T> Reset<T>(
        IReadOnlyList<T>    oldSortedItems,
        IEnumerable<T>      newSortedItems)
    {
        if (newSortedItems.TryGetNonEnumeratedCount(out var newSortedItemsCount))
            newSortedItemsCount = 0;

        var changes = ImmutableArray.CreateBuilder<SortedChange<T>>(initialCapacity: oldSortedItems.Count + newSortedItemsCount);
        int index;

        // List removed items in reverse order, to preserve correctness of indexing.
        for(index = oldSortedItems.Count - 1; index >= 0; --index)
            changes.Add(SortedChange.Removal(
                index:  index,
                item:   oldSortedItems[index]));

        index = 0;
        foreach(var item in newSortedItems)
            changes.Add(SortedChange.Insertion(
                index:  index++,
                item:   item));

        return new()
        {
            Changes = changes.MoveToOrCreateImmutable(),
            Type    = ChangeSetType.Reset
        };
    }

    /// <summary>
    /// Creates a new <see cref="SortedChangeSet{T}"/> representing the resetting of items in a sorted collection.
    /// </summary>
    /// <typeparam name="T">The type of items being added and removed.</typeparam>
    /// <param name="oldSortedItems">The items being removed, and their indexes.</param>
    /// <param name="newSortedItems">The items being added, in the order they should appear.</param>
    /// <returns>A <see cref="SortedChangeSet{T}"/> describing the given reset operation.</returns>
    public static SortedChangeSet<T> Reset<T>(
        ReadOnlySpan<T> oldSortedItems,
        ReadOnlySpan<T> newSortedItems)
    {
        var changes = ImmutableArray.CreateBuilder<SortedChange<T>>(initialCapacity: oldSortedItems.Length + newSortedItems.Length);
        int index;

        // List removed items in reverse order, to preserve correctness of indexing.
        for(index = oldSortedItems.Length - 1; index >= 0; --index)
            changes.Add(SortedChange.Removal(
                index:  index,
                item:   oldSortedItems[index]));

        index = 0;
        foreach(var item in newSortedItems)
            changes.Add(SortedChange.Insertion(
                index:  index++,
                item:   item));

        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Reset
        };
    }

    /// <summary>
    /// Creates a new <see cref="SortedChangeSet{T}"/> representing the replacement of a single item by another, and their movement within a collection.
    /// </summary>
    /// <typeparam name="T">The type of items being added and removed.</typeparam>
    /// <param name="oldIndex">The index of <paramref name="oldItem"/>, before being moved.</param>
    /// <param name="oldItem">The item being replaced.</param>
    /// <param name="newIndex">The index of <paramref name="newItem"/>, after being moved..</param>
    /// <param name="newItem">The replacement item.</param>
    /// <returns>A <see cref="SortedChangeSet{T}"/> describing the replacement and movement of the given items.</returns>
    public static SortedChangeSet<T> Update<T>(
            int oldIndex,
            T   oldItem,
            int newIndex,
            T   newItem)
        => new()
        {
            Changes = ImmutableArray.Create(SortedChange.Update(
                oldIndex:   oldIndex,
                oldItem:    oldItem,
                newIndex:   newIndex,
                newItem:    newItem)),
            Type    = ChangeSetType.Update
        };
}

/// <summary>
/// Describes a set of single-item changes made (or to be made) to a sorted collection
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public readonly record struct SortedChangeSet<T>
{
    /// <summary>
    /// The set of single-item changes to the collection.
    /// Changes must be applied to the collection in the order they appear here.
    /// </summary>
    public required ImmutableArray<SortedChange<T>> Changes { get; init; }

    /// <summary>
    /// The type of operation that created this changeset, or that this changeset represents.
    /// </summary>
    public required ChangeSetType Type { get; init; }
}
