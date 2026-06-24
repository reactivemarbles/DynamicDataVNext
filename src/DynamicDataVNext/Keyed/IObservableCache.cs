using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reactive;

namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of keyed items, which publishes notifications about its mutations, as they occur.
/// </summary>
/// <typeparam name="TKey">The type of the key values in the collection.</typeparam>
/// <typeparam name="TItem">The type of the items in the collection.</typeparam>
public interface IObservableCache<TKey, TItem>
    : ICollection<TItem>,
        IObservable<KeyedChange<TKey, TItem>>
{
    /// <summary>
    /// Accesses the item in the collection (if any) for the given key.
    /// </summary>
    /// <param name="key">The key of the item to be accessed.</param>
    /// <exception cref="KeyNotFoundException">Throws when <paramref name="key"/> does not exist within the collection, during a retrieval.</exception>
    /// <returns>The item in the collection with the given key.</returns>
    /// <remarks>
    /// When assigning a value to a given key, the key need not be already-present within the collection.
    /// </remarks>
    TItem this[TKey key] { get; set; }
    
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

    /// <inheritdoc cref="AddRange(ReadOnlySpan{TItem})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    void AddRange(IEnumerable<TItem> items);

    /// <summary>
    /// Adds a range of items to the collection, as a single operation.
    /// </summary>
    /// <param name="items">The items to be added.</param>
    /// <exception cref="ArgumentException">Throws if <paramref name="items"/> contains an item whose key is already present within the collection.</exception>
    void AddRange(ReadOnlySpan<TItem> items);

    /// <inheritdoc cref="RemoveRange(ReadOnlySpan{TItem})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    void RemoveRange(IEnumerable<TItem> items);

    /// <summary>
    /// Removes a set of items from the collection, as a single operation.
    /// </summary>
    /// <param name="items">The set of items to be removed.</param>
    /// <remarks>
    /// Note that the collection will silently ignore items that are not present in the collection.
    /// </remarks>
    void RemoveRange(ReadOnlySpan<TItem> items);

    /// <summary>
    /// Attempts to retrieve an item from the collection, by its key.
    /// </summary>
    /// <param name="key">The key whose item is to be retrieved.</param>
    /// <param name="item">The item in the collection whose key is <paramref name="key"/>. The default value of <typeparamref name="TItem"/> is assigned, if no such item is present.</param>
    /// <returns>A flag indicating whether an item with the given key was successfully retrieved, or not.</returns>
    bool TryGetItem(TKey key, [MaybeNullWhen(false)] out TItem item);

    /// <summary>
    /// Signals that an item within the collection has, itself, mutated, triggering a <see cref="KeyedChangeType.Refreshment"/> notification to be published via <see cref="ChangeStream"/>.
    /// </summary>
    /// <param name="item">The item that was refreshed.</param>
    /// <returns><see langword="false"/> if the collection does not actually contain <paramref name="item"/>. Otherwise, <see langword="true"/>.</returns>
    bool Refresh(TItem item);

    /// <inheritdoc cref="Reset(ReadOnlySpan{TItem})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    void Reset(IEnumerable<TItem> items);

    /// <summary>
    /// Performs a <see cref="ChangeSetType.Reset"/> operation upon the collection, by removing any existing items within the collection, and replacing them with the given items. 
    /// </summary>
    /// <param name="items">The new set of items to be loaded into the collection.</param>
    void Reset(ReadOnlySpan<TItem> items);
}
