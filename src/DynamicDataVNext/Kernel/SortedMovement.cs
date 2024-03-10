namespace DynamicDataVNext.Kernel;

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
}
