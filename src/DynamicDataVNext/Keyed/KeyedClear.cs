using System.Collections.Immutable;

namespace DynamicDataVNext;

/// <summary>
/// Represents a <see cref="ChangeSetType.Clear"/> operation performed upon a collection of keyed items.
/// </summary>
/// <typeparam name="TKey">The type of the items' keys.</typeparam>
/// <typeparam name="TItem">The type of the items in the collection.</typeparam>
public readonly struct KeyedClear<TKey, TItem>
{
    internal KeyedClear(ImmutableArray<KeyedChange<TKey, TItem>> changes)
        => _changes = changes;
    
    /// <summary>
    /// The items in the cleared collection.
    /// </summary>
    public KeyedChangeItemCollection<TKey, TItem> Items
        => new()
        {
            Changes     = _changes,
            FirstIndex  = 0,
            LastIndex   = _changes.Length - 1 
        };
    
    private readonly ImmutableArray<KeyedChange<TKey, TItem>> _changes;
}
