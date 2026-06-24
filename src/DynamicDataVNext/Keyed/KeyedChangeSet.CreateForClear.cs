using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DynamicDataVNext;

public static partial class KeyedChangeSet
{
    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForClear(IEnumerable{TItem}, Func{TItem, TKey})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the removed items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForClear<TKey, TItem>(
            IEnumerable<TItem>  items,
            Func<TItem, TKey>   keySelector)
        => KeyedChangeSet<TKey, TItem>.CreateForClear(
            items:          items,
            keySelector:    keySelector);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForClear(ReadOnlySpan{TItem}, Func{TItem, TKey})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the removed items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForClear<TKey, TItem>(
            ReadOnlySpan<TItem> items,
            Func<TItem, TKey>   keySelector)
        => KeyedChangeSet<TKey, TItem>.CreateForClear(
            items:          items,
            keySelector:    keySelector);
    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForClear(IEnumerable{KeyedItem{TKey,TItem}})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the removed items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForClear<TKey, TItem>(IEnumerable<KeyedItem<TKey, TItem>> removals)
        => KeyedChangeSet<TKey, TItem>.CreateForClear(removals);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForClear(IEnumerable{KeyValuePair{TKey,TItem}})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the removed items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForClear<TKey, TItem>(IEnumerable<KeyValuePair<TKey, TItem>> removals)
        => KeyedChangeSet<TKey, TItem>.CreateForClear(removals);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForClear(ReadOnlySpan{KeyedItem{TKey,TItem}})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the removed items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForClear<TKey, TItem>(ReadOnlySpan<KeyedItem<TKey, TItem>> removals)
        => KeyedChangeSet<TKey, TItem>.CreateForClear(removals);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForClear(ReadOnlySpan{KeyValuePair{TKey,TItem}})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the removed items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForClear<TKey, TItem>(ReadOnlySpan<KeyValuePair<TKey, TItem>> removals)
        => KeyedChangeSet<TKey, TItem>.CreateForClear(removals);
}

public readonly partial record struct KeyedChangeSet<TKey, TItem>
{
    /// <inheritdoc cref="CreateForClear(ReadOnlySpan{TItem}, Func{TItem, TKey})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/> and <paramref name="keySelector"/>.</exception>
    public static KeyedChangeSet<TKey, TItem> CreateForClear(
        IEnumerable<TItem>  items,
        Func<TItem, TKey>   keySelector)
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(keySelector);

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
            Type    = ChangeSetType.Clear
        };
    }

    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing a <see cref="ChangeSetType.Clear"/> operation.
    /// </summary>
    /// <param name="items">The added items.</param>
    /// <param name="keySelector">A selector for determining the key for each added item.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="keySelector"/>.</exception>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the addition of the given items, or <see cref="Empty"/> if no items were given.</returns>
    public static KeyedChangeSet<TKey, TItem> CreateForClear(
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
            Type    = ChangeSetType.Clear
        };
    }

    /// <inheritdoc cref="CreateForClear(ReadOnlySpan{KeyedItem{TKey,TItem}})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="removals"/>.</exception>
    public static KeyedChangeSet<TKey, TItem> CreateForClear(IEnumerable<KeyedItem<TKey, TItem>> removals)
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
            Type    = ChangeSetType.Clear
        };
    }

    /// <inheritdoc cref="CreateForClear(ReadOnlySpan{KeyValuePair{TKey,TItem}})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="removals"/>.</exception>
    public static KeyedChangeSet<TKey, TItem> CreateForClear(IEnumerable<KeyValuePair<TKey, TItem>> removals)
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
            Type    = ChangeSetType.Clear
        };
    }

    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing a <see cref="ChangeSetType.Clear"/> operation.
    /// </summary>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the added items.</typeparam>
    /// <param name="removals">The removed items, and their keys.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the addition of the given items, or <see cref="Empty"/> if no items were given.</returns>
    public static KeyedChangeSet<TKey, TItem> CreateForClear(ReadOnlySpan<KeyedItem<TKey, TItem>> removals)
    {
        if (removals.Length is 0)
            return Empty;
        
        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: removals.Length);

        foreach(var removal in removals)
            changes.Add(KeyedChange.CreateRemoval(removal));
            
        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Clear
        };
    }

    /// <inheritdoc cref="CreateForClear(ReadOnlySpan{KeyedItem{TKey,TItem}})"/>
    public static KeyedChangeSet<TKey, TItem> CreateForClear(ReadOnlySpan<KeyValuePair<TKey, TItem>> removals)
    {
        if (removals.Length is 0)
            return Empty;
        
        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: removals.Length);

        foreach(var removal in removals)
            changes.Add(KeyedChange.CreateAddition(removal));
            
        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Clear
        };
    }
}
