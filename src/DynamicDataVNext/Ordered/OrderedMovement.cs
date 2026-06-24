namespace DynamicDataVNext;

/// <summary>
/// Describes an item being moved within a collection of ordered items.
/// </summary>
/// <typeparam name="T">The type of the moved item.</typeparam>
public readonly record struct OrderedMovement<T>
{
    /// <summary>
    /// The moved item.
    /// </summary>
    public required T Item { get; init; }

    /// <summary>
    /// The 0-based index of the item's position, within its collection, after being moved.
    /// </summary>
    public required int NewIndex { get; init; }

    /// <summary>
    /// The 0-based index of the item's position, within its collection, before being moved.
    /// </summary>
    public required int OldIndex { get; init; }
}
