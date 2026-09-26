namespace DynamicDataVNext;

/// <summary>
/// A read-only, zero-allocation wrapper for a <see cref="Dictionary{TKey, TValue}"/>, allowing zero-allocation iteration of its items. 
/// </summary>
/// <typeparam name="TKey">The type of the item keys in the collection.</typeparam>
/// <typeparam name="TValue">The type of the item values in the collection.</typeparam>
public readonly struct KeyValuePairCollection<TKey, TValue>
        : IReadOnlyCollection<KeyValuePair<TKey, TValue>>
    where TKey : notnull
{
    internal KeyValuePairCollection(Dictionary<TKey, TValue> items)
        => _items = items;

    /// <inheritdoc/>
    public int Count
        => _items.Count;

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator()"/>
    public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        => _items.GetEnumerator();

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    private readonly Dictionary<TKey, TValue> _items;
}
