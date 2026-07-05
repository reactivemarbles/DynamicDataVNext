using System;

namespace DynamicDataVNext;

public static partial class DistinctChangeStream
{
    /// <summary>
    /// Materializes a virtual collection, described by a given stream of changes, into a physical collection that can be queried synchronously.
    /// </summary>
    /// <param name="source">The change stream to be materialized.</param>
    /// <typeparam name="T">The type of items in the stream.</typeparam>
    /// <returns>A <see cref="ReactiveHashSet{T}"/> that will reflect and re-publish every change published by <paramref name="source"/>.</returns>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="source"/>.</exception>
    /// <remarks>
    /// This operator can also serve as a source for multicasting a change stream to multiple subscribers, as it allows upstream operations to be de-duplicated.
    /// </remarks>
    public static ReactiveHashSet<T> Materialize<T>(this DistinctChangeStream<T> source)
        => new(
            source:     source.Source,
            comparer:   source.Comparer,
            options:    source.Options);
}
