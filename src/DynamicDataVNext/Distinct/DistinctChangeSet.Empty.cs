namespace DynamicDataVNext;

public static partial class DistinctChangeSet
{
    /// <summary>
    /// Retrieve a copy of the empty changeset.
    /// </summary>
    /// <typeparam name="T">The type of the items in the source collection.</typeparam>
    /// <returns>A copy of the empty changeset.</returns>
    public static DistinctChangeSet<T> Empty<T>()
        => DistinctChangeSet<T>.Empty;
}

public readonly partial record struct DistinctChangeSet<T>
{
    /// <summary>
    /// A copy of the empty changeset.
    /// </summary>
    public static readonly DistinctChangeSet<T> Empty
        = default;
}
