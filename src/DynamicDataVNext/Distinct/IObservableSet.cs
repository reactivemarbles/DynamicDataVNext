using System;
using System.Collections.Generic;

namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of distinct items, which publishes notifications about its mutations to subscribers, as they occur.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public interface IObservableSet<T>
    : IObservableCollection<T>,
        ISet<T>
{
    /// <summary>
    /// The stream of changes describing mutations made to the collection.
    /// </summary>
    DistinctChangeStream<T> ChangeStream { get; }

    /// <summary>
    /// Signals that the given item within the collection has, itself, mutated, triggering a <see cref="DistinctChangeType.Refreshment"/> notification to be published via <see cref="ChangeStream"/>.
    /// </summary>
    /// <param name="item">The item that was refreshed.</param>
    /// <returns><see langword="false"/> if the collection does not actually contain <paramref name="item"/>. Otherwise, <see langword="true"/>.</returns>
    bool Refresh(T item);
    
    /// <inheritdoc cref="Reset(ReadOnlySpan{T})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    void Reset(IEnumerable<T> items);

    /// <summary>
    /// Performs a <see cref="ChangeSetType.Reset"/> operation upon the collection, by removing any existing items within the collection, and replacing them with the given items. 
    /// </summary>
    /// <param name="items">The new set of items to be loaded into the collection.</param>
    /// <remarks>
    /// Any duplicate items within <paramref name="items"/> are automatically ignored.
    /// </remarks>
    void Reset(ReadOnlySpan<T> items);
}
