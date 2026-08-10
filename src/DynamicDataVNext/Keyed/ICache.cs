namespace DynamicDataVNext;

public interface ICache<TKey, TItem>
    : ICollection<TItem>
{
    /// <inheritdoc cref="IReadOnlyCache{TKey, TItem}.this[TKey]"/>
    TItem this[TKey key] { get; }
    
    /// <summary>
    /// Retrieves the current set of keys present within the collection.
    /// </summary>
    /// <remarks>
    /// Note that the returned collection represents a "snapshot" of the source collection, at the time at which it is created. Changes made to the source collection after a key collection is retrieved are not reflected upon the key collection.
    /// </remarks>
    IReadOnlyCollection<TKey> Keys { get; }

    /// <summary>
    /// A delegate that defines, and allows retrieval of, the key for each item in the collection.
    /// </summary>
    Func<TItem, TKey> KeySelector { get; }

    /// <summary>
    /// Checks whether the cache contains an item with the given key.
    /// </summary>
    /// <param name="key">The key to check for.</param>
    /// <returns><see langword="false"/> if the collection does not actually contain <paramref name="key"/>. Otherwise, <see langword="true"/>.</returns>
    bool ContainsKey(TKey key);

    /// <summary>
    /// Merges an item into the cache by either adding it, or replacing it, if an item with the same key is already present.
    /// </summary>
    /// <param name="item">The item to be added or replaced</param>
    void Merge(TItem item);

    /// <summary>
    /// Attempts to remove an item from the collection, by its key.
    /// </summary>
    /// <param name="key">The key value of the item to be removed.</param>
    /// <param name="item">The item, if any, that was removed.</param>
    /// <returns><see langword="false"/> if the collection does not actually contain <paramref name="key"/>, and no item was removed. Otherwise, <see langword="true"/>.</returns>
    bool Remove(                    TKey    key,
        [MaybeNullWhen(false)]  out TItem   item);

    /// <summary>
    /// Attempts to retrieve an item from the collection, by its key.
    /// </summary>
    /// <param name="key">The key whose item is to be retrieved.</param>
    /// <param name="item">The item in the collection whose key is <paramref name="key"/>. The default value of <typeparamref name="TItem"/> is assigned, if no such item is present.</param>
    /// <returns>A flag indicating whether an item with the given key was successfully retrieved, or not.</returns>
    bool TryGetItem(                TKey    key,
        [MaybeNullWhen(false)]  out TItem   item);
}
