using System;
using System.Collections.Generic;
using System.Reactive;

namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of items of unspecified nature, that publishes notifications about changes in its state. 
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public interface IObservableCollection<T>
    : ICollection<T>
{
    /// <summary>
    /// An event that occurs after any mutation of the collection occurs.
    /// </summary>
    IObservable<Unit> CollectionChanged { get; }
 
    /// <summary>
    /// Temporarily pauses the publication of notifications, allowing change notifications to be buffered and published in batches.
    /// </summary>
    /// <returns>An object that, when disposed, will end the suspension and immediately trigger the publication of buffered notifications.</returns>
    /// <remarks>
    /// May be called multiple times, in which case no notifications will be published until all outstanding suspensions have been disposed.
    /// </remarks>
    IDisposable SuspendNotifications();
}
