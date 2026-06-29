using System;
using System.Collections.Immutable;

namespace DynamicDataVNext;

/// <summary>
/// Describes the basic structural representation of a sequence of single-item changes to a collection.
/// </summary>
/// <typeparam name="TChange">The type of single-item changes being sequenced.</typeparam>
/// <typeparam name="TChangeType">An enum describing the types of single-item change actions that can be represented by this structure.</typeparam>
/// <remarks>
/// This exists to allow for certain bits of logic regarding how changesets are built to be shared between all different types of collections. I.E. it's a compatibility tag interface, it's not intended to allow many different types of changes to be grouped together. It should basically never be used except as a generic type constraint, as that would result in boxing allocations. 
/// </remarks>
public interface IChangeSet<TChange, out TChangeType>
    where TChange : struct, IChange<TChangeType>
    where TChangeType : Enum
{
    /// <summary>
    /// The sequence of single-item changes that make up the operation.
    /// </summary>
    ImmutableArray<TChange> Changes { get; }

    /// <summary>
    /// The type of operation being described.
    /// </summary>
    ChangeSetType Type { get; }
}
