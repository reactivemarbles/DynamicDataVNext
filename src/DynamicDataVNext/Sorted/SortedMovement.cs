namespace DynamicDataVNext;

/// <summary>
/// Describes an item being moved within a sorted collection.
/// </summary>
/// <typeparam name="T">The type of the item being moved.</typeparam>
public readonly record struct SortedMovement<T>
{
    /// <summary>
    /// The item being moved.
    /// </summary>
    public required T Item { get; init; }

    /// <summary>
    /// The index of the item within the collection, after being moved.
    /// </summary>
    public required int NewIndex { get; init; }

    /// <summary>
    /// The index of the item within the collection, before being moved.
    /// </summary>
    public required int OldIndex { get; init; }

    /// <summary>
    /// Checks whether a given index is affected by this movement change.
    /// </summary>
    /// <param name="index">The index to check.</param>
    /// <returns><see langword="true"/> if the given index falls within the range of moved items represented by this change; <see langword="false"/> otherwise.</returns>
    public bool IsIndexAffected(int index)
        =>      ((index >= NewIndex) && (index <= OldIndex))
            ||  ((index >= OldIndex) && (index <= NewIndex));
}
