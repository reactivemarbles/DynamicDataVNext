namespace DynamicDataVNext;

public static partial class DistinctChange
{
    /// <inheritdoc cref="DistinctChange{T}.CreateRemoval"/>
    public static DistinctChange<T> CreateRemoval<T>(T item)
        => DistinctChange<T>.CreateRemoval(item);
}

public readonly partial record struct DistinctChange<T>
{
    /// <summary>
    /// Creates a new <see cref="DistinctChange{T}"/> representing the <see cref="DistinctChangeType.Removal"/> of a given item.
    /// </summary>
    /// <param name="item">The item being added</param>
    /// <returns>A <see cref="DistinctChange{T}"/> describing the removal of the given item.</returns>
    public static DistinctChange<T> CreateRemoval(T item)
        => new()
        {
            Item = item,
            Type = DistinctChangeType.Removal
        };
}
