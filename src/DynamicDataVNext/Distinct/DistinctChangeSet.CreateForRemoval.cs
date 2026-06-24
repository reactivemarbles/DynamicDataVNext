namespace DynamicDataVNext;

public static partial class DistinctChangeSet
{
    /// <inheritdoc cref="DistinctChangeSet{T}.CreateForRemoval(T)"/>
    /// <typeparam name="T">The type of the removed item.</typeparam>
    public static DistinctChangeSet<T> CreateForRemoval<T>(T item)
        => DistinctChangeSet<T>.CreateForRemoval(item);
}

public readonly partial record struct DistinctChangeSet<T>
{
    /// <summary>
    /// Creates a new <see cref="DistinctChangeSet{T}"/> representing the <see cref="DistinctChangeType.Removal"/> of a single item.
    /// </summary>
    /// <param name="item">The removed item.</param>
    /// <returns>A <see cref="DistinctChangeSet{T}"/> describing the removal of the given item.</returns>
    public static DistinctChangeSet<T> CreateForRemoval(T item)
        => CreateForUpdate(DistinctChange.CreateRemoval(item));
}
