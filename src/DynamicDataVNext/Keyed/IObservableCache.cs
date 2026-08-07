using System;
using System.Collections.Generic;

namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of keyed items, and which publishes notifications about mutations made to itself or its items.
/// </summary>
/// <typeparam name="TKey">The type of the key values in the collection.</typeparam>
/// <typeparam name="TItem">The type of the items in the collection.</typeparam>
public interface IObservableCache<TKey, TItem>
    : IObservableCollection<TItem>,
        ICache<TKey, TItem>
{
    /// <summary>
    /// The stream of changes describing mutations made to the collection.
    /// </summary>
    KeyedChangeStream<TKey, TItem> ChangeStream { get; }

    /// <summary>
    /// Adds a range of items to the collection, as a single operation.
    /// </summary>
    /// <param name="items">The items to be added.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    /// <exception cref="ArgumentException">Throws if <paramref name="items"/> contains any items whose key value is duplicated or already in the collection.</exception>
    void AddRange(IEnumerable<TItem> items);

    /// <summary>
    /// Merges a range of items into the collection by either adding or replacing each one, based on whether an item with the same key is already present.
    /// </summary>
    /// <param name="items">The items to be added or replaced</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    void MergeRange(IEnumerable<TItem> items);

    /// <summary>
    /// Signals that an item within the collection has, itself, mutated, triggering a <see cref="KeyedChangeType.Refreshment"/> notification to be published via <see cref="ChangeStream"/>.
    /// </summary>
    /// <param name="item">The item that was mutated.</param>
    /// <returns><see langword="false"/> if the collection does not actually contain <paramref name="item"/>. Otherwise, <see langword="true"/>.</returns>
    bool Refresh(TItem item);

    /// <summary>
    /// Signals that an item within the collection has, itself, mutated, triggering a <see cref="KeyedChangeType.Refreshment"/> notification to be published via <see cref="ChangeStream"/>.
    /// </summary>
    /// <param name="key">The key value of the item that was mutated.</param>
    /// <returns><see langword="false"/> if the collection does not actually contain <paramref name="key"/>. Otherwise, <see langword="true"/>.</returns>
    bool RefreshKey(TKey key);

    /// <summary>
    /// Removes a set of items from the collection, as a single operation.
    /// </summary>
    /// <param name="items">The set of items to be removed.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    /// <remarks>
    /// Note that the collection will silently ignore items that are not present in the collection.
    /// </remarks>
    void RemoveRange(IEnumerable<TItem> items);

    /// <summary>
    /// Performs a <see cref="ChangeSetType.Reset"/> operation upon the collection, by removing any existing items within the collection, and replacing them with the given items. 
    /// </summary>
    /// <param name="items">The new set of items to be loaded into the collection.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    void Reset(IEnumerable<TItem> items);
}
