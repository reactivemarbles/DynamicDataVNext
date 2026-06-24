using System;
using System.Collections.Immutable;

namespace DynamicDataVNext;

/// <summary>
/// Contains convenience methods for creating <see cref="DistinctChangeSet{T}"/> structures.
/// </summary>
public static partial class DistinctChangeSet { }

/// <summary>
/// Describes a change operation performed upon a collection of distinct items, in the form of a sequence of single-item changes.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public readonly partial record struct DistinctChangeSet<T>
{
    /// <summary>
    /// The sequence of single-item changes that make up the operation.
    /// </summary>
    public ImmutableArray<DistinctChange<T>> Changes { get; private init; }

    /// <summary>
    /// The type of operation being described.
    /// </summary>
    public ChangeSetType Type { get; private init; }

    /// <summary>
    /// Provides a more-detailed representation of this changeset as a <see cref="ChangeSetType.Clear"/> operation. 
    /// </summary>
    /// <returns>A <see cref="DistinctClear{T}"/> representation of this changeset.</returns>
    /// <exception cref="InvalidOperationException">Throws when <see cref="Type"/> is not <see cref="ChangeSetType.Clear"/>.</exception>
    public DistinctClear<T> AsClear()
        => Type is ChangeSetType.Clear
            ? new(changes: Changes)
            : throw new InvalidOperationException($"Unable to interpret {nameof(DistinctChangeSet)} of type {Type} as {nameof(ChangeSetType.Clear)}");
    
    /// <summary>
    /// Provides a more-detailed representation of this changeset as a <see cref="ChangeSetType.Reset"/> operation. 
    /// </summary>
    /// <returns>A <see cref="DistinctReset{T}"/> representation of this changeset.</returns>
    /// <exception cref="InvalidOperationException">Throws when <see cref="Type"/> is not <see cref="ChangeSetType.Reset"/>.</exception>
    public DistinctReset<T> AsReset()
        => Type is ChangeSetType.Reset
            ? new(
                changes:            Changes,
                firstAdditionIndex: FirstAdditionIndex)
            : throw new InvalidOperationException($"Unable to interpret {nameof(DistinctChangeSet)} of type {Type} as {nameof(ChangeSetType.Reset)}");
    
    private int FirstAdditionIndex { get; init; }
}
