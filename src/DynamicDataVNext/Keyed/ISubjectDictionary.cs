using System;
using System.Collections.Generic;
using System.Reactive;

namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of items, with distinct keys, which publishes notifications about its mutations to subscribers, as they occur.
/// </summary>
/// <typeparam name="TKey">The type of the item keys in the collection.</typeparam>
/// <typeparam name="TValue">The type of the item values in the collection.</typeparam>
public interface ISubjectDictionary<TKey, TValue>
    : IExtendedDictionary<TKey, TValue>,
        IObservable<KeyedChangeSet<TKey, TValue>>
{
    /// <summary>
    /// An event that occurs after any mutation of the collection occurs.
    /// </summary>
    IObservable<Unit> CollectionChanged { get; }

    /// <summary>
    /// Allows subscribers to observe the value of a particular keyed item within the collection, as it changes.
    /// </summary>
    /// <param name="key">The key whose value is to be observed.</param>
    /// <returns>A stream which will publish the latest value, for the given key, within the collection.</returns>
    /// <remarks>
    /// The returned stream will always immediately publish the current value for the given key, upon subscription, and will complete if the given key is removed. If the key is not present within the collection upon subscription, the stream will complete immediately.
    /// </remarks>
    IObservable<TValue> ObserveValue(TKey key);

    /// <summary>
    /// Temporarily suspends the publication of notifications by the collection, until the returned object is disposed, at which point all mutations made during the suspension will (if any) will be published as one notification.
    /// </summary>
    /// <returns>An object that will trigger the resumption of notifications, when disposed.</returns>
    /// <remarks>
    /// May be called multiple times, in which case no notifications will be published until all outstanding suspensions have been disposed.
    /// </remarks>
    IDisposable SuspendNotifications();
}
