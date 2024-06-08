using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of distinctly-keyed items, which does not allow public mutation.
/// </summary>
/// <typeparam name="TKey">The type of the key values of items in the collection.</typeparam>
/// <typeparam name="TItem">The type of the items in the collection.</typeparam>
public interface IReadOnlyCache<TKey, TItem>
    : IReadOnlyCollection<TItem>
{
    /// <inheritdoc cref="ICache{TKey, TItem}.this[TKey]"/>
    TItem this[TKey key] { get; }

    /// <inheritdoc cref="ICache{TKey, TItem}.Keys"/>
    IReadOnlyCollection<TKey> Keys { get; }

    /// <inheritdoc cref="ICache{TKey, TItem}.ContainsKey(TKey)"/>
    bool ContainsKey(TKey key);

    /// <inheritdoc cref="ICache{TKey, TItem}.TryGetItem(TKey, out TItem)"/>
    bool TryGetItem(TKey key, [MaybeNullWhen(false)] out TItem item);
}
