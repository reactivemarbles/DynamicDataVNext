namespace DynamicDataVNext;

/// <summary>
/// Describes an item within a sorted collection being replaced by another.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public readonly record struct SortedReplacement<T>
{
    /// <summary>
    /// The index at which the replacement is occurring.
    /// </summary>
    public required int Index { get; init; }

    /// <summary>
    /// The replacement item.
    /// </summary>
    public required T NewItem { get; init; }
    
    /// <summary>
    /// The item being replaced.
    /// </summary>
    public required T OldItem { get; init; }
}
