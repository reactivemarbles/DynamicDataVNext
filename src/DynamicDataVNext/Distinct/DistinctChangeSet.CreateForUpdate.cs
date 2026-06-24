using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DynamicDataVNext;

public static partial class DistinctChangeSet
{
    /// <inheritdoc cref="DistinctChangeSet{T}.CreateForUpdate(DistinctChange{T})"/>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    public static DistinctChangeSet<T> CreateForUpdate<T>(DistinctChange<T> change)
        => DistinctChangeSet<T>.CreateForUpdate(change);
    
    /// <inheritdoc cref="DistinctChangeSet{T}.CreateForUpdate(IEnumerable{DistinctChange{T}})"/>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    public static DistinctChangeSet<T> CreateForUpdate<T>(IEnumerable<DistinctChange<T>> changes)
        => DistinctChangeSet<T>.CreateForUpdate(changes);

    /// <inheritdoc cref="DistinctChangeSet{T}.CreateForUpdate(ReadOnlySpan{DistinctChange{T}})"/>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    public static DistinctChangeSet<T> CreateForUpdate<T>(ReadOnlySpan<DistinctChange<T>> changes)
        => DistinctChangeSet<T>.CreateForUpdate(changes);
}

public readonly partial record struct DistinctChangeSet<T>
{
    /// <summary>
    /// Creates a new <see cref="DistinctChangeSet{T}"/> representing a single-item <see cref="ChangeSetType.Update"/> operation.
    /// </summary>
    /// <param name="change">The change being represented.</param>
    /// <returns>A <see cref="DistinctChangeSet{T}"/> describing the given single-item change, as an <see cref="ChangeSetType.Update"/> operation.</returns>
    public static DistinctChangeSet<T> CreateForUpdate(DistinctChange<T> change)
        => new()
        {
            Changes = ImmutableArray.Create(change),
            Type    = ChangeSetType.Update
        };

    /// <inheritdoc cref="CreateForUpdate(System.ReadOnlySpan{DistinctChange{T}})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="changes"/>.</exception>
    public static DistinctChangeSet<T> CreateForUpdate(IEnumerable<DistinctChange<T>> changes)
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
    /// Creates a new <see cref="DistinctChangeSet{T}"/> representing a multi-item <see cref="ChangeSetType.Update"/> operation.
    /// </summary>
    /// <param name="changes">The sequence of changes that make up the operation.</param>
    /// <returns>A <see cref="DistinctChangeSet{T}"/> describing the given sequence of collection changes as an <see cref="ChangeSetType.Update"/> operation, or <see cref="Empty"/> if no changes were given.</returns>
    public static DistinctChangeSet<T> CreateForUpdate(ReadOnlySpan<DistinctChange<T>> changes)
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
