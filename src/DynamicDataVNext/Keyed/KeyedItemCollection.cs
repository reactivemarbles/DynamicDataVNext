namespace DynamicDataVNext;

/// <summary>
/// A read-only, zero-allocation wrapper for a <see cref="Dictionary{TKey, TValue}"/>, allowing zero-allocation iteration of its items, as <see cref="KeyedItem{TKey, TItem}"/> values. 
/// </summary>
/// <typeparam name="TKey">The type of the item keys in the collection.</typeparam>
/// <typeparam name="TItem">The type of the item values in the collection.</typeparam>
public readonly partial struct KeyedItemCollection<TKey, TItem>
        : IReadOnlyCollection<KeyedItem<TKey, TItem>>
    where TKey : notnull
{
    internal KeyedItemCollection(Dictionary<TKey, TItem> itemsByKey)
        => _itemsByKey = itemsByKey;

    /// <inheritdoc/>
    public int Count
        => _itemsByKey.Count;

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator()"/>
    public Enumerator GetEnumerator()
        => new(_itemsByKey.GetEnumerator());

    IEnumerator<KeyedItem<TKey, TItem>> IEnumerable<KeyedItem<TKey, TItem>>.GetEnumerator()
        => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    private readonly Dictionary<TKey, TItem> _itemsByKey;
}
