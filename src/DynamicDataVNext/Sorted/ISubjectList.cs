using System;
using System.Collections.Generic;
using System.Reactive;

namespace DynamicDataVNext;

/// <summary>
/// Describes a sorted collection of items, which publishes notifications about its mutations to subscribers, as they occur.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public interface ISubjectList<T>
    : IList<T>,
        IObservable<SortedChangeSet<T>>
{
    /// <summary>
    /// An event that occurs after any mutation of the collection occurs.
    /// </summary>
    IObservable<Unit> CollectionChanged { get; }

    /// <summary>
    /// Adds a range of items to the end of the list.
    /// </summary>
    /// <param name="items">The items to be added.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    void AddRange(IEnumerable<T> items);

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
    /// Moves an item within the list.
    /// </summary>
    /// <param name="oldIndex">The index of the item to be moved, before the operation.</param>
    /// <param name="newIndex">The desired index of the item to be moved, after the operation.</param>
    /// <exception cref="IndexOutOfRangeException">Throws when <paramref name="oldIndex"/> or <paramref name="newIndex"/> does not represent a valid index of an item in the list.</exception>
    void Move(
        int oldIndex,
        int newIndex);

    /// <summary>
    /// Allows subscribers to observe the item in the collection, at a particular index, as it changes.
    /// </summary>
    /// <param name="index">The index whose item is to be observed.</param>
    /// <returns>A stream which will publish the latest item, for the given index, within the collection.</returns>
    /// <remarks>
    /// The returned stream will always immediately publish the current item for the given index, upon subscription, and will complete if the given index is removed. If the index is not present within the collection upon subscription, the stream will complete immediately.
    /// </remarks>
    IObservable<T> ObserveValue(int index);

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

    /// <summary>
    /// Temporarily suspends the publication of notifications by the collection, until the returned object is disposed, at which point all mutations made during the suspension will (if any) will be published as one notification.
    /// </summary>
    /// <returns>An object that will trigger the resumption of notifications, when disposed.</returns>
    /// <remarks>
    /// May be called multiple times, in which case no notifications will be published until all outstanding suspensions have been disposed.
    /// </remarks>
    IDisposable SuspendNotifications();
}
