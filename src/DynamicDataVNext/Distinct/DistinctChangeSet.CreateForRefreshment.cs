namespace DynamicDataVNext;

public static partial class DistinctChangeSet
{
    /// <inheritdoc cref="DistinctChangeSet{T}.CreateForRefreshment(T)"/>
    /// <typeparam name="T">The type of the refreshed item.</typeparam>
    public static DistinctChangeSet<T> CreateForRefreshment<T>(T item)
        => DistinctChangeSet<T>.CreateForRefreshment(item);
}

public readonly partial record struct DistinctChangeSet<T>
{
    /// <summary>
    /// Creates a new <see cref="DistinctChangeSet{T}"/> representing the <see cref="DistinctChangeType.Refreshment"/> of a single item.
    /// </summary>
    /// <param name="item">The refreshed item.</param>
    /// <returns>A <see cref="DistinctChangeSet{T}"/> describing the refreshment of the given item.</returns>
    public static DistinctChangeSet<T> CreateForRefreshment(T item)
        => CreateForUpdate(DistinctChange.CreateRefreshment(item));
}
