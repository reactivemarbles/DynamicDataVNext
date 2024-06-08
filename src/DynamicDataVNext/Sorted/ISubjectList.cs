using System;
using System.Collections.Generic;
using System.Reactive;

namespace DynamicDataVNext;

/// <summary>
/// Describes a sorted collection of items, which publishes notifications about its mutations to subscribers, as they occur.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public interface ISubjectList<T>
    : IExtendedList<T>,
        IObservable<SortedChangeSet<T>>
{
    /// <summary>
    /// An event that occurs after any mutation of the collection occurs.
    /// </summary>
    IObservable<Unit> CollectionChanged { get; }

    /// <summary>
    /// Allows subscribers to observe the item in the collection, at a particular index, as it changes.
    /// </summary>
    /// <param name="index">The index whose item is to be observed.</param>
    /// <returns>A stream which will publish the latest item, for the given index, within the collection.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if <paramref name="index"/> is negative.</exception>
    /// <remarks>
    /// The returned stream will always immediately publish the current item for the given index, upon subscription, and will complete if the given index is removed. If the index is not present within the collection upon subscription, the stream will complete immediately.
    /// </remarks>
    IObservable<T> ObserveValue(int index);

    /// <summary>
    /// Temporarily suspends the publication of notifications by the collection, until the returned object is disposed, at which point all mutations made during the suspension will (if any) will be published as one notification.
    /// </summary>
    /// <returns>An object that will trigger the resumption of notifications, when disposed.</returns>
    /// <remarks>
    /// May be called multiple times, in which case no notifications will be published until all outstanding suspensions have been disposed.
    /// </remarks>
    IDisposable SuspendNotifications();
}
