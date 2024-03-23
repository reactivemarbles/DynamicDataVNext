using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reactive;

namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of distinctly-keyed items, which publishes notifications about its mutations to subscribers, as they occur.
/// </summary>
/// <typeparam name="TKey">The type of the key values of items in the collection.</typeparam>
/// <typeparam name="TItem">The type of the items in the collection.</typeparam>
public interface IObservableCache<TKey, TItem>
    : IReadOnlyCollection<TItem>,
        IObservable<KeyedChange<TKey, TItem>>
{
    /// <summary>
    /// retrieves the item in the collection (if any) for the given key.
    /// </summary>
    /// <param name="key">The key of the item to be retrieved.</param>
    /// <exception cref="KeyNotFoundException">Throws when <paramref name="key"/> does not exist within the collection.</exception>
    /// <returns>The item in the collection with the given key.</returns>
    TItem this[TKey key] { get; }
    
    /// <summary>
    /// Retrieves the current set of keys present within the collection.
    /// </summary>
    /// <remarks>
    /// Note that the returned collection represents a "snapshot" of the source collection, at the time at which it is created. Changes made to the source collection after a key collection is retrieved are not reflected upon the key collection.
    /// </remarks>
    IReadOnlyCollection<TKey> Keys { get; }

    /// <summary>
    /// Retrieves the current set of items present within the collection.
    /// </summary>
    /// <remarks>
    /// Note that the returned collection represents a "snapshot" of the source collection, at the time at which it is created. Changes made to the source collection after an item collection is retrieved are not reflected upon the item collection.
    /// </remarks>
    IReadOnlyCollection<TItem> Items { get; }

    /// <summary>
    /// An event that occurs after any mutation of the collection occurs.
    /// </summary>
    IObservable<Unit> CollectionChanged { get; }

    /// <summary>
    /// Checks whether a given key is currently present, within the collection.
    /// </summary>
    /// <param name="key">The key to check for.</param>
    /// <returns>A flag indicating whether the given key is present in the collection, or not.</returns>
    bool ContainsKey(TKey key);

    /// <summary>
    /// Allows subscribers to observe the value of a particular keyed item within the collection, as it changes.
    /// </summary>
    /// <param name="key">The key whose value is to be observed.</param>
    /// <returns>A stream which will publish the latest value, for the given key, within the collection.</returns>
    /// <remarks>
    /// The returned stream will always immediately publish the current value for the given key, upon subscription, and will complete if the given key is removed. If the key is not present within the collection upon subscription, the stream will complete immediately.
    /// </remarks>
    IObservable<TItem> ObserveValue(TKey key);

    /// <summary>
    /// Attempts to retrieve an item from the collection, by its key.
    /// </summary>
    /// <param name="key">The key whose item is to be retrieved.</param>
    /// <param name="item">The item in the collection whose key is <paramref name="key"/>. The default value of <typeparamref name="TItem"/> is assigned, if no such item is present.</param>
    /// <returns>A flag indicating whether an item with the given key was successfully retrieved, or not.</returns>
    bool TryGetItem(TKey key, [MaybeNullWhen(false)] out TItem item);

    /// <summary>
    /// Temporarily suspends the publication of notifications by the collection, until the returned object is disposed, at which point all mutations made during the suspension will (if any) will be published as one notification.
    /// </summary>
    /// <returns>An object that will trigger the resumption of notifications, when disposed.</returns>
    /// <remarks>
    /// May be called multiple times, in which case no notifications will be published until all outstanding suspensions have been disposed.
    /// </remarks>
    IDisposable SuspendNotifications();
}
