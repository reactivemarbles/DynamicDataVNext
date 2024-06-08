using System;
using System.Collections.Generic;

namespace DynamicDataVNext;

/// <summary>
/// Describes an extended version of <see cref="ISet{T}"/>, supporting range operations.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public interface IExtendedSet<T>
    : ISet<T>
{
    /// <inheritdoc cref="ISet{T}.ExceptWith(IEnumerable{T})"/>.
    void ExceptWith(ReadOnlySpan<T> other);

    /// <inheritdoc cref="ISet{T}.IntersectWith(IEnumerable{T})"/>.
    void IntersectWith(ReadOnlySpan<T> other);

    /// <summary>
    /// Resets the items within the collection to the given set of items. I.E. clears and then re-populates the collection, as a single operation.
    /// </summary>
    /// <param name="items">The set of items with which to populate the collection.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    void Reset(IEnumerable<T> items);

    /// <summary>
    /// Resets the items within the collection to the given set of items. I.E. clears and then re-populates the collection, as a single operation.
    /// </summary>
    /// <param name="items">The set of items with which to populate the collection.</param>
    void Reset(ReadOnlySpan<T> items);

    /// <inheritdoc cref="ISet{T}.SymmetricExceptWith(IEnumerable{T})"/>.
    void SymmetricExceptWith(ReadOnlySpan<T> other);

    /// <inheritdoc cref="ISet{T}.UnionWith(IEnumerable{T})"/>.
    void UnionWith(ReadOnlySpan<T> other);
}
