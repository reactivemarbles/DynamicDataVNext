namespace DynamicDataVNext;

public static partial class DistinctChange
{
    /// <inheritdoc cref="DistinctChange{T}.CreateRefreshment"/>
    public static DistinctChange<T> CreateRefreshment<T>(T item)
        => DistinctChange<T>.CreateRefreshment(item);
}

public readonly partial record struct DistinctChange<T>
{
    /// <summary>
    /// Creates a new <see cref="DistinctChange{T}"/> representing the <see cref="DistinctChangeType.Refreshment"/> of a given item.
    /// </summary>
    /// <param name="item">The item being added</param>
    /// <returns>A <see cref="DistinctChange{T}"/> describing the refreshment of the given item.</returns>
    public static DistinctChange<T> CreateRefreshment(T item)
        => new()
        {
            Item = item,
            Type = DistinctChangeType.Refreshment
        };
}
