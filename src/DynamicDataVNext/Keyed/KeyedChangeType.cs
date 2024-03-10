namespace DynamicDataVNext;

/// <summary>
/// Identifies the types of single-item operations that can be performed upon a keyed collection.
/// </summary>
public enum KeyedChangeType
{
    /// <summary>
    /// A safeguard value to prevent uninitialized changes from going unnoticed. Should not be used, and should be considered invalid if encountered.
    /// </summary>
    None,
    /// <summary>
    /// Identifies that an item was added to a collection.
    /// </summary>
    Addition,
    /// <summary>
    /// Identifies that an item was removed from a collection.
    /// </summary>
    Removal,
    /// <summary>
    /// Identifies that an existing item was replaced with another, within a collection.
    /// </summary>
    Replacement
}
