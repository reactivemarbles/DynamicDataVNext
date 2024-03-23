namespace DynamicDataVNext;

/// <summary>
/// Describes an item within a keyed collection.
/// </summary>
/// <typeparam name="TKey">The type of the item's key.</typeparam>
/// <typeparam name="TItem">The type of the item.</typeparam>
public readonly record struct KeyedItem<TKey, TItem>
{
    /// <summary>
    /// The collection item.
    /// </summary>
    public required TItem Item { get; init; }

    /// <summary>
    /// The identifying key of the item.
    /// </summary>
    public required TKey Key { get; init; }
}
