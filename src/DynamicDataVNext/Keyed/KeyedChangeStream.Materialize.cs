namespace DynamicDataVNext;

public static partial class KeyedChangeStream
{
    /// <summary>
    /// Materializes a virtual collection, described by a given stream of changes, into a physical collection that can be queried synchronously.
    /// </summary>
    /// <param name="stream">The change stream to be materialized.</param>
    /// <typeparam name="TKey">The type of the item keys in the collection.</typeparam>
    /// <typeparam name="TItem">The type of the item values in the collection.</typeparam>
    /// <returns>A <see cref="ReactiveDictionary{TKey, TValue}"/> that will reflect and re-publish every change published by <paramref name="stream"/>.</returns>
    /// <remarks>
    /// This operator can also serve as a source for multicasting a change stream to multiple subscribers, as it allows upstream operations to be de-duplicated.
    /// </remarks>
    public static ReactiveDictionary<TKey, TItem> Materialize<TKey, TItem>(this KeyedChangeStream<TKey, TItem> stream)
            where TKey : notnull
        => new(
            source:     stream.Source,
            comparer:   stream.KeyComparer,
            options:    stream.Options);
}
