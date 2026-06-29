using System;

namespace DynamicDataVNext;

/// <summary>
/// Describes an item within a collection of ordered items.
/// </summary>
/// <typeparam name="T">The type of the item.</typeparam>
public readonly record struct OrderedItem<T>
{
    /// <summary>
    /// The 0-based index of the item's position, within its collection.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Throws if set to a negative value</exception>
    public required int Index
    {
        get;
        init
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            
            field = value;
        }
    }

    /// <summary>
    /// The collection item.
    /// </summary>
    public required T Item { get; init; }
}
