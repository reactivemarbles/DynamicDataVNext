using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DynamicDataVNext;

/// <summary>
/// Provides access to a collection of items, within a <see cref="DistinctChangeSet{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public readonly partial struct DistinctChangeItemCollection<T>
    : IReadOnlyCollection<T>
{
    /// <inheritdoc/>
    public int Count
        => LastIndex - FirstIndex + 1;

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator()"/>
    public Enumerator GetEnumerator()
        => new(this);
    
    internal ImmutableArray<DistinctChange<T>> Changes { get; init; }
    
    internal int FirstIndex { get; init; }
    
    internal int LastIndex { get; init; }
    
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
        => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
