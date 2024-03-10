namespace DynamicDataVNext.Kernel;

/// <summary>
/// Identifies the types of single-item operations that can be performed upon a collection of distinct items.
/// </summary>
public enum DistinctChangeType
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
    Removal
}
