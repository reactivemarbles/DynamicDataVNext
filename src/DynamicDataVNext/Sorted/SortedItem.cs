namespace DynamicDataVNext;

/// <summary>
/// Describes an item within a sorted collection.
/// </summary>
/// <typeparam name="T">The type of the item.</typeparam>
public readonly record struct SortedItem<T>
{
    /// <summary>
    /// The index of the item.
    /// </summary>
    public required int Index { get; init; }

    /// <summary>
    /// The collection item.
    /// </summary>
    public required T Item { get; init; }
}
