using System;

namespace DynamicDataVNext.Kernel;

/// <summary>
/// Contains convenience methods for creating <see cref="SortedChange{T}"/> values.
/// </summary>
public static class SortedChange
{
    /// <inheritdoc cref="SortedChange{T}.Insertion(int, T)"/>
    public static SortedChange<T> Insertion<T>(
            int index,
            T   item)
        => SortedChange<T>.Insertion(
            index:  index,
            item:   item);

    /// <inheritdoc cref="SortedChange{T}.Movement(int, int, T)"/>
    public static SortedChange<T> Movement<T>(
            int oldIndex,
            int newIndex,
            T   item)
        => SortedChange<T>.Movement(
            oldIndex:   oldIndex,
            newIndex:   newIndex,
            item:       item);

    /// <inheritdoc cref="SortedChange{T}.Removal(int, T)"/>
    public static SortedChange<T> Removal<T>(
            int index,
            T   item)
        => SortedChange<T>.Removal(
            index:  index,
            item:   item);

    /// <inheritdoc cref="SortedChange{T}.Replacement(int, T, T)"/>
    public static SortedChange<T> Replacement<T>(
            int index,
            T   oldItem,
            T   newItem)
        => SortedChange<T>.Replacement(
            index:      index,
            newItem:    newItem,
            oldItem:    oldItem);

    /// <inheritdoc cref="SortedChange{T}.Update(int, T, int, T)"/>
    public static SortedChange<T> Update<T>(
            int oldIndex,
            T   oldItem,
            int newIndex,
            T   newItem)
        => SortedChange<T>.Update(
            oldIndex:   oldIndex,
            oldItem:    oldItem,
            newIndex:   newIndex,
            newItem:    newItem);
}

/// <summary>
/// Describes a single-item change made (or to be made) upon a sorted collection.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public readonly record struct SortedChange<T>
{
    /// <summary>
    /// Creates a new <see cref="SortedChange{T}"/> representing the insertion of a given item.
    /// </summary>
    /// <param name="index">The index at which the item is being inserted.</param>
    /// <param name="item">The item being inserted.</param>
    /// <returns>A <see cref="SortedChange{T}"/> describing the insertion of the given item.</returns>
    public static SortedChange<T> Insertion(
            int index,
            T   item)
        => new()
        {
            NewIndex    = index,
            NewItem     = item,
            Type        = SortedChangeType.Insertion
        };

    /// <summary>
    /// Creates a new <see cref="SortedChange{T}"/> representing the movement of a given item, within a collection.
    /// </summary>
    /// <param name="oldIndex">The index of the item, before being moved.</param>
    /// <param name="newIndex">The index of the item, after being moved..</param>
    /// <param name="item">The item being moved.</param>
    /// <returns>A <see cref="SortedChange{T}"/> describing the movement of the given item.</returns>
    public static SortedChange<T> Movement(
            int oldIndex,
            int newIndex,
            T   item)
        => new()
        {
            NewIndex    = newIndex,
            NewItem     = item,
            OldIndex    = oldIndex,
            Type        = SortedChangeType.Movement
        };

    /// <summary>
    /// Creates a new <see cref="SortedChange{T}"/> representing the removal of a given item.
    /// </summary>
    /// <param name="index">The index of the item being removed.</param>
    /// <param name="item">The item being removed.</param>
    /// <returns>A <see cref="SortedChange{T}"/> describing the removal of the given item.</returns>
    public static SortedChange<T> Removal(
            int index,
            T   item)
        => new()
        {
            OldIndex    = index,
            OldItem     = item,
            Type        = SortedChangeType.Removal
        };

    /// <summary>
    /// Creates a new <see cref="SortedChange{T}"/> representing the replacement of a given item by another.
    /// </summary>
    /// <param name="index">The index of the item being replaced.</param>
    /// <param name="oldItem">The item being replaced.</param>
    /// <param name="newItem">The replacement item.</param>
    /// <returns>A <see cref="SortedChange{T}"/> describing the replacement of the given items.</returns>
    public static SortedChange<T> Replacement(
            int index,
            T   oldItem,
            T   newItem)
        => new()
        {
            OldItem     = oldItem,
            NewIndex    = index,
            NewItem     = newItem,
            Type        = SortedChangeType.Replacement
        };

    /// <summary>
    /// Creates a new <see cref="SortedChange{T}"/> representing the replacement of a given item by another, and their movement within a collection.
    /// </summary>
    /// <param name="oldIndex">The index of <paramref name="oldItem"/>, before being moved.</param>
    /// <param name="oldItem">The item being replaced.</param>
    /// <param name="newIndex">The index of <paramref name="newItem"/>, after being moved..</param>
    /// <param name="newItem">The replacement item.</param>
    /// <returns>A <see cref="SortedChange{T}"/> describing the replacement and movement of the given items.</returns>
    public static SortedChange<T> Update(
            int oldIndex,
            T   oldItem,
            int newIndex,
            T   newItem)
        => new()
        {
            OldIndex    = oldIndex,
            OldItem     = oldItem,
            NewIndex    = newIndex,
            NewItem     = newItem,
            Type        = SortedChangeType.Update
        };

    private readonly int                _newIndex;
    private readonly T                  _newItem;
    private readonly int                _oldIndex;
    private readonly T                  _oldItem;
    private readonly SortedChangeType   _type;


    /// <summary>
    /// The type of single-item change being made.
    /// </summary>
    public SortedChangeType Type
    {
        get => _type;
        private init => _type = value;
    }

    /// <summary>
    /// Interprets the information within this change as an insertion operation.
    /// </summary>
    /// <returns>A <see cref="SortedItem{T}"/> describing this change.</returns>
    /// <exception cref="InvalidOperationException"><see cref="Type"/> is not <see cref="SortedChangeType.Insertion"/>.</exception>
    public SortedItem<T> AsInsertion()
        => (Type is SortedChangeType.Insertion)
            ? new()
            {
                Index   = NewIndex,
                Item    = NewItem
            }
            : throw new InvalidOperationException($"Invalid attempt to interpret a {nameof(SortedChange)} of type {Type} as type {SortedChangeType.Insertion}");

    /// <summary>
    /// Interprets the information within this change as a movement operation.
    /// </summary>
    /// <returns>A <see cref="SortedMovement{T}"/> describing this change.</returns>
    /// <exception cref="InvalidOperationException"><see cref="Type"/> is not <see cref="SortedChangeType.Movement"/>.</exception>
    public SortedMovement<T> AsMovement()
        => (Type is SortedChangeType.Movement)
            ? new()
            {
                Item        = NewItem,
                NewIndex    = NewIndex,
                OldIndex    = OldIndex
            }
            : throw new InvalidOperationException($"Invalid attempt to interpret a {nameof(SortedChange)} of type {Type} as type {SortedChangeType.Movement}");

    /// <summary>
    /// Interprets the information within this change as a removal operation.
    /// </summary>
    /// <returns>A <see cref="SortedRemoval{T}"/> describing this change.</returns>
    /// <exception cref="InvalidOperationException"><see cref="Type"/> is not <see cref="SortedChangeType.Removal"/>.</exception>
    public SortedItem<T> AsRemoval()
        => (Type is SortedChangeType.Removal)
            ? new()
            {
                Index   = OldIndex,
                Item    = OldItem
            }
            : throw new InvalidOperationException($"Invalid attempt to interpret a {nameof(SortedChange)} of type {Type} as type {SortedChangeType.Removal}");

    /// <summary>
    /// Interprets the information within this change as a replacement operation.
    /// </summary>
    /// <returns>A <see cref="SortedReplacement{T}"/> describing this change.</returns>
    /// <exception cref="InvalidOperationException"><see cref="Type"/> is not <see cref="SortedChangeType.Replacement"/>.</exception>
    public SortedReplacement<T> AsReplacement()
        => (Type is SortedChangeType.Replacement)
            ? new()
            {
                Index   = NewIndex,
                NewItem = NewItem,
                OldItem = OldItem
            }
            : throw new InvalidOperationException($"Invalid attempt to interpret a {nameof(SortedChange)} of type {Type} as type {SortedChangeType.Replacement}");

    /// <summary>
    /// Interprets the information within this change as an update operation.
    /// </summary>
    /// <returns>A <see cref="SortedUpdate{T}"/> describing this change.</returns>
    /// <exception cref="InvalidOperationException"><see cref="Type"/> is not <see cref="SortedChangeType.Update"/>.</exception>
    public SortedUpdate<T> AsUpdate()
        => (Type is SortedChangeType.Update)
            ? new()
            {
                NewIndex    = NewIndex,
                NewItem     = NewItem,
                OldIndex    = OldIndex,
                OldItem     = OldItem
            }
            : throw new InvalidOperationException($"Invalid attempt to interpret a {nameof(SortedChange)} of type {Type} as type {SortedChangeType.Update}");

    private int NewIndex
    {
        get => _newIndex;
        init => _newIndex = value;
    }
    
    private T NewItem
    {
        get => _newItem;
        init => _newItem = value;
    }
    
    private int OldIndex
    {
        get => _oldIndex;
        init => _oldIndex = value;
    }
    
    private T OldItem
    {
        get => _oldItem;
        init => _oldItem = value;
    }
}
