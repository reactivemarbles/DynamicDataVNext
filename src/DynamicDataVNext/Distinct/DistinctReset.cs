using System.Collections.Immutable;

namespace DynamicDataVNext;

/// <summary>
/// Represents a <see cref="ChangeSetType.Reset"/> operation performed upon a collection of distinct items.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public readonly struct DistinctReset<T>
{
    internal DistinctReset(
        ImmutableArray<DistinctChange<T>>   changes,
        int                                 firstAdditionIndex)
    {
        _changes            = changes;
        _firstAdditionIndex = firstAdditionIndex;
    }
    
    /// <summary>
    /// The items added to the collection by the reset operation.
    /// </summary>
    public DistinctChangeItemCollection<T> Additions
        => new()
        {
            Changes     = _changes,
            FirstIndex  = _firstAdditionIndex,
            LastIndex   = _changes.Length - 1 
        };
    
    /// <summary>
    /// The items removed from the collection by the reset operation.
    /// </summary>
    public DistinctChangeItemCollection<T> Removals
        => new()
        {
            Changes     = _changes,
            FirstIndex  = 0,
            LastIndex   = _firstAdditionIndex - 1 
        };
    
    private readonly ImmutableArray<DistinctChange<T>>  _changes;
    private readonly int                                _firstAdditionIndex;
}
