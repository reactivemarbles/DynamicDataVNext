namespace DynamicDataVNext;

/// <summary>
/// A set of options describing the nature of items, within <see cref="DistinctChangeSet{T}"/>s.
/// </summary>
/// <remarks>
/// <para>Making this metadata available to downstream consumers changesets, allows them to automatically make optimizations about how they process said changesets.</para>
/// <para>For example, filtering can be performed without having to maintain an internal copy of the source collection, when items are immutable.</para> 
/// </remarks>
public readonly record struct DistinctItemOptions
{
    /// <summary>
    /// A flag indicating whether the items are mutable.
    /// </summary>
    /// <remarks>
    /// Generally, immutable items provide for more optimizations and better performance.
    /// </remarks>
    public bool ItemsAreMutable { get; init; }
}
