namespace DynamicDataVNext;

/// <summary>
/// Describes an item within a collection of ordered items being replaced by another.
/// </summary>
/// <typeparam name="T">The type of the collection items.</typeparam>
public readonly record struct OrderedReplacement<T>
{
    /// <summary>
    /// The 0-based index of the collection position at which the replacement is occurring.
    /// </summary>
    public required int Index { get; init; }

    /// <summary>
    /// The replacement item.
    /// </summary>
    public required T NewItem { get; init; }
    
    /// <summary>
    /// The replaced item.
    /// </summary>
    public required T OldItem { get; init; }
}
