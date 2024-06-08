using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DynamicDataVNext;

/// <summary>
/// Contains convenience methods for creating <see cref="KeyedChangeSet{TKey, TItem}"/> values.
/// </summary>
public static partial class KeyedChangeSet
{
    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the addition of a single item.
    /// </summary>
    /// <typeparam name="TKey">The type of the item's key.</typeparam>
    /// <typeparam name="TItem">The type of the item being added.</typeparam>
    /// <param name="key">The item's key.</param>
    /// <param name="item">The item being added.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the addition of the given item.</returns>
    public static KeyedChangeSet<TKey, TItem> Addition<TKey, TItem>(
            TKey    key,
            TItem   item)
        => new()
        {
            Changes = ImmutableArray.Create(KeyedChange.Addition(
                key:    key,
                item:   item)),
            Type    = ChangeSetType.Update
        };

    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the addition of a range of items.
    /// </summary>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the items being added.</typeparam>
    /// <param name="additions">The items being added, and their keys.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the addition of the given items.</returns>
    public static KeyedChangeSet<TKey, TItem> BulkAddition<TKey, TItem>(IEnumerable<KeyValuePair<TKey, TItem>> additions)
    {
        if (!additions.TryGetNonEnumeratedCount(out var additionsCount))
            additionsCount = 0;

        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: additionsCount);

        foreach(var pair in additions)
            changes.Add(KeyedChange.Addition(
                key:    pair.Key,
                item:   pair.Value));

        return new()
        {
            Changes = changes.MoveToOrCreateImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the addition of a range of items.
    /// </summary>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the items being added.</typeparam>
    /// <param name="items">The items being added.</param>
    /// <param name="keySelector">A selector for determining the key for each item being added.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the addition of the given items.</returns>
    public static KeyedChangeSet<TKey, TItem> BulkAddition<TKey, TItem>(
        IEnumerable<TItem>  items,
        Func<TItem, TKey>   keySelector)
    {
        if (!items.TryGetNonEnumeratedCount(out var itemsCount))
            itemsCount = 0;

        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: itemsCount);

        foreach(var item in items)
            changes.Add(KeyedChange.Addition(
                key:    keySelector.Invoke(item),
                item:   item));

        return new()
        {
            Changes = changes.MoveToOrCreateImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <inheritdoc cref="BulkAddition{TKey, TItem}(IEnumerable{TItem}, Func{TItem, TKey})"/>
    public static KeyedChangeSet<TKey, TItem> BulkAddition<TKey, TItem>(
        ReadOnlySpan<TItem>  items,
        Func<TItem, TKey>   keySelector)
    {
        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: items.Length);

        foreach(var item in items)
            changes.Add(KeyedChange.Addition(
                key:    keySelector.Invoke(item),
                item:   item));

        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the removal of a range of items.
    /// </summary>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the items being removed.</typeparam>
    /// <param name="removals">The items being removed, and their keys.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the removal of the given items.</returns>
    public static KeyedChangeSet<TKey, TItem> BulkRemoval<TKey, TItem>(IEnumerable<KeyValuePair<TKey, TItem>> removals)
    {
        if (!removals.TryGetNonEnumeratedCount(out var removalsCount))
            removalsCount = 0;

        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: removalsCount);

        foreach(var pair in removals)
            changes.Add(KeyedChange.Removal(
                key:    pair.Key,
                item:   pair.Value));

        return new()
        {
            Changes = changes.MoveToOrCreateImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the removal of a range of items.
    /// </summary>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the items being removed.</typeparam>
    /// <param name="items">The items being removed.</param>
    /// <param name="keySelector">A selector for determining the key for each item being removed.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the removal of the given items.</returns>
    public static KeyedChangeSet<TKey, TItem> BulkRemoval<TKey, TItem>(
        IEnumerable<TItem>  items,
        Func<TItem, TKey>   keySelector)
    {
        if (!items.TryGetNonEnumeratedCount(out var itemsCount))
            itemsCount = 0;

        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: itemsCount);

        foreach(var item in items)
            changes.Add(KeyedChange.Removal(
                key:    keySelector.Invoke(item),
                item:   item));

        return new()
        {
            Changes = changes.MoveToOrCreateImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <inheritdoc cref="BulkRemoval{TKey, TItem}(IEnumerable{TItem}, Func{TItem, TKey})"/>
    public static KeyedChangeSet<TKey, TItem> BulkRemoval<TKey, TItem>(
        ReadOnlySpan<TItem>  items,
        Func<TItem, TKey>   keySelector)
    {
        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: items.Length);

        foreach(var item in items)
            changes.Add(KeyedChange.Removal(
                key:    keySelector.Invoke(item),
                item:   item));

        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the clearing of a keyed collection.
    /// </summary>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the items being removed.</typeparam>
    /// <typeparam name="TRemovals">The type of collection or sequence containing the items being removed. Allows JIT optimizations, depending on the type given.</typeparam>
    /// <param name="removals">The items being removed, and their keys.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the clearing of the collection.</returns>
    public static KeyedChangeSet<TKey, TItem> Clear<TKey, TItem>(IEnumerable<KeyValuePair<TKey, TItem>> removals)
    {
        if (!removals.TryGetNonEnumeratedCount(out var removalsCount))
            removalsCount = 0;

        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: removalsCount);

        foreach(var pair in removals)
            changes.Add(KeyedChange.Removal(
                key:    pair.Key,
                item:   pair.Value));

        return new()
        {
            Changes = changes.MoveToOrCreateImmutable(),
            Type    = ChangeSetType.Clear
        };
    }

    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the clearing of a keyed collection.
    /// </summary>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the items being removed.</typeparam>
    /// <param name="items">The items being removed.</param>
    /// <param name="keySelector">A selector for determining the key for each item being removed.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the clearing of the collection.</returns>
    public static KeyedChangeSet<TKey, TItem> Clear<TKey, TItem>(
        IEnumerable<TItem>  items,
        Func<TItem, TKey>   keySelector)
    {
        if (!items.TryGetNonEnumeratedCount(out var itemsCount))
            itemsCount = 0;

        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: itemsCount);

        foreach(var item in items)
            changes.Add(KeyedChange.Removal(
                key:    keySelector.Invoke(item),
                item:   item));

        return new()
        {
            Changes = changes.MoveToOrCreateImmutable(),
            Type    = ChangeSetType.Clear
        };
    }

    /// <inheritdoc cref="Clear{TKey, TItem}(IEnumerable{TItem}, Func{TItem, TKey})"/>
    public static KeyedChangeSet<TKey, TItem> Clear<TKey, TItem>(
        ReadOnlySpan<TItem>  items,
        Func<TItem, TKey>   keySelector)
    {
        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: items.Length);

        foreach(var item in items)
            changes.Add(KeyedChange.Removal(
                key:    keySelector.Invoke(item),
                item:   item));

        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Clear
        };
    }

    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the removal of a single item.
    /// </summary>
    /// <typeparam name="TKey">The type of the item's key.</typeparam>
    /// <typeparam name="TItem">The type of the item being removed.</typeparam>
    /// <param name="key">The item's key.</param>
    /// <param name="item">The item being removed.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the removal of the given item.</returns>
    public static KeyedChangeSet<TKey, TItem> Removal<TKey, TItem>(
            TKey    key,
            TItem   item)
        => new()
        {
            Changes = ImmutableArray.Create(KeyedChange.Removal(
                key:    key,
                item:   item)),
            Type    = ChangeSetType.Update
        };

    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the replacement of a single item.
    /// </summary>
    /// <typeparam name="TKey">The type of the items' key.</typeparam>
    /// <typeparam name="TItem">The type of the items being added and removed.</typeparam>
    /// <param name="key">The items' key.</param>
    /// <param name="oldItem">The item being replaced.</param>
    /// <param name="newItem">The replacement item.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the replacement of the given items.</returns>
    public static KeyedChangeSet<TKey, TItem> Replacement<TKey, TItem>(
            TKey    key,
            TItem   oldItem,
            TItem   newItem)
        => new()
        {
            Changes = ImmutableArray.Create(KeyedChange.Replacement(
                key:        key,
                oldItem:    oldItem,
                newItem:    newItem)),
            Type    = ChangeSetType.Update
        };

    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the resetting of items in a keyed collection.
    /// </summary>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the items being added and removed.</typeparam>
    /// <param name="removals">The items being removed, and their keys.</param>
    /// <param name="additions">The items being added, and their keys.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the given reset operation.</returns>
    public static KeyedChangeSet<TKey, TItem> Reset<TKey, TItem>(
        IEnumerable<KeyValuePair<TKey, TItem>> removals,
        IEnumerable<KeyValuePair<TKey, TItem>> additions)
    {
        if (!removals.TryGetNonEnumeratedCount(out var removalsCount))
            removalsCount = 0;

        if (!additions.TryGetNonEnumeratedCount(out var additionsCount))
            additionsCount = 0;

        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: removalsCount + additionsCount);

        foreach(var pair in removals)
            changes.Add(KeyedChange.Removal(
                key:    pair.Key,
                item:   pair.Value));

        foreach(var pair in additions)
            changes.Add(KeyedChange.Addition(
                key:    pair.Key,
                item:   pair.Value));

        return new()
        {
            Changes = changes.MoveToOrCreateImmutable(),
            Type    = ChangeSetType.Reset
        };
    }

    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the resetting of items in a keyed collection.
    /// </summary>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the items being added and removed.</typeparam>
    /// <param name="oldItems">The items being removed.</param>
    /// <param name="newItems">The items being added.</param>
    /// <param name="keySelector">A selector for determining the key for each item being added or removed.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the given reset operation.</returns>
    public static KeyedChangeSet<TKey, TItem> Reset<TKey, TItem>(
        IEnumerable<TItem>  oldItems,
        IEnumerable<TItem>  newItems,
        Func<TItem, TKey>   keySelector)
    {
        if (!oldItems.TryGetNonEnumeratedCount(out var oldItemsCount))
            oldItemsCount = 0;

        if (!newItems.TryGetNonEnumeratedCount(out var newItemsCount))
            newItemsCount = 0;

        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: oldItemsCount + newItemsCount);

        foreach(var item in oldItems)
            changes.Add(KeyedChange.Removal(
                key:    keySelector.Invoke(item),
                item:   item));

        foreach(var item in newItems)
            changes.Add(KeyedChange.Addition(
                key:    keySelector.Invoke(item),
                item:   item));

        return new()
        {
            Changes = changes.MoveToOrCreateImmutable(),
            Type    = ChangeSetType.Reset
        };
    }

    /// <inheritdoc cref="Reset{TKey, TItem}(IEnumerable{TItem}, IEnumerable{TItem}, Func{TItem, TKey})"/>
    public static KeyedChangeSet<TKey, TItem> Reset<TKey, TItem>(
        ReadOnlySpan<TItem> oldItems,
        ReadOnlySpan<TItem> newItems,
        Func<TItem, TKey>   keySelector)
    {
        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: oldItems.Length + newItems.Length);

        foreach(var item in oldItems)
            changes.Add(KeyedChange.Removal(
                key:    keySelector.Invoke(item),
                item:   item));

        foreach(var item in newItems)
            changes.Add(KeyedChange.Addition(
                key:    keySelector.Invoke(item),
                item:   item));

        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Reset
        };
    }
}

/// <summary>
/// Describes a set of single-item changes made (or to be made) to a keyed collection
/// </summary>
/// <typeparam name="TKey">The type of the items' keys.</typeparam>
/// <typeparam name="TItem">The type of the items in the collection.</typeparam>
public readonly record struct KeyedChangeSet<TKey, TItem>
{
    /// <summary>
    /// The set of single-item changes to the collection.
    /// Changes must be applied to the collection in the order they appear here.
    /// </summary>
    public required ImmutableArray<KeyedChange<TKey, TItem>> Changes { get; init; }

    /// <summary>
    /// The type of operation that created this changeset, or that this changeset represents.
    /// </summary>
    public required ChangeSetType Type { get; init; }
}
