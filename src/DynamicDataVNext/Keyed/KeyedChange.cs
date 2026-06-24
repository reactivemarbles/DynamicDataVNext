using System;

namespace DynamicDataVNext;

/// <summary>
/// Contains convenience methods for creating <see cref="KeyedChange{TKey, TItem}"/> values.
/// </summary>
public static partial class KeyedChange { }

/// <summary>
/// Describes a single-item change to a collection of keyed items.
/// </summary>
/// <typeparam name="TKey">The type of the keys of items in the collection.</typeparam>
/// <typeparam name="TItem">The type of the items in the collection.</typeparam>
public readonly partial record struct KeyedChange<TKey, TItem>
    : IChange<KeyedChangeType>
{
    /// <inheritdoc/>
    public ChangeCategory Category
        => Type switch
        {
            KeyedChangeType.None        => ChangeCategory.None,
            KeyedChangeType.Addition    => ChangeCategory.Addition,
            KeyedChangeType.Removal     => ChangeCategory.Removal,
            _                           => ChangeCategory.Other
        };

    /// <inheritdoc/>
    public KeyedChangeType Type { get; private init; }

    /// <summary>
    /// Provides a more-detailed representation of this change as an <see cref="KeyedChangeType.Addition"/> operation. 
    /// </summary>
    /// <returns>A <see cref="KeyedItem{TKey, TItem}"/> describing this change.</returns>
    /// <exception cref="InvalidOperationException">Throws when <see cref="Type"/> is not <see cref="KeyedChangeType.Addition"/>.</exception>
    public KeyedItem<TKey, TItem> AsAddition()
        => (Type is KeyedChangeType.Addition)
            ? new()
            {
                Item    = PrimaryItem,
                Key     = Key
            }
            : throw new InvalidOperationException($"Invalid attempt to interpret a {nameof(KeyedChange)} of type {Type} as type {KeyedChangeType.Addition}");

    /// <summary>
    /// Provides a more-detailed representation of this change as a <see cref="KeyedChangeType.Refreshment"/> operation. 
    /// </summary>
    /// <returns>A <see cref="KeyedItem{TKey, TItem}"/> describing this change.</returns>
    /// <exception cref="InvalidOperationException">Throws when <see cref="Type"/> is not <see cref="KeyedChangeType.Refreshment"/>.</exception>
    public KeyedItem<TKey, TItem> AsRefreshment()
        => (Type is KeyedChangeType.Refreshment)
            ? new()
            {
                Item    = PrimaryItem,
                Key     = Key
            }
            : throw new InvalidOperationException($"Invalid attempt to interpret a {nameof(KeyedChange)} of type {Type} as type {KeyedChangeType.Refreshment}");

    /// <summary>
    /// Provides a more-detailed representation of this change as a <see cref="KeyedChangeType.Removal"/> operation. 
    /// </summary>
    /// <returns>A <see cref="KeyedItem{TKey, TItem}"/> describing this change.</returns>
    /// <exception cref="InvalidOperationException">Throws when <see cref="Type"/> is not <see cref="KeyedChangeType.Removal"/>.</exception>
    public KeyedItem<TKey, TItem> AsRemoval()
        => (Type is KeyedChangeType.Removal)
            ? new()
            {
                Item    = PrimaryItem,
                Key     = Key
            }
            : throw new InvalidOperationException($"Invalid attempt to interpret a {nameof(KeyedChange)} of type {Type} as type {KeyedChangeType.Removal}");

    /// <summary>
    /// Provides a more-detailed representation of this change as a <see cref="KeyedChangeType.Replacement"/> operation. 
    /// </summary>
    /// <returns>A <see cref="KeyedReplacement{TKey, TItem}"/> describing this change.</returns>
    /// <exception cref="InvalidOperationException">Throws when <see cref="Type"/> is not <see cref="KeyedChangeType.Replacement"/>.</exception>
    public KeyedReplacement<TKey, TItem> AsReplacement()
        => (Type is KeyedChangeType.Replacement)
            ? new()
            {
                Key     = Key,
                NewItem = PrimaryItem,
                OldItem = SecondaryItem
            }
            : throw new InvalidOperationException($"Invalid attempt to interpret a {nameof(KeyedChange)} of type {Type} as type {KeyedChangeType.Replacement}");

    internal TKey Key { get; private init; }

    internal TItem PrimaryItem { get; private init; }
        
    private TItem SecondaryItem { get; init; }
}
