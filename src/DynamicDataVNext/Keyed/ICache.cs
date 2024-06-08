using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of distinctly-keyed items.
/// </summary>
/// <typeparam name="TKey">The type of the key values of items in the collection.</typeparam>
/// <typeparam name="TItem">The type of the items in the collection.</typeparam>
public interface ICache<TKey, TItem>
    : ICollection<TItem>
{
    /// <summary>
    /// Retrieves the item in the collection for the given key.
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
    /// Applies an item to the collection, as a single operation, by either adding or replacing the item, based on its key value.
    /// </summary>
    /// <param name="item">The item to be applied to the collection.</param>
    void AddOrReplace(TItem item);

    /// <summary>
    /// Applies a range of items to the collection, as a single operation, by either adding or replacing each item, based on its key value.
    /// </summary>
    /// <param name="items">The items to applied to the collection.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    void AddOrReplaceRange(IEnumerable<TItem> items);

    /// <summary>
    /// Applies a range of items to the collection, as a single operation, by either adding or replacing each item, based on its key value.
    /// </summary>
    /// <param name="items">The items to applied to the collection.</param>
    void AddOrReplaceRange(ReadOnlySpan<TItem> items);

    /// <summary>
    /// Checks whether a given key is currently present, within the collection.
    /// </summary>
    /// <param name="key">The key to check for.</param>
    /// <returns>A flag indicating whether the given key is present in the collection, or not.</returns>
    bool ContainsKey(TKey key);

    /// <summary>
    /// Removes an item from the collection, by its key.
    /// </summary>
    /// <param name="key">The key to be removed.</param>
    /// <returns>A flag indicating whether the given key was present in the collection, and thus removed successfully, or not.</returns>
    bool Remove(TKey key);

    /// <summary>
    /// Removes a set of items, from the collection, as a single operation.
    /// </summary>
    /// <param name="items">The set of items to be removed.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    /// <remarks>
    /// Note that the collection will silently ignore items that are not present in the collection.
    /// </remarks>
    void RemoveRange(IEnumerable<TItem> items);

    /// <summary>
    /// Removes a set of items, from the collection, as a single operation.
    /// </summary>
    /// <param name="items">The set of items to be removed.</param>
    /// <remarks>
    /// Note that the collection will silently ignore items that are not present in the collection.
    /// </remarks>
    void RemoveRange(ReadOnlySpan<TItem> items);

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
    /// Removes a set of items, by key, from the collection, as a single operation.
    /// </summary>
    /// <param name="keys">The set of keys to be removed.</param>
    /// <remarks>
    /// Note that the collection will silently ignore keys that are not present in the collection.
    /// </remarks>
    void RemoveRange(ReadOnlySpan<TKey> keys);

    /// <summary>
    /// Resets the items within the collection to the given set of items. I.E. clears and then re-populates the collection, as a single operation.
    /// </summary>
    /// <param name="items">The set of items with which to populate the collection.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    void Reset(IEnumerable<TItem> items);

    /// <summary>
    /// Resets the items within the collection to the given set of items. I.E. clears and then re-populates the collection, as a single operation.
    /// </summary>
    /// <param name="items">The set of items with which to populate the collection.</param>
    void Reset(ReadOnlySpan<TItem> items);

    /// <summary>
    /// Attempts to retrieve an item from the collection, by its key.
    /// </summary>
    /// <param name="key">The key whose item is to be retrieved.</param>
    /// <param name="item">The item in the collection whose key is <paramref name="key"/>. The default value of <typeparamref name="TItem"/> is assigned, if no such item is present.</param>
    /// <returns>A flag indicating whether an item with the given key was successfully retrieved, or not.</returns>
    bool TryGetItem(TKey key, [MaybeNullWhen(false)] out TItem item);
}
