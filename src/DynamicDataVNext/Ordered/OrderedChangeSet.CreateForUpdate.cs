using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DynamicDataVNext;

public static partial class OrderedChangeSet
{
    /// <inheritdoc cref="OrderedChangeSet{T}.CreateForUpdate(OrderedChange{T})"/>
    /// <typeparam name="T">The type of the involved items.</typeparam>
    public static OrderedChangeSet<T> CreateForUpdate<T>(OrderedChange<T> change)
        => OrderedChangeSet<T>.CreateForUpdate(change);

    /// <inheritdoc cref="OrderedChangeSet{T}.CreateForUpdate(IEnumerable{OrderedChange{T}})"/>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    public static OrderedChangeSet<T> CreateForUpdate<T>(IEnumerable<OrderedChange<T>> changes)
        => OrderedChangeSet<T>.CreateForUpdate(changes);

    /// <inheritdoc cref="OrderedChangeSet{T}.CreateForUpdate(ReadOnlySpan{OrderedChange{T}})"/>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    public static OrderedChangeSet<T> CreateForUpdate<T>(ReadOnlySpan<OrderedChange<T>> changes)
        => OrderedChangeSet<T>.CreateForUpdate(changes);

    /// <inheritdoc cref="OrderedChangeSet{T}.CreateForUpdate(int, T, int, T)"/>
    /// <typeparam name="T">The type of the involved items.</typeparam>
    public static OrderedChangeSet<T> CreateForUpdate<T>(
            int oldIndex,
            T   oldItem,
            int newIndex,
            T   newItem)
        => OrderedChangeSet<T>.CreateForUpdate(
            oldIndex:   oldIndex,
            oldItem:    oldItem,
            newIndex:   newIndex,
            newItem:    newItem);

    /// <inheritdoc cref="OrderedChangeSet{T}.CreateForUpdate(OrderedUpdate{T})"/>
    /// <typeparam name="T">The type of the involved items.</typeparam>
    public static OrderedChangeSet<T> CreateForUpdate<T>(OrderedUpdate<T> update)
        => OrderedChangeSet<T>.CreateForUpdate(update);
}

public readonly partial record struct OrderedChangeSet<T>
{
    /// <summary>
    /// Creates a new <see cref="OrderedChangeSet{T}"/> representing a single item <see cref="ChangeSetType.Update"/> operation.
    /// </summary>
    /// <param name="change">The change being represented.</param>
    /// <returns>A <see cref="OrderedChangeSet{T}"/> describing the given single-item change, as an <see cref="ChangeSetType.Update"/> operation.</returns>
    public static OrderedChangeSet<T> CreateForUpdate(OrderedChange<T> change)
        => new()
        {
            Changes = ImmutableArray.Create(change),
            Type    = ChangeSetType.Update
        };

    /// <inheritdoc cref="CreateForUpdate(System.ReadOnlySpan{OrderedChange{T}})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="changes"/>.</exception>
    public static OrderedChangeSet<T> CreateForUpdate(IEnumerable<OrderedChange<T>> changes)
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
    /// Creates a new <see cref="OrderedChangeSet{T}"/> representing a multi-item <see cref="ChangeSetType.Update"/> operation.
    /// </summary>
    /// <param name="changes">The sequence of changes that make up the operation.</param>
    /// <returns>A <see cref="OrderedChangeSet{T}"/> describing the given sequence of collection changes as an <see cref="ChangeSetType.Update"/> operation, or <see cref="Empty"/> if no changes were given.</returns>
    public static OrderedChangeSet<T> CreateForUpdate(ReadOnlySpan<OrderedChange<T>> changes)
    {
        if (changes.Length is 0)
            return Empty;
    
        return new()
        {
            Changes = ImmutableArray.Create(changes),
            Type    = ChangeSetType.Update
        };
    }

    /// <summary>
    /// Creates a new <see cref="OrderedChangeSet{T}"/> representing a single-item <see cref="OrderedChangeType.Update"/> operation.
    /// </summary>
    /// <param name="oldIndex">The index of <paramref name="oldItem"/>, before the update occurs.</param>
    /// <param name="oldItem">The replaced item.</param>
    /// <param name="newIndex">The index of <paramref name="newItem"/>, after the update occurs.</param>
    /// <param name="newItem">The replacement item.</param>
    /// <returns>An <see cref="OrderedChangeSet{T}"/> describing the update involving the given items.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if <paramref name="oldIndex"/> or <paramref name="newIndex"/> is negative.</exception>
    public static OrderedChangeSet<T> CreateForUpdate(
            int oldIndex,
            T   oldItem,
            int newIndex,
            T   newItem)
        => new()
        {
            Changes = ImmutableArray.Create(OrderedChange.CreateUpdate(
                oldIndex:   oldIndex,
                oldItem:    oldItem,
                newIndex:   newIndex,
                newItem:    newItem)),
            Type    = ChangeSetType.Update
        };

    /// <summary>
    /// Creates a new <see cref="OrderedChangeSet{T}"/> representing a single-item <see cref="OrderedChangeType.Update"/> operation.
    /// </summary>
    /// <param name="update">The update operation to be described.</param>
    /// <returns>An <see cref="OrderedChangeSet{T}"/> describing the given update operation.</returns>
    public static OrderedChangeSet<T> CreateForUpdate(OrderedUpdate<T> update)
        => new()
        {
            Changes = ImmutableArray.Create(OrderedChange.CreateUpdate(update)),
            Type    = ChangeSetType.Update
        };
}
