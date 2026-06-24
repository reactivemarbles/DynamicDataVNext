using System;

namespace DynamicDataVNext;

/// <summary>
/// Contains convenience methods for creating <see cref="OrderedChange{T}"/> values.
/// </summary>
public static partial class OrderedChange { }

/// <summary>
/// Describes a single-item change to a collection of ordered items.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public readonly partial record struct OrderedChange<T>
    : IChange<OrderedChangeType>
{
    /// <inheritdoc/>
    public ChangeCategory Category
        => Type switch
        {
            OrderedChangeType.None      => ChangeCategory.None,
            OrderedChangeType.Insertion => ChangeCategory.Addition,
            OrderedChangeType.Removal   => ChangeCategory.Removal,
            _                           => ChangeCategory.Other
        };

    /// <inheritdoc/>
    public OrderedChangeType Type { get; private init; }

    /// <summary>
    /// Provides a more-detailed representation of this change as an <see cref="OrderedChangeType.Insertion"/> operation. 
    /// </summary>
    /// <returns>An <see cref="OrderedItem{T}"/> describing this change.</returns>
    /// <exception cref="InvalidOperationException">Throws when <see cref="Type"/> is not <see cref="OrderedChangeType.Insertion"/>.</exception>
    public OrderedItem<T> AsInsertion()
        => (Type is OrderedChangeType.Insertion)
            ? new()
            {
                Index   = PrimaryIndex,
                Item    = PrimaryItem
            }
            : throw new InvalidOperationException($"Invalid attempt to interpret a {nameof(OrderedChange)} of type {Type} as type {OrderedChangeType.Insertion}");

    /// <summary>
    /// Provides a more-detailed representation of this change as a <see cref="OrderedChangeType.Movement"/> operation. 
    /// </summary>
    /// <returns>An <see cref="OrderedMovement{T}"/> describing this change.</returns>
    /// <exception cref="InvalidOperationException">Throws when <see cref="Type"/> is not <see cref="OrderedChangeType.Movement"/>.</exception>
    public OrderedMovement<T> AsMovement()
        => (Type is OrderedChangeType.Movement)
            ? new()
            {
                Item        = PrimaryItem,
                NewIndex    = PrimaryIndex,
                OldIndex    = SecondaryIndex
            }
            : throw new InvalidOperationException($"Invalid attempt to interpret a {nameof(OrderedChange)} of type {Type} as type {OrderedChangeType.Movement}");

    /// <summary>
    /// Provides a more-detailed representation of this change as a <see cref="OrderedChangeType.Refreshment"/> operation. 
    /// </summary>
    /// <returns>An <see cref="OrderedItem{T}"/> describing this change.</returns>
    /// <exception cref="InvalidOperationException">Throws when <see cref="Type"/> is not <see cref="OrderedChangeType.Refreshment"/>.</exception>
    public OrderedItem<T> AsRefreshment()
        => (Type is OrderedChangeType.Refreshment)
            ? new()
            {
                Index   = PrimaryIndex,
                Item    = PrimaryItem
            }
            : throw new InvalidOperationException($"Invalid attempt to interpret a {nameof(OrderedChange)} of type {Type} as type {OrderedChangeType.Refreshment}");

    /// <summary>
    /// Provides a more-detailed representation of this change as a <see cref="OrderedChangeType.Removal"/> operation. 
    /// </summary>
    /// <returns>An <see cref="OrderedItem{T}"/> describing this change.</returns>
    /// <exception cref="InvalidOperationException">Throws when <see cref="Type"/> is not <see cref="OrderedChangeType.Removal"/>.</exception>
    public OrderedItem<T> AsRemoval()
        => (Type is OrderedChangeType.Removal)
            ? new()
            {
                Index   = PrimaryIndex,
                Item    = PrimaryItem
            }
            : throw new InvalidOperationException($"Invalid attempt to interpret a {nameof(OrderedChange)} of type {Type} as type {OrderedChangeType.Removal}");

    /// <summary>
    /// Provides a more-detailed representation of this change as a <see cref="OrderedChangeType.Replacement"/> operation. 
    /// </summary>
    /// <returns>An <see cref="OrderedReplacement{T}"/> describing this change.</returns>
    /// <exception cref="InvalidOperationException">Throws when <see cref="Type"/> is not <see cref="OrderedChangeType.Replacement"/>.</exception>
    public OrderedReplacement<T> AsReplacement()
        => (Type is OrderedChangeType.Replacement)
            ? new()
            {
                Index   = PrimaryIndex,
                NewItem = PrimaryItem,
                OldItem = SecondaryItem
            }
            : throw new InvalidOperationException($"Invalid attempt to interpret a {nameof(OrderedChange)} of type {Type} as type {OrderedChangeType.Replacement}");

    /// <summary>
    /// Provides a more-detailed representation of this change as an <see cref="OrderedChangeType.Update"/> operation. 
    /// </summary>
    /// <returns>A <see cref="OrderedUpdate{T}"/> describing this change.</returns>
    /// <exception cref="InvalidOperationException">Throws when <see cref="Type"/> is not <see cref="OrderedChangeType.Update"/>.</exception>
    public OrderedUpdate<T> AsUpdate()
        => (Type is OrderedChangeType.Update)
            ? new()
            {
                NewIndex    = PrimaryIndex,
                NewItem     = PrimaryItem,
                OldIndex    = SecondaryIndex,
                OldItem     = SecondaryItem
            }
            : throw new InvalidOperationException($"Invalid attempt to interpret a {nameof(OrderedChange)} of type {Type} as type {OrderedChangeType.Update}");

    internal int PrimaryIndex { get; private init; }

    internal T PrimaryItem { get; private init; }
        
    private int SecondaryIndex { get; init; }

    private T SecondaryItem { get; init; }
}
