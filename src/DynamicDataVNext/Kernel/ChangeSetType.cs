namespace DynamicDataVNext.Kernel;

/// <summary>
/// Identifies the types of operations that can be performed upon a collection.
/// This is useful to identify operations that affect an entire collection at once, allowing consumers of changesets to apply them without necessarily having to interate over each individual change, or to invoke methods on the collection object to apply all changes at once, in a more-optimized fashion.
/// </summary>
public enum ChangeSetType
{
    /// <summary>
    /// Identifies a changeset created from an operation that does not affect a collection as a whole. Its changes should be processed individually.
    /// </summary>
    Update,
    /// <summary>
    /// Identifies a changeset created from a clear operation, where all items in a collection were removed.
    /// </summary>
    Clear,
    /// <summary>
    /// Identifies a changeset created from a reset operation, where a non-empty collection was cleared and populated with new items, in one operation.
    /// </summary>
    Reset
}
