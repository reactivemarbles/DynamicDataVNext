using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DynamicDataVNext;

/// <summary>
/// Contains convenience methods for creating <see cref="KeyedChangeSet{TKey, TItem}"/> values.
/// </summary>
public static class KeyedChangeSet
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
    /// <typeparam name="TAdditions">The type of collection or sequence containing the items being added. Allows JIT optimizations, depending on the type given.</typeparam>
    /// <param name="additions">The items being added, and their keys.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the addition of the given items.</returns>
    public static KeyedChangeSet<TKey, TItem> Addition<TKey, TItem, TAdditions>(TAdditions additions)
        where TAdditions: IEnumerable<KeyValuePair<TKey, TItem>>
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
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the clearing of a keyed collection.
    /// </summary>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the items being removed.</typeparam>
    /// <typeparam name="TRemovals">The type of collection or sequence containing the items being removed. Allows JIT optimizations, depending on the type given.</typeparam>
    /// <param name="removals">The items being removed, and their keys.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the clearing of the collection.</returns>
    public static KeyedChangeSet<TKey, TItem> Clear<TKey, TItem, TRemovals>(TRemovals removals)
        where TRemovals : IEnumerable<KeyValuePair<TKey, TItem>>
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
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the removal of a range of items.
    /// </summary>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the items being removed.</typeparam>
    /// <typeparam name="TRemovals">The type of collection or sequence containing the items being removed. Allows JIT optimizations, depending on the type given.</typeparam>
    /// <param name="removals">The items being removed, and their keys.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the removal of the given items.</returns>
    public static KeyedChangeSet<TKey, TItem> Removal<TKey, TItem, TRemovals>(TRemovals removals)
        where TRemovals : IEnumerable<KeyValuePair<TKey, TItem>>
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
    /// <typeparam name="TRemovals">The type of collection or sequence containing the items being removed. Allows JIT optimizations, depending on the type given.</typeparam>
    /// <typeparam name="TAdditions">The type of collection or sequence containing the items being added. Allows JIT optimizations, depending on the type given.</typeparam>
    /// <param name="removals">The items being removed, and their keys.</param>
    /// <param name="additions">The items being added, and their keys.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the given reset operation.</returns>
    public static KeyedChangeSet<TKey, TItem> Reset<TKey, TItem, TRemovals, TAdditions>(
            TRemovals   removals,
            TAdditions  additions)
        where TRemovals : IEnumerable<KeyValuePair<TKey, TItem>>
        where TAdditions : IEnumerable<KeyValuePair<TKey, TItem>>
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
