using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DynamicDataVNext;

/// <summary>
/// Provides access to a collection of items, within a <see cref="KeyedChangeSet{TKey, TItem}"/>.
/// </summary>
/// <typeparam name="TKey">The type of the keys of items in the collection.</typeparam>
/// <typeparam name="TItem">The type of the items in the collection.</typeparam>
public readonly partial struct KeyedChangeItemCollection<TKey, TItem>
    : IReadOnlyCollection<KeyedItem<TKey, TItem>>
{
    /// <inheritdoc/>
    public int Count
        => LastIndex - FirstIndex + 1;

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator()"/>
    public Enumerator GetEnumerator()
        => new(this);
    
    internal ImmutableArray<KeyedChange<TKey, TItem>> Changes { get; init; }
    
    internal KeyedChangeType ChangesType { get; init; } 

    internal int FirstIndex { get; init; }
    
    internal int LastIndex { get; init; }
    
    IEnumerator<KeyedItem<TKey, TItem>> IEnumerable<KeyedItem<TKey, TItem>>.GetEnumerator()
        => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
