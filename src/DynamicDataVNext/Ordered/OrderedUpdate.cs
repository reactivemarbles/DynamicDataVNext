namespace DynamicDataVNext;

/// <summary>
/// Describes an item within a collection of ordered items being replaced by another, and moved at the same time.
/// </summary>
/// <typeparam name="T">The type of the collection items.</typeparam>
/// <remarks>
/// This can alternatively be considered as a two-item change, I.E. and item being removed and another item being inserted, but it's defined here as a type of single-item change, since replacing an item may often require it to be moved as well, to maintain proper sorting of the collection.
/// </remarks>
public readonly record struct OrderedUpdate<T>
{
    /// <summary>
    /// The 0-based index of the position of <see cref="NewItem"/>, within its collection, after the update is performed.
    /// </summary>
    public required int NewIndex { get; init; }

    /// <summary>
    /// The replacement item.
    /// </summary>
    public required T NewItem { get; init; }
    
    /// <summary>
    /// The 0-based index of the position of <see cref="OldItem"/>, within its collection, before the update is performed.
    /// </summary>
    public required int OldIndex { get; init; }

    /// <summary>
    /// The replaced item.
    /// </summary>
    public required T OldItem { get; init; }
}
