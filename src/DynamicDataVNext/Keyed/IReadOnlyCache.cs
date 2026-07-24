using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DynamicDataVNext;

public interface IReadOnlyCache<TKey, TItem>
    : IReadOnlyCollection<TItem>
{
    /// <summary>
    /// Retrieves the item in the collection (if any) for the given key.
    /// </summary>
    /// <param name="key">The key of the item to be retrieved.</param>
    /// <exception cref="KeyNotFoundException">Throws when no item matching <paramref name="key"/> exists within the collection.</exception>
    TItem this[TKey key] { get; }
    
    /// <inheritdoc cref="ICache{TKey, TItem}.Keys"/>
    IReadOnlyCollection<TKey> Keys { get; }

    /// <inheritdoc cref="ICache{TKey, TItem}.KeySelector"/>
    Func<TItem, TKey> KeySelector { get; }

    /// <inheritdoc cref="ICollection{TItem}.Contains(TItem)"/>
    bool Contains(TItem item);

    /// <inheritdoc cref="ICache{TKey, TItem}.ContainsKey(TKey)"/>
    bool ContainsKey(TKey key);

    /// <inheritdoc cref="ICache{TKey, TItem}.TryGetItem(TKey, out TItem)"/>
    bool TryGetItem(                TKey    key,
        [MaybeNullWhen(false)]  out TItem   item);
}
