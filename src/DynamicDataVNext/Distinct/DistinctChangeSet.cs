using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DynamicDataVNext;

/// <summary>
/// Contains convenience methods for creating <see cref="DistinctChangeSet{T}"/> values.
/// </summary>
public static partial class DistinctChangeSet
{
    /// <summary>
    /// Creates a new <see cref="DistinctChangeSet{T}"/> representing the addition of a single item.
    /// </summary>
    /// <typeparam name="T">The type of the item being added.</typeparam>
    /// <param name="key">The item's key.</param>
    /// <param name="item">The item being added.</param>
    /// <returns>A <see cref="DistinctChangeSet{T}"/> describing the addition of the given item.</returns>
    public static DistinctChangeSet<T> Addition<T>(T item)
        => new()
        {
            Changes = ImmutableArray.Create(DistinctChange.Addition(item)),
            Type    = ChangeSetType.Update
        };

    /// <summary>
    /// Creates a new <see cref="DistinctChangeSet{T}"/> representing the addition of a range of items.
    /// </summary>
    /// <typeparam name="T">The type of the items being added.</typeparam>
    /// <param name="items">The items being added.</param>
    /// <returns>A <see cref="DistinctChangeSet{T}"/> describing the addition of the given items.</returns>
    public static DistinctChangeSet<T> Addition<T>(IEnumerable<T> items)
    {
        if (!items.TryGetNonEnumeratedCount(out var itemsCount))
            itemsCount = 0;

        var changes = ImmutableArray.CreateBuilder<DistinctChange<T>>(initialCapacity: itemsCount);

        foreach(var item in items)
            changes.Add(DistinctChange.Addition(item));

        return new()
        {
            Changes = changes.MoveToOrCreateImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <inheritdoc cref="Addition{T}(IEnumerable{T})"/>
    public static DistinctChangeSet<T> Addition<T>(ReadOnlySpan<T> items)
    {
        var changes = ImmutableArray.CreateBuilder<DistinctChange<T>>(initialCapacity: items.Length);

        foreach(var item in items)
            changes.Add(DistinctChange.Addition(item));

        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <summary>
    /// Creates a new <see cref="DistinctChangeSet{T}"/> representing the clearing of a collection of distinct items.
    /// </summary>
    /// <typeparam name="T">The type of the items being removed.</typeparam>
    /// <param name="items">The items being removed.</param>
    /// <returns>A <see cref="DistinctChangeSet{T}"/> describing the clearing of the collection.</returns>
    public static DistinctChangeSet<T> Clear<T>(IEnumerable<T> items)
    {
        if (!items.TryGetNonEnumeratedCount(out var itemsCount))
            itemsCount = 0;

        var changes = ImmutableArray.CreateBuilder<DistinctChange<T>>(initialCapacity: itemsCount);

        foreach(var item in items)
            changes.Add(DistinctChange.Removal(item));

        return new()
        {
            Changes = changes.MoveToOrCreateImmutable(),
            Type    = ChangeSetType.Clear
        };
    }

    /// <inheritdoc cref="Clear{T}(IEnumerable{T})"/>
    public static DistinctChangeSet<T> Clear<T>(ReadOnlySpan<T> items)
    {
        var changes = ImmutableArray.CreateBuilder<DistinctChange<T>>(initialCapacity: items.Length);

        foreach(var item in items)
            changes.Add(DistinctChange.Removal(item));

        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Clear
        };
    }

    /// <summary>
    /// Creates a new <see cref="DistinctChangeSet{T}"/> representing the removal of a single item.
    /// </summary>
    /// <typeparam name="T">The type of the item being removed.</typeparam>
    /// <param name="item">The item being removed.</param>
    /// <returns>A <see cref="DistinctChangeSet{T}"/> describing the removal of the given item.</returns>
    public static DistinctChangeSet<T> Removal<T>(T item)
        => new()
        {
            Changes = ImmutableArray.Create(DistinctChange.Removal(item)),
            Type    = ChangeSetType.Update
        };

    /// <summary>
    /// Creates a new <see cref="DistinctChangeSet{T}"/> representing the removal of a range of items.
    /// </summary>
    /// <typeparam name="T">The type of the items being removed.</typeparam>
    /// <param name="items">The items being removed.</param>
    /// <returns>A <see cref="DistinctChangeSet{T}"/> describing the removal of the given items.</returns>
    public static DistinctChangeSet<T> Removal<T>(IEnumerable<T> items)
    {
        if (!items.TryGetNonEnumeratedCount(out var itemsCount))
            itemsCount = 0;

        var changes = ImmutableArray.CreateBuilder<DistinctChange<T>>(initialCapacity: itemsCount);

        foreach(var item in items)
            changes.Add(DistinctChange.Removal(item));

        return new()
        {
            Changes = changes.MoveToOrCreateImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <inheritdoc cref="Removal{T}(IEnumerable{T})"/>
    public static DistinctChangeSet<T> Removal<T>(ReadOnlySpan<T> items)
    {
        var changes = ImmutableArray.CreateBuilder<DistinctChange<T>>(initialCapacity: items.Length);

        foreach(var item in items)
            changes.Add(DistinctChange.Removal(item));

        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <summary>
    /// Creates a new <see cref="DistinctChangeSet{T}"/> representing the resetting of items in a collection of distinct items.
    /// </summary>
    /// <typeparam name="T">The type of the items being added and removed.</typeparam>
    /// <param name="oldItems">The items being removed.</param>
    /// <param name="newItems">The items being added.</param>
    /// <returns>A <see cref="DistinctChangeSet{T}"/> describing the given reset operation.</returns>
    public static DistinctChangeSet<T> Reset<T>(
        IEnumerable<T> oldItems,
        IEnumerable<T> newItems)
    {
        if (!oldItems.TryGetNonEnumeratedCount(out var oldItemsCount))
            oldItemsCount = 0;

        if (!newItems.TryGetNonEnumeratedCount(out var newItemsCount))
            newItemsCount = 0;

        var changes = ImmutableArray.CreateBuilder<DistinctChange<T>>(initialCapacity: oldItemsCount + newItemsCount);

        foreach(var item in oldItems)
            changes.Add(DistinctChange.Removal(item));

        foreach(var item in newItems)
            changes.Add(DistinctChange.Addition(item));

        return new()
        {
            Changes = changes.MoveToOrCreateImmutable(),
            Type    = ChangeSetType.Reset
        };
    }

    /// <inheritdoc cref="Reset{T}(IEnumerable{T}, IEnumerable{T})"/>
    public static DistinctChangeSet<T> Reset<T>(
        ReadOnlySpan<T> oldItems,
        ReadOnlySpan<T> newItems)
    {
        var changes = ImmutableArray.CreateBuilder<DistinctChange<T>>(initialCapacity: oldItems.Length + newItems.Length);

        foreach(var item in oldItems)
            changes.Add(DistinctChange.Removal(item));

        foreach(var item in newItems)
            changes.Add(DistinctChange.Addition(item));

        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Reset
        };
    }
}

/// <summary>
/// Describes a single-item change made (or to be made) upon a collection of distinct items.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public readonly record struct DistinctChangeSet<T>
{
    /// <summary>
    /// The set of single-item changes to the collection.
    /// Changes must be applied to the collection in the order they appear here.
    /// </summary>
    public required ImmutableArray<DistinctChange<T>> Changes { get; init; }

    /// <summary>
    /// The type of operation that created this changeset, or that this changeset represents.
    /// </summary>
    public required ChangeSetType Type { get; init; }
}
