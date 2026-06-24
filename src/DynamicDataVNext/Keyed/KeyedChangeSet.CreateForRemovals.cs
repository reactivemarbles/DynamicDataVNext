using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DynamicDataVNext;

public static partial class KeyedChangeSet
{
    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForRemovals(IEnumerable{TItem}, Func{TItem, TKey})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the removed items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForRemovals<TKey, TItem>(
            IEnumerable<TItem>  items,
            Func<TItem, TKey>   keySelector)
        => KeyedChangeSet<TKey, TItem>.CreateForRemovals(
            items:          items,
            keySelector:    keySelector);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForRemovals(ReadOnlySpan{TItem}, Func{TItem, TKey})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the removed items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForRemovals<TKey, TItem>(
            ReadOnlySpan<TItem> items,
            Func<TItem, TKey>   keySelector)
        => KeyedChangeSet<TKey, TItem>.CreateForRemovals(
            items:          items,
            keySelector:    keySelector);
    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForRemovals(IEnumerable{KeyedItem{TKey,TItem}})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the removed items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForRemovals<TKey, TItem>(IEnumerable<KeyedItem<TKey, TItem>> removals)
        => KeyedChangeSet<TKey, TItem>.CreateForRemovals(removals);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForRemovals(IEnumerable{KeyValuePair{TKey,TItem}})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the removed items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForRemovals<TKey, TItem>(IEnumerable<KeyValuePair<TKey, TItem>> removals)
        => KeyedChangeSet<TKey, TItem>.CreateForRemovals(removals);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForRemovals(ReadOnlySpan{KeyedItem{TKey,TItem}})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the removed items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForRemovals<TKey, TItem>(ReadOnlySpan<KeyedItem<TKey, TItem>> removals)
        => KeyedChangeSet<TKey, TItem>.CreateForRemovals(removals);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForRemovals(ReadOnlySpan{KeyValuePair{TKey,TItem}})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the removed items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForRemovals<TKey, TItem>(ReadOnlySpan<KeyValuePair<TKey, TItem>> removals)
        => KeyedChangeSet<TKey, TItem>.CreateForRemovals(removals);
}

public readonly partial record struct KeyedChangeSet<TKey, TItem>
{
    /// <inheritdoc cref="CreateForRemovals(ReadOnlySpan{TItem}, Func{TItem, TKey})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/> and <paramref name="keySelector"/>.</exception>
    public static KeyedChangeSet<TKey, TItem> CreateForRemovals(
        IEnumerable<TItem>  items,
        Func<TItem, TKey>   keySelector)
    {
        ArgumentNullException.ThrowIfNull(items);

        var isItemCountValid = items.TryGetNonEnumeratedCount(out var itemCount);
        if (isItemCountValid && (itemCount is 0))
            return Empty;
        
        var changes = isItemCountValid
            ? ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: itemCount)
            : ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>();

        foreach(var item in items)
            changes.Add(KeyedChange.CreateRemoval(
                key:    keySelector.Invoke(item),
                item:   item));
            
        if (changes.Count is 0)
            return Empty;

        return new()
        {
            Changes = changes.MoveToOrCreateImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the <see cref="KeyedChangeType.Removal"/> of a range of items.
    /// </summary>
    /// <param name="items">The removed items.</param>
    /// <param name="keySelector">A selector for determining the key for each removed item.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="keySelector"/>.</exception>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the removal of the given items, or <see cref="Empty"/> if no items were given.</returns>
    public static KeyedChangeSet<TKey, TItem> CreateForRemovals(
        ReadOnlySpan<TItem> items,
        Func<TItem, TKey>   keySelector)
    {
        if (items.Length is 0)
            return Empty;
        
        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: items.Length);

        foreach(var item in items)
            changes.Add(KeyedChange.CreateRemoval(
                key:    keySelector.Invoke(item),
                item:   item));
            
        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <inheritdoc cref="CreateForRemovals(ReadOnlySpan{KeyedItem{TKey,TItem}})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="removals"/>.</exception>
    public static KeyedChangeSet<TKey, TItem> CreateForRemovals(IEnumerable<KeyedItem<TKey, TItem>> removals)
    {
        ArgumentNullException.ThrowIfNull(removals);

        var isRemovalCountValid = removals.TryGetNonEnumeratedCount(out var removalCount);
        if (isRemovalCountValid && (removalCount is 0))
            return Empty;
        
        var changes = isRemovalCountValid
            ? ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: removalCount)
            : ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>();

        foreach(var removal in removals)
            changes.Add(KeyedChange.CreateRemoval(removal));
            
        if (changes.Count is 0)
            return Empty;

        return new()
        {
            Changes = changes.MoveToOrCreateImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <inheritdoc cref="CreateForRemovals(ReadOnlySpan{KeyValuePair{TKey,TItem}})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="removals"/>.</exception>
    public static KeyedChangeSet<TKey, TItem> CreateForRemovals(IEnumerable<KeyValuePair<TKey, TItem>> removals)
    {
        ArgumentNullException.ThrowIfNull(removals);

        var isRemovalCountValid = removals.TryGetNonEnumeratedCount(out var removalCount);
        if (isRemovalCountValid && (removalCount is 0))
            return Empty;
        
        var changes = isRemovalCountValid
            ? ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: removalCount)
            : ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>();

        foreach(var removal in removals)
            changes.Add(KeyedChange.CreateRemoval(removal));
            
        if (changes.Count is 0)
            return Empty;

        return new()
        {
            Changes = changes.MoveToOrCreateImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the <see cref="KeyedChangeType.Removal"/> of a range of items.
    /// </summary>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the removed items.</typeparam>
    /// <param name="removals">The removed items, and their keys.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the removal of the given items, or <see cref="Empty"/> if no items were given.</returns>
    public static KeyedChangeSet<TKey, TItem> CreateForRemovals(ReadOnlySpan<KeyedItem<TKey, TItem>> removals)
    {
        if (removals.Length is 0)
            return Empty;
        
        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: removals.Length);

        foreach(var removal in removals)
            changes.Add(KeyedChange.CreateRemoval(removal));
            
        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <inheritdoc cref="CreateForRemovals(ReadOnlySpan{KeyedItem{TKey,TItem}})"/>
    public static KeyedChangeSet<TKey, TItem> CreateForRemovals(ReadOnlySpan<KeyValuePair<TKey, TItem>> removals)
    {
        if (removals.Length is 0)
            return Empty;
        
        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: removals.Length);

        foreach(var removal in removals)
            changes.Add(KeyedChange.CreateRemoval(removal));
            
        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Update
        };
    }
}
