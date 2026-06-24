namespace DynamicDataVNext;

/// <summary>
/// Identifies the types of single-item operations that can be performed upon a collection or ordered items.
/// </summary>
public enum OrderedChangeType
{
    /// <summary>
    /// A safeguard value to prevent uninitialized changes from going unnoticed. Should not be used, and should be considered invalid if encountered.
    /// </summary>
    None,
    /// <summary>
    /// Identifies that an item was inserted into a collection.
    /// </summary>
    Insertion,
    /// <summary>
    /// Identifies that an item was moved within a collection.
    /// </summary>
    Movement,
    /// <summary>
    /// Identifies that an item was removed from a collection.
    /// </summary>
    Removal,
    /// <summary>
    /// Identifies that an existing item within a collection was replaced with another.
    /// </summary>
    Replacement,
    /// <summary>
    /// Identifies that an existing item within a collection was replaced with another, and moved at the same time.
    /// </summary>
    /// <remarks>
    /// This can alternatively be considered as a two-item change, I.E. an item being removed and another item being inserted, but it's included as a type of single-item change, since replacing an item may often require it to be moved as well, to maintain proper sorting of the collection.
    /// </remarks>
    Update,
    /// <summary>
    /// Identifies that an item within a collection has been mutated, and should be re-evaluated by downstream listeners that care about its state.
    /// </summary>
    Refreshment
}
