using System;
using System.Collections.Generic;

namespace DynamicDataVNext;

/// <summary>
/// Describes an extended version of <see cref="IList{T}"/>, supporting range and movement operations.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public interface IExtendedList<T>
    : IList<T>
{
    /// <summary>
    /// Adds a range of items to the end of the list.
    /// </summary>
    /// <param name="items">The items to be added.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    void AddRange(IEnumerable<T> items);

    /// <summary>
    /// Adds a range of items to the end of the list.
    /// </summary>
    /// <param name="items">The items to be added.</param>
    void AddRange(ReadOnlySpan<T> items);

    /// <summary>
    /// Inserts a range of items into the list.
    /// </summary>
    /// <param name="index">The index at which the first item in the range should be inserted.</param>
    /// <param name="items">The items to be inserted.</param>
    /// <exception cref="IndexOutOfRangeException">Throws when <paramref name="index"/> does not represent a valid index of an item in the list, or the next available index of the list.</exception>
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
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
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
    /// Removes a range of consecutive items from the list.
    /// </summary>
    /// <param name="index">The index of the first item to be removed.</param>
    /// <param name="count">The number of items to be removed.</param>
    /// <exception cref="IndexOutOfRangeException">Throws when <paramref name="index"/> does not represent a valid index of an item within the list.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Throws when <paramref name="count"/> and <paramref name="index"/> define a range that extends beyond the end of the list.</exception>
    void RemoveRange(
        int index,
        int count);
}
