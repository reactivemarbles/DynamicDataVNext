using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DynamicDataVNext;

public static partial class KeyedChangeSet
{
    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForReset(IEnumerable{TItem}, Func{TItem, TKey})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the involved items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForReset<TKey, TItem>(
            IEnumerable<TItem>  addedItems,
            Func<TItem, TKey>   keySelector)
        => KeyedChangeSet<TKey, TItem>.CreateForReset(
            addedItems:     addedItems,
            keySelector:    keySelector);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForReset(ReadOnlySpan{TItem}, Func{TItem, TKey})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the involved items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForReset<TKey, TItem>(
            ReadOnlySpan<TItem> addedItems,
            Func<TItem, TKey>   keySelector)
        => KeyedChangeSet<TKey, TItem>.CreateForReset(
            addedItems:     addedItems,
            keySelector:    keySelector);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForReset(IEnumerable{KeyedItem{TKey, TItem}})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the involved items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForReset<TKey, TItem>(IEnumerable<KeyedItem<TKey, TItem>> additions)
        => KeyedChangeSet<TKey, TItem>.CreateForReset(additions);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForReset(IEnumerable{KeyValuePair{TKey, TItem}})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the involved items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForReset<TKey, TItem>(IEnumerable<KeyValuePair<TKey, TItem>> additions)
        => KeyedChangeSet<TKey, TItem>.CreateForReset(additions);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForReset(ReadOnlySpan{KeyedItem{TKey, TItem}})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the involved items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForReset<TKey, TItem>(ReadOnlySpan<KeyedItem<TKey, TItem>> additions)
        => KeyedChangeSet<TKey, TItem>.CreateForReset(additions);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForReset(ReadOnlySpan{KeyValuePair{TKey, TItem}})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the involved items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForReset<TKey, TItem>(ReadOnlySpan<KeyValuePair<TKey, TItem>> additions)
        => KeyedChangeSet<TKey, TItem>.CreateForReset(additions);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForReset(IEnumerable{KeyedItem{TKey, TItem}}, IEnumerable{KeyedItem{TKey, TItem}})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the involved items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForReset<TKey, TItem>(
            IEnumerable<KeyedItem<TKey, TItem>> removals,
            IEnumerable<KeyedItem<TKey, TItem>> additions)
        => KeyedChangeSet<TKey, TItem>.CreateForReset(
            removals:   removals,
            additions:  additions);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForReset(IEnumerable{KeyValuePair{TKey, TItem}}, IEnumerable{KeyValuePair{TKey, TItem}})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the involved items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForReset<TKey, TItem>(
            IEnumerable<KeyValuePair<TKey, TItem>> removals,
            IEnumerable<KeyValuePair<TKey, TItem>> additions)
        => KeyedChangeSet<TKey, TItem>.CreateForReset(
            removals:   removals,
            additions:  additions);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForReset(ReadOnlySpan{KeyedItem{TKey, TItem}}, ReadOnlySpan{KeyedItem{TKey, TItem}})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the involved items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForReset<TKey, TItem>(
            ReadOnlySpan<KeyedItem<TKey, TItem>> removals,
            ReadOnlySpan<KeyedItem<TKey, TItem>> additions)
        => KeyedChangeSet<TKey, TItem>.CreateForReset(
            removals:   removals,
            additions:  additions);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForReset(ReadOnlySpan{KeyValuePair{TKey, TItem}}, ReadOnlySpan{KeyValuePair{TKey, TItem}})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the involved items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForReset<TKey, TItem>(
            ReadOnlySpan<KeyValuePair<TKey, TItem>> removals,
            ReadOnlySpan<KeyValuePair<TKey, TItem>> additions)
        => KeyedChangeSet<TKey, TItem>.CreateForReset(
            removals:   removals,
            additions:  additions);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForReset(IEnumerable{TItem}, IEnumerable{TItem}, Func{TItem, TKey})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the involved items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForReset<TKey, TItem>(
            IEnumerable<TItem>  removedItems,
            IEnumerable<TItem>  addedItems,
            Func<TItem, TKey>   keySelector)
        => KeyedChangeSet<TKey, TItem>.CreateForReset(
            removedItems:   removedItems,
            addedItems:     addedItems,
            keySelector:    keySelector);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForReset(ReadOnlySpan{TItem}, ReadOnlySpan{TItem}, Func{TItem, TKey})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the involved items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForReset<TKey, TItem>(
            ReadOnlySpan<TItem> removedItems,
            ReadOnlySpan<TItem> addedItems,
            Func<TItem, TKey>   keySelector)
        => KeyedChangeSet<TKey, TItem>.CreateForReset(
            removedItems:   removedItems,
            addedItems:     addedItems,
            keySelector:    keySelector);
}

public readonly partial record struct KeyedChangeSet<TKey, TItem>
{
    /// <inheritdoc cref="CreateForReset(ReadOnlySpan{TItem}, Func{TItem, TKey})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="addedItems"/> and <paramref name="keySelector"/>.</exception>
    public static KeyedChangeSet<TKey, TItem> CreateForReset(
        IEnumerable<TItem>  addedItems,
        Func<TItem, TKey>   keySelector)
    {
        ArgumentNullException.ThrowIfNull(addedItems);
    
        var isAddedItemCountValid = addedItems.TryGetNonEnumeratedCount(out var addedItemCount);

        if (isAddedItemCountValid && (addedItemCount is 0))
            return Empty;
    
        var changes = isAddedItemCountValid
            ? ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: addedItemCount)
            : ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>();

        foreach(var item in addedItems)
            changes.Add(KeyedChange.CreateAddition(
                key:    keySelector.Invoke(item),
                item:   item));

        return new()
        {
            Changes             = changes.MoveToImmutable(),
            Type                = ChangeSetType.Reset,
            FirstAdditionIndex  = 0
        };
    }

    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing a <see cref="ChangeSetType.Reset"/> operation upon an empty collection.
    /// </summary>
    /// <param name="addedItems">The items added by the reset.</param>
    /// <param name="keySelector">A selector for determining the key for each item.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="keySelector"/>.</exception>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the given reset operation, or <see cref="Empty"/> if no items were given.</returns>
    public static KeyedChangeSet<TKey, TItem> CreateForReset(
        ReadOnlySpan<TItem> addedItems,
        Func<TItem, TKey>   keySelector)
    {
        if (addedItems.Length is 0)
            return Empty;
    
        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: addedItems.Length);

        foreach(var item in addedItems)
            changes.Add(KeyedChange.CreateAddition(
                key:    keySelector.Invoke(item),
                item:   item));

        return new()
        {
            Changes             = changes.MoveToImmutable(),
            Type                = ChangeSetType.Reset,
            FirstAdditionIndex  = 0
        };
    }

    /// <inheritdoc cref="CreateForReset(ReadOnlySpan{KeyedItem{TKey, TItem}})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="additions"/>.</exception>
    public static KeyedChangeSet<TKey, TItem> CreateForReset(IEnumerable<KeyedItem<TKey, TItem>> additions)
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
            Changes             = changes.MoveToOrCreateImmutable(),
            Type                = ChangeSetType.Reset,
            FirstAdditionIndex  = 0 
        };
    }

    /// <inheritdoc cref="CreateForReset(ReadOnlySpan{KeyValuePair{TKey, TItem}})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="additions"/>.</exception>
    public static KeyedChangeSet<TKey, TItem> CreateForReset(IEnumerable<KeyValuePair<TKey, TItem>> additions)
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
            Changes             = changes.MoveToOrCreateImmutable(),
            Type                = ChangeSetType.Reset,
            FirstAdditionIndex  = 0 
        };
    }

    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing a <see cref="ChangeSetType.Reset"/> operation upon an empty collection.
    /// </summary>
    /// <param name="additions">The items added by the reset, and their keys.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the given reset operation, or <see cref="Empty"/> if no items were given.</returns>
    public static KeyedChangeSet<TKey, TItem> CreateForReset(ReadOnlySpan<KeyedItem<TKey, TItem>> additions)
    {
        if (additions.Length is 0)
            return Empty;
    
        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: additions.Length);

        foreach(var addition in additions)
            changes.Add(KeyedChange.CreateAddition(addition));

        return new()
        {
            Changes             = changes.MoveToImmutable(),
            Type                = ChangeSetType.Reset,
            FirstAdditionIndex  = 0 
        };
    }

    /// <inheritdoc cref="CreateForReset(ReadOnlySpan{KeyedItem{TKey, TItem}})"/>
    public static KeyedChangeSet<TKey, TItem> CreateForReset(ReadOnlySpan<KeyValuePair<TKey, TItem>> additions)
    {
        if (additions.Length is 0)
            return Empty;
    
        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: additions.Length);

        foreach(var addition in additions)
            changes.Add(KeyedChange.CreateAddition(addition));

        return new()
        {
            Changes             = changes.MoveToImmutable(),
            Type                = ChangeSetType.Reset,
            FirstAdditionIndex  = 0 
        };
    }

    /// <inheritdoc cref="CreateForReset(ReadOnlySpan{KeyedItem{TKey, TItem}}, ReadOnlySpan{KeyedItem{TKey, TItem}})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="removals"/> and <paramref name="additions"/>.</exception>
    public static KeyedChangeSet<TKey, TItem> CreateForReset(
        IEnumerable<KeyedItem<TKey, TItem>> removals,
        IEnumerable<KeyedItem<TKey, TItem>> additions)
    {
        ArgumentNullException.ThrowIfNull(removals);
        ArgumentNullException.ThrowIfNull(additions);
    
        var isRemovalCountValid     = removals  .TryGetNonEnumeratedCount(out var removalCount);
        var isAdditionCountValid    = additions .TryGetNonEnumeratedCount(out var additionCount);
        
        if (        isRemovalCountValid     && (removalCount    is 0)
                &&  isAdditionCountValid    && (additionCount   is 0))
            return Empty;
    
        var changes = (isRemovalCountValid || isAdditionCountValid)
            ? ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: removalCount + additionCount)
            : ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>();

        foreach(var removal in removals)
            changes.Add(KeyedChange.CreateRemoval(removal));

        removalCount = changes.Count;

        foreach(var addition in additions)
            changes.Add(KeyedChange.CreateAddition(addition));

        if (changes.Count is 0)
            return Empty;

        return new()
        {
            Changes             = changes.MoveToOrCreateImmutable(),
            Type                = (changes.Count == removalCount)
                ? ChangeSetType.Clear
                : ChangeSetType.Reset,
            FirstAdditionIndex  = removalCount 
        };
    }

    /// <inheritdoc cref="CreateForReset(IEnumerable{KeyValuePair{TKey, TItem}}, IEnumerable{KeyValuePair{TKey, TItem}})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="removals"/> and <paramref name="additions"/>.</exception>
    public static KeyedChangeSet<TKey, TItem> CreateForReset(
        IEnumerable<KeyValuePair<TKey, TItem>> removals,
        IEnumerable<KeyValuePair<TKey, TItem>> additions)
    {
        ArgumentNullException.ThrowIfNull(removals);
        ArgumentNullException.ThrowIfNull(additions);
    
        var isRemovalCountValid     = removals  .TryGetNonEnumeratedCount(out var removalCount);
        var isAdditionCountValid    = additions .TryGetNonEnumeratedCount(out var additionCount);
        
        if (        isRemovalCountValid     && (removalCount    is 0)
                                            &&  isAdditionCountValid    && (additionCount   is 0))
            return Empty;
    
        var changes = (isRemovalCountValid || isAdditionCountValid)
            ? ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: removalCount + additionCount)
            : ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>();

        foreach(var removal in removals)
            changes.Add(KeyedChange.CreateRemoval(removal));

        removalCount = changes.Count;

        foreach(var addition in additions)
            changes.Add(KeyedChange.CreateAddition(addition));

        if (changes.Count is 0)
            return Empty;

        return new()
        {
            Changes             = changes.MoveToOrCreateImmutable(),
            Type                = (changes.Count == removalCount)
                ? ChangeSetType.Clear
                : ChangeSetType.Reset,
            FirstAdditionIndex  = removalCount 
        };
    }

    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing a <see cref="ChangeSetType.Reset"/> operation.
    /// </summary>
    /// <param name="removals">The items removed by the reset, and their keys.</param>
    /// <param name="additions">The items added by the reset, and their keys.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the given reset operation, or a <see cref="ChangeSetType.Clear"/> operation if <paramref name="additions"/> is empty, or <see cref="Empty"/> if no items were given.</returns>
    public static KeyedChangeSet<TKey, TItem> CreateForReset(
        ReadOnlySpan<KeyedItem<TKey, TItem>> removals,
        ReadOnlySpan<KeyedItem<TKey, TItem>> additions)
    {
        if ((removals.Length is 0) && (additions.Length is 0))
            return Empty;
    
        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: removals.Length + additions.Length);

        foreach(var removal in removals)
            changes.Add(KeyedChange.CreateRemoval(removal));

        foreach(var addition in additions)
            changes.Add(KeyedChange.CreateAddition(addition));

        return new()
        {
            Changes             = changes.MoveToImmutable(),
            Type                = (additions.Length is 0)
                ? ChangeSetType.Clear
                : ChangeSetType.Reset,
            FirstAdditionIndex  = removals.Length 
        };
    }

    /// <inheritdoc cref="CreateForReset(ReadOnlySpan{KeyedItem{TKey, TItem}}, ReadOnlySpan{KeyedItem{TKey, TItem}})"/>
    public static KeyedChangeSet<TKey, TItem> CreateForReset(
        ReadOnlySpan<KeyValuePair<TKey, TItem>> removals,
        ReadOnlySpan<KeyValuePair<TKey, TItem>> additions)
    {
        if ((removals.Length is 0) && (additions.Length is 0))
            return Empty;
    
        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: removals.Length + additions.Length);

        foreach(var removal in removals)
            changes.Add(KeyedChange.CreateRemoval(removal));

        foreach(var addition in additions)
            changes.Add(KeyedChange.CreateAddition(addition));

        return new()
        {
            Changes             = changes.MoveToImmutable(),
            Type                = (additions.Length is 0)
                ? ChangeSetType.Clear
                : ChangeSetType.Reset,
            FirstAdditionIndex  = removals.Length 
        };
    }

    /// <inheritdoc cref="CreateForReset(ReadOnlySpan{TItem}, ReadOnlySpan{TItem}, Func{TItem, TKey})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="removedItems"/>, <paramref name="addedItems"/>, and <paramref name="keySelector"/>.</exception>
    public static KeyedChangeSet<TKey, TItem> CreateForReset(
        IEnumerable<TItem>  removedItems,
        IEnumerable<TItem>  addedItems,
        Func<TItem, TKey>   keySelector)
    {
        ArgumentNullException.ThrowIfNull(removedItems);
        ArgumentNullException.ThrowIfNull(addedItems);
    
        var isRemovedItemCountValid = removedItems  .TryGetNonEnumeratedCount(out var removedItemCount);
        var isAddedItemCountValid   = addedItems    .TryGetNonEnumeratedCount(out var addedItemCount);

        if (        isRemovedItemCountValid &&  (removedItemCount is 0)
                                            &&  isAddedItemCountValid   &&  (addedItemCount is 0))
            return Empty;
    
        var changes = (isRemovedItemCountValid || isAddedItemCountValid)
            ? ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: removedItemCount + addedItemCount)
            : ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>();

        foreach(var item in removedItems)
            changes.Add(KeyedChange.CreateRemoval(
                key:    keySelector.Invoke(item),
                item:   item));

        removedItemCount = changes.Count;

        foreach(var item in addedItems)
            changes.Add(KeyedChange.CreateAddition(
                key:    keySelector.Invoke(item),
                item:   item));

        return new()
        {
            Changes             = changes.MoveToImmutable(),
            Type                = (changes.Count == removedItemCount)
                ? ChangeSetType.Clear
                : ChangeSetType.Reset,
            FirstAdditionIndex  = removedItemCount
        };
    }

    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing a <see cref="ChangeSetType.Reset"/> operation.
    /// </summary>
    /// <param name="removedItems">The items removed by the reset.</param>
    /// <param name="addedItems">The items added by the reset.</param>
    /// <param name="keySelector">A selector for determining the key for each item.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="keySelector"/>.</exception>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the given reset operation, or a <see cref="ChangeSetType.Clear"/> operation if <paramref name="addedItems"/> is empty, or <see cref="Empty"/> if no items were given.</returns>
    public static KeyedChangeSet<TKey, TItem> CreateForReset(
        ReadOnlySpan<TItem> removedItems,
        ReadOnlySpan<TItem> addedItems,
        Func<TItem, TKey>   keySelector)
    {
        if ((removedItems.Length is 0) && (addedItems.Length is 0))
            return Empty;
    
        var changes = ImmutableArray.CreateBuilder<KeyedChange<TKey, TItem>>(initialCapacity: removedItems.Length + addedItems.Length);

        foreach(var item in removedItems)
            changes.Add(KeyedChange.CreateRemoval(
                key:    keySelector.Invoke(item),
                item:   item));

        foreach(var item in addedItems)
            changes.Add(KeyedChange.CreateAddition(
                key:    keySelector.Invoke(item),
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
