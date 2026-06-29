using System;
using System.Collections.Immutable;

namespace DynamicDataVNext;

/// <summary>
/// Contains convenience methods for creating <see cref="KeyedChangeSet{TKey, TItem}"/> structures.
/// </summary>
public static partial class KeyedChangeSet { }

/// <summary>
/// Describes a change operation performed upon a collection of keyed items, in the form of a sequence of single-item changes.
/// </summary>
/// <typeparam name="TKey">The type of the items' keys.</typeparam>
/// <typeparam name="TItem">The type of the items in the collection.</typeparam>
public readonly partial record struct KeyedChangeSet<TKey, TItem>
    : IChangeSet<KeyedChange<TKey, TItem>, KeyedChangeType>
{
    /// <summary>
    /// The sequence of single-item changes that make up the operation.
    /// </summary>
    public ImmutableArray<KeyedChange<TKey, TItem>> Changes { get; private init; }

    /// <summary>
    /// The type of operation being described.
    /// </summary>
    public ChangeSetType Type { get; private init; }

    /// <summary>
    /// Provides a more-detailed representation of this changeset as a <see cref="ChangeSetType.Clear"/> operation. 
    /// </summary>
    /// <returns>A <see cref="KeyedClear{TKey, TItem}"/> representation of this changeset.</returns>
    /// <exception cref="InvalidOperationException">Throws when <see cref="Type"/> is not <see cref="ChangeSetType.Clear"/>.</exception>
    public KeyedClear<TKey, TItem> AsClear()
        => Type is ChangeSetType.Clear
            ? new(changes: Changes)
            : throw new InvalidOperationException($"Unable to interpret {nameof(KeyedChangeSet)} of type {Type} as {nameof(ChangeSetType.Clear)}");
    
    /// <summary>
    /// Provides a more-detailed representation of this changeset as a <see cref="ChangeSetType.Reset"/> operation. 
    /// </summary>
    /// <returns>A <see cref="KeyedReset{TKey, TItem}"/> representation of this changeset.</returns>
    /// <exception cref="InvalidOperationException">Throws when <see cref="Type"/> is not <see cref="ChangeSetType.Reset"/>.</exception>
    public KeyedReset<TKey, TItem> AsReset()
        => Type is ChangeSetType.Reset
            ? new(
                changes:            Changes,
                firstAdditionIndex: FirstAdditionIndex)
            : throw new InvalidOperationException($"Unable to interpret {nameof(KeyedChangeSet)} of type {Type} as {nameof(ChangeSetType.Reset)}");
    
    private int FirstAdditionIndex { get; init; }
}
