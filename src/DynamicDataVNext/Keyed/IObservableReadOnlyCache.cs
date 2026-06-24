using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of keyed items, which may not be mutated by the consumer, and which publishes notifications about its mutations, as they occur.
/// </summary>
/// <typeparam name="TKey">The type of the key values in the collection.</typeparam>
/// <typeparam name="TItem">The type of the items in the collection.</typeparam>
public interface IObservableReadOnlyCache<TKey, TItem>
    : IObservableReadOnlyCollection<TItem>,
        IObservable<KeyedChange<TKey, TItem>>
{
    /// <summary>
    /// retrieves the item in the collection (if any) for the given key.
    /// </summary>
    /// <param name="key">The key of the item to be retrieved.</param>
    /// <exception cref="KeyNotFoundException">Throws when <paramref name="key"/> does not exist within the collection.</exception>
    /// <returns>The item in the collection with the given key.</returns>
    TItem this[TKey key] { get; }
    
    /// <inheritdoc cref="IObservableCache{TKey, TItem}.ChangeStream"/>
    KeyedChangeStream<TKey, TItem> ChangeStream { get; }

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
    /// Checks whether a given key is currently present, within the collection.
    /// </summary>
    /// <param name="key">The key to check for.</param>
    /// <returns>A flag indicating whether the given key is present in the collection, or not.</returns>
    bool ContainsKey(TKey key);

    /// <summary>
    /// Attempts to retrieve an item from the collection, by its key.
    /// </summary>
    /// <param name="key">The key whose item is to be retrieved.</param>
    /// <param name="item">The item in the collection whose key is <paramref name="key"/>. The default value of <typeparamref name="TItem"/> is assigned, if no such item is present.</param>
    /// <returns>A flag indicating whether an item with the given key was successfully retrieved, or not.</returns>
    bool TryGetItem(TKey key, [MaybeNullWhen(false)] out TItem item);
}
