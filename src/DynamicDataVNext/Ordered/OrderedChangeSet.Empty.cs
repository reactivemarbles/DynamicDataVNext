namespace DynamicDataVNext;

public static partial class OrderedChangeSet
{
    /// <summary>
    /// Retrieve a copy of the empty changeset.
    /// </summary>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    /// <returns>A copy of the empty changeset.</returns>
    public static OrderedChangeSet<T> Empty<T>()
        => OrderedChangeSet<T>.Empty;
}

public readonly partial record struct OrderedChangeSet<T>
{
    /// <summary>
    /// A copy of the empty changeset.
    /// </summary>
    public static readonly OrderedChangeSet<T> Empty
        = default;
}
