using System;
using System.Collections.Generic;
using System.Reactive;

namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of distinct items, which publishes notifications about its mutations to subscribers, as they occur.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public interface ISubjectSet<T>
    : IExtendedSet<T>,
        IObservable<DistinctChangeSet<T>>
{
    /// <summary>
    /// An event that occurs after any mutation of the collection occurs.
    /// </summary>
    IObservable<Unit> CollectionChanged { get; }

    /// <summary>
    /// Temporarily suspends the publication of notifications by the collection, until the returned object is disposed, at which point all mutations made during the suspension will (if any) will be published as one notification.
    /// </summary>
    /// <returns>An object that will trigger the resumption of notifications, when disposed.</returns>
    /// <remarks>
    /// May be called multiple times, in which case no notifications will be published until all outstanding suspensions have been disposed.
    /// </remarks>
    IDisposable SuspendNotifications();
}
