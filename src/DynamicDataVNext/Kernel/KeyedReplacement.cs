namespace DynamicDataVNext.Kernel;

/// <summary>
/// Describes an item within a keyed collection being replaced by another item with the same key.
/// </summary>
/// <typeparam name="TKey">The type of the items' key.</typeparam>
/// <typeparam name="TItem">The type of the items being added and removed.</typeparam>
public readonly record struct KeyedReplacement<TKey, TItem>
{
    /// <summary>
    /// The identifying key of the items.
    /// </summary>
    public required TKey Key { get; init; }

    /// <summary>
    /// The replacement item.
    /// </summary>
    public required TItem NewItem { get; init; }

    /// <summary>
    /// The item being replaced.
    /// </summary>
    public required TItem OldItem { get; init; }
}
