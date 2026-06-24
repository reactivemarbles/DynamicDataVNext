namespace DynamicDataVNext;

public static partial class DistinctChangeSet
{
    /// <inheritdoc cref="DistinctChangeSet{T}.CreateForAddition(T)"/>
    /// <typeparam name="T">The type of the added item.</typeparam>
    public static DistinctChangeSet<T> CreateForAddition<T>(T item)
        => DistinctChangeSet<T>.CreateForAddition(item);
}

public readonly partial record struct DistinctChangeSet<T>
{
    /// <summary>
    /// Creates a new <see cref="DistinctChangeSet{T}"/> representing the <see cref="DistinctChangeType.Addition"/> of a single item to a collection.
    /// </summary>
    /// <param name="item">The added item.</param>
    /// <returns>A <see cref="DistinctChangeSet{T}"/> describing the addition of the given item.</returns>
    public static DistinctChangeSet<T> CreateForAddition(T item)
        => CreateForUpdate(DistinctChange.CreateAddition(item));
}
