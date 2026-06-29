using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DynamicDataVNext;

public static partial class KeyedChangeSet
{
    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForAdditions(IEnumerable{KeyedItem{TKey,TItem}})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the added items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForAdditions<TKey, TItem>(IEnumerable<KeyedItem<TKey, TItem>> additions)
        => KeyedChangeSet<TKey, TItem>.CreateForAdditions(additions);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForAdditions(IEnumerable{KeyValuePair{TKey,TItem}})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the added items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForAdditions<TKey, TItem>(IEnumerable<KeyValuePair<TKey, TItem>> additions)
        => KeyedChangeSet<TKey, TItem>.CreateForAdditions(additions);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForAdditions(ReadOnlySpan{KeyedItem{TKey,TItem}})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the added items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForAdditions<TKey, TItem>(ReadOnlySpan<KeyedItem<TKey, TItem>> additions)
        => KeyedChangeSet<TKey, TItem>.CreateForAdditions(additions);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForAdditions(ReadOnlySpan{KeyValuePair{TKey,TItem}})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the added items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForAdditions<TKey, TItem>(ReadOnlySpan<KeyValuePair<TKey, TItem>> additions)
        => KeyedChangeSet<TKey, TItem>.CreateForAdditions(additions);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForAdditions(IEnumerable{TItem}, Func{TItem, TKey})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the added items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForAdditions<TKey, TItem>(
            IEnumerable<TItem>  items,
            Func<TItem, TKey>   keySelector)
        => KeyedChangeSet<TKey, TItem>.CreateForAdditions(
            items:          items,
            keySelector:    keySelector);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForAdditions(ReadOnlySpan{TItem}, Func{TItem, TKey})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the added items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForAdditions<TKey, TItem>(
            ReadOnlySpan<TItem> items,
            Func<TItem, TKey>   keySelector)
        => KeyedChangeSet<TKey, TItem>.CreateForAdditions(
            items:          items,
            keySelector:    keySelector);
}

public readonly partial record struct KeyedChangeSet<TKey, TItem>
{
    /// <inheritdoc cref="CreateForAdditions(ReadOnlySpan{KeyedItem{TKey,TItem}})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="additions"/>.</exception>
    public static KeyedChangeSet<TKey, TItem> CreateForAdditions(IEnumerable<KeyedItem<TKey, TItem>> additions)
    {
        ArgumentNullException.ThrowIfNull(additions);

        var isAdditionCountValid = additions.TryGetNonEnumeratedCount(out var additionCount);
        if (isAdditionCountValid && (additionCount is 0))
            return Empty;
        
        var changes = isAdditionCountValid
            ? ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: additionCount)
            : ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>();

        foreach(var addition in additions)
            changes.Add(KeyedChange.CreateAddition(addition));
            
        if (changes.Count is 0)
            return Empty;

        return new()
        {
            Changes = changes.MoveToOrCreateImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <inheritdoc cref="CreateForAdditions(ReadOnlySpan{KeyValuePair{TKey,TItem}})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="additions"/>.</exception>
    public static KeyedChangeSet<TKey, TItem> CreateForAdditions(IEnumerable<KeyValuePair<TKey, TItem>> additions)
    {
        ArgumentNullException.ThrowIfNull(additions);

        var isAdditionCountValid = additions.TryGetNonEnumeratedCount(out var additionCount);
        if (isAdditionCountValid && (additionCount is 0))
            return Empty;
        
        var changes = isAdditionCountValid
            ? ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: additionCount)
            : ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>();

        foreach(var addition in additions)
            changes.Add(KeyedChange.CreateAddition(addition));
            
        if (changes.Count is 0)
            return Empty;

        return new()
        {
            Changes = changes.MoveToOrCreateImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the <see cref="KeyedChangeType.Addition"/> of a range of items.
    /// </summary>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the added items.</typeparam>
    /// <param name="additions">The added items, and their keys.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the addition of the given items, or <see cref="Empty"/> if no items were given.</returns>
    public static KeyedChangeSet<TKey, TItem> CreateForAdditions(ReadOnlySpan<KeyedItem<TKey, TItem>> additions)
    {
        if (additions.Length is 0)
            return Empty;
        
        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: additions.Length);

        foreach(var addition in additions)
            changes.Add(KeyedChange.CreateAddition(addition));
            
        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <inheritdoc cref="CreateForAdditions(ReadOnlySpan{KeyedItem{TKey,TItem}})"/>
    public static KeyedChangeSet<TKey, TItem> CreateForAdditions(ReadOnlySpan<KeyValuePair<TKey, TItem>> additions)
    {
        if (additions.Length is 0)
            return Empty;
        
        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: additions.Length);

        foreach(var addition in additions)
            changes.Add(KeyedChange.CreateAddition(addition));
            
        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <inheritdoc cref="CreateForAdditions(ReadOnlySpan{TItem}, Func{TItem, TKey})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/> and <paramref name="keySelector"/>.</exception>
    public static KeyedChangeSet<TKey, TItem> CreateForAdditions(
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
            changes.Add(KeyedChange.CreateAddition(
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
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing the <see cref="KeyedChangeType.Addition"/> of a range of items.
    /// </summary>
    /// <param name="items">The added items.</param>
    /// <param name="keySelector">A selector for determining the key for each added item.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the addition of the given items.</returns>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="keySelector"/>.</exception>
    public static KeyedChangeSet<TKey, TItem> CreateForAdditions(
        ReadOnlySpan<TItem> items,
        Func<TItem, TKey>   keySelector)
    {
        ArgumentNullException.ThrowIfNull(keySelector);
    
        if (items.Length is 0)
            return Empty;
        
        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: items.Length);

        foreach(var item in items)
            changes.Add(KeyedChange.CreateAddition(
                key:    keySelector.Invoke(item),
                item:   item));
            
        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Update
        };
    }
}
