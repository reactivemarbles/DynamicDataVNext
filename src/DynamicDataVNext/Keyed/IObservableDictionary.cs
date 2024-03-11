using System;
using System.Collections.Generic;
using System.Reactive;

namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of items, with distinct keys, which publishes notifications about its mutations to subscribers, as they occur.
/// </summary>
/// <typeparam name="TKey">The type of the item keys in the collection.</typeparam>
/// <typeparam name="TValue">The type of the item values in the collection.</typeparam>
public interface IObservableDictionary<TKey, TValue>
    : IDictionary<TKey, TValue>,
        IObservable<KeyedChange<TKey, TValue>>
{
    /// <summary>
    /// An event that occurs after any mutation of the collection occurs.
    /// </summary>
    IObservable<Unit> CollectionChanged { get; }

    /// <inheritdoc cref="IDictionary{TKey, TValue}.Keys"/>
    new IReadOnlyCollection<TKey> Keys { get; }

    /// <inheritdoc cref="IDictionary{TKey, TValue}.Values"/>
    new IReadOnlyCollection<TValue> Values { get; }

    /// <summary>
    /// Adds a range of items to the collection, as a single operation.
    /// </summary>
    /// <param name="values">The values to use as <see cref="KeyValuePair{TKey, TValue}.Value"/> for each new item.</param>
    /// <param name="keySelector">A selector to select a <see cref="KeyValuePair{TKey, TValue}.Key"/> value for each new item.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="values"/> and <paramref name="keySelector"/>.</exception>
    /// <exception cref="ArgumentException">Throws if <paramref name="keySelector"/> returns a key that already exists within the collection.</exception>
    void AddRange(
        IEnumerable<TValue> values,
        Func<TValue, TKey>  keySelector);

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
    /// Removes a set of items, by key, from the collection, as a single operation.
    /// </summary>
    /// <param name="keys">The set of keys to be removed.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="keys"/>.</exception>
    /// <remarks>
    /// Note that the collection will silently ignore keys that are not present in the collection.
    /// </remarks>
    void RemoveRange(IEnumerable<TKey> keys);

    /// <summary>
    /// Resets the items within the collection to the given set of items. I.E. clears and then re-populates the collection, as a single operation.
    /// </summary>
    /// <param name="values">The set of items with which to populate the collection.</param>
    /// <param name="keySelector">A selector to select a <see cref="KeyValuePair{TKey, TValue}.Key"/> value for each new item.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="values"/> and <paramref name="keySelector"/>.</exception>
    void Reset(
        IEnumerable<TValue> values,
        Func<TValue, TKey>  keySelector);

    /// <summary>
    /// Temporarily suspends the publication of notifications by the collection, until the returned object is disposed, at which point all mutations made during the suspension will (if any) will be published as one notification.
    /// </summary>
    /// <returns>An object that will trigger the resumption of notifications, when disposed.</returns>
    /// <remarks>
    /// May be called multiple times, in which case no notifications will be published until all outstanding suspensions have been disposed.
    /// </remarks>
    IDisposable SuspendNotifications();
}
