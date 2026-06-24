using System.Collections.Immutable;

namespace DynamicDataVNext;

/// <summary>
/// Represents a <see cref="ChangeSetType.Clear"/> operation performed upon a collection of distinct items.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public readonly struct DistinctClear<T>
{
    internal DistinctClear(ImmutableArray<DistinctChange<T>> changes)
        => _changes = changes;
    
    /// <summary>
    /// The items in the cleared collection.
    /// </summary>
    public DistinctChangeItemCollection<T> Items
        => new()
        {
            Changes     = _changes,
            FirstIndex  = 0,
            LastIndex   = _changes.Length - 1 
        };
    
    private readonly ImmutableArray<DistinctChange<T>> _changes;
}
