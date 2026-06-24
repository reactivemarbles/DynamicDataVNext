using System.Collections.Immutable;

namespace DynamicDataVNext;

/// <summary>
/// Represents a <see cref="ChangeSetType.Clear"/> operation performed upon a collection of ordered items.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public readonly struct OrderedClear<T>
{
    internal OrderedClear(ImmutableArray<OrderedChange<T>> changes)
        => _changes = changes;
    
    /// <summary>
    /// The items in the cleared collection, in reverse order of their appearance within the collection.
    /// </summary>
    public OrderedChangeItemCollection<T> ReversedItems
        => new()
        {
            Changes     = _changes,
            FirstIndex  = 0,
            LastIndex   = _changes.Length - 1 
        };
    
    private readonly ImmutableArray<OrderedChange<T>> _changes;
}
