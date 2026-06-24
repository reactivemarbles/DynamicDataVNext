using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DynamicDataVNext;

public static partial class KeyedChangeSet
{
    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForUpdate(KeyedChange{TKey, TItem})"/>
    /// <typeparam name="TKey">The type of the item's or items' key.</typeparam>
    /// <typeparam name="TItem">The type of the changed item or items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForUpdate<TKey, TItem>(KeyedChange<TKey, TItem> change)
        => KeyedChangeSet<TKey, TItem>.CreateForUpdate(change);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForUpdate(System.Collections.Generic.IEnumerable{KeyedChange{TKey, TItem}})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the changed items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForUpdate<TKey, TItem>(IEnumerable<KeyedChange<TKey, TItem>> changes)
        => KeyedChangeSet<TKey, TItem>.CreateForUpdate(changes);

    /// <inheritdoc cref="KeyedChangeSet{TKey, TItem}.CreateForUpdate(System.ReadOnlySpan{KeyedChange{TKey, TItem}})"/>
    /// <typeparam name="TKey">The type of the items' keys.</typeparam>
    /// <typeparam name="TItem">The type of the changed items.</typeparam>
    public static KeyedChangeSet<TKey, TItem> CreateForUpdate<TKey, TItem>(ReadOnlySpan<KeyedChange<TKey, TItem>> changes)
        => KeyedChangeSet<TKey, TItem>.CreateForUpdate(changes);
}

public readonly partial record struct KeyedChangeSet<TKey, TItem>
{
    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing a single-item <see cref="ChangeSetType.Update"/> operation.
    /// </summary>
    /// <param name="change">The change being represented.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the given single-item change, as an <see cref="ChangeSetType.Update"/> operation.</returns>
    public static KeyedChangeSet<TKey, TItem> CreateForUpdate(KeyedChange<TKey, TItem> change)
        => new()
        {
            Changes = ImmutableArray.Create(change),
            Type    = ChangeSetType.Update
        };

    /// <inheritdoc cref="CreateForUpdate(System.ReadOnlySpan{KeyedChange{TKey, TItem}})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="changes"/>.</exception>
    public static KeyedChangeSet<TKey, TItem> CreateForUpdate(IEnumerable<KeyedChange<TKey, TItem>> changes)
    {
        ArgumentNullException.ThrowIfNull(changes);
        
        var capturedChanges = ImmutableArray.CreateRange(changes);
        if (capturedChanges.Length is 0)
            return Empty;
        
        return new()
        {
            Changes = capturedChanges,
            Type    = ChangeSetType.Update
        };
    }

    /// <summary>
    /// Creates a new <see cref="KeyedChangeSet{TKey, TItem}"/> representing a multi-item <see cref="ChangeSetType.Update"/> operation.
    /// </summary>
    /// <param name="changes">The sequence of changes that make up the operation.</param>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> describing the given sequence of collection changes as an <see cref="ChangeSetType.Update"/> operation, or <see cref="Empty"/> if no changes were given.</returns>
    public static KeyedChangeSet<TKey, TItem> CreateForUpdate(ReadOnlySpan<KeyedChange<TKey, TItem>> changes)
    {
        if (changes.Length is 0)
            return Empty;
    
        return new()
        {
            Changes = ImmutableArray.Create(changes),
            Type    = ChangeSetType.Update
        };
    }
}
