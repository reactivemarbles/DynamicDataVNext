namespace DynamicDataVNext.Kernel;

/// <summary>
/// Describes an item within a sorted collection being replaced by another, and moved at the same time.
/// This can alternatively be considered as a two-item change, I.E. and item being removed and another item being inserted, but it's defined here as a type of single-item change, since replacing an item may often require it to be moved as well, to maintain proper sorting of the collection.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public readonly record struct SortedUpdate<T>
{
    /// <summary>
    /// The index of <see cref="NewItem"/> after the update is performed.
    /// </summary>
    public required int NewIndex { get; init; }

    /// <summary>
    /// The replacement item.
    /// </summary>
    public required T NewItem { get; init; }
    
    /// <summary>
    /// The index of <see cref="OldItem"/> before the update is performed.
    /// </summary>
    public required int OldIndex { get; init; }

    /// <summary>
    /// The item being replaced.
    /// </summary>
    public required T OldItem { get; init; }
}
