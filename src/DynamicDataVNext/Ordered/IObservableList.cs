using System;
using System.Collections.Generic;

namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of ordered items, which publishes notifications about its mutations to subscribers, as they occur.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public interface IObservableList<T>
    : IObservableCollection<T>,
        IList<T>
{
    /// <summary>
    /// The stream of changes describing mutations made to the collection.
    /// </summary>
    DistinctChangeStream<T> ChangeStream { get; }

    /// <inheritdoc cref="AddRange(ReadOnlySpan{T})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    void AddRange(IEnumerable<T> items);

    /// <summary>
    /// Adds a range of items to the end of the list.
    /// </summary>
    /// <param name="items">The items to be added.</param>
    void AddRange(ReadOnlySpan<T> items);

    /// <inheritdoc cref="InsertRange(int, ReadOnlySpan{T})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    void InsertRange(
        int             index,
        IEnumerable<T>  items);

    /// <summary>
    /// Inserts a range of items into the list.
    /// </summary>
    /// <param name="index">The index at which the first item in the range should be inserted.</param>
    /// <param name="items">The items to be inserted.</param>
    /// <exception cref="IndexOutOfRangeException">Throws when <paramref name="index"/> does not represent a valid index of an item in the list, or the next available index of the list.</exception>
    void InsertRange(
        int             index,
        ReadOnlySpan<T> items);

    /// <summary>
    /// Moves an item within the list.
    /// </summary>
    /// <param name="oldIndex">The index of the item to be moved, before the operation.</param>
    /// <param name="newIndex">The desired index of the item to be moved, after the operation.</param>
    /// <exception cref="IndexOutOfRangeException">Throws when <paramref name="oldIndex"/> or <paramref name="newIndex"/> does not represent a valid index of an item in the list.</exception>
    void Move(
        int oldIndex,
        int newIndex);

    /// <summary>
    /// Signals that the given item within the collection has, itself, mutated, triggering a <see cref="DistinctChangeType.Refreshment"/> notification to be published via <see cref="ChangeStream"/>.
    /// </summary>
    /// <param name="index">The index of the item that was refreshed.</param>
    /// <exception cref="IndexOutOfRangeException">Throws when <paramref name="index"/> does not represent a valid index of an item within the list.</exception>
    void Refresh(int index);
    
    /// <summary>
    /// Removes a range of consecutive items from the list.
    /// </summary>
    /// <param name="index">The index of the first item to be removed.</param>
    /// <param name="count">The number of items to be removed.</param>
    /// <exception cref="IndexOutOfRangeException">Throws when <paramref name="index"/> does not represent a valid index of an item within the list.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Throws when <paramref name="count"/> and <paramref name="index"/> define a range that extends beyond the end of the list.</exception>
    void RemoveRange(
        int index,
        int count);

    /// <inheritdoc cref="Reset(ReadOnlySpan{T})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    void Reset(IEnumerable<T> items);

    /// <summary>
    /// Performs a <see cref="ChangeSetType.Reset"/> operation upon the collection, by removing any existing items within the collection, and replacing them with the given items. 
    /// </summary>
    /// <param name="items">The new set of items to be loaded into the collection.</param>
    void Reset(ReadOnlySpan<T> items);
}
