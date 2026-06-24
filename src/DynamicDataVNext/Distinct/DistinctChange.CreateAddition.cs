namespace DynamicDataVNext;

public static partial class DistinctChange
{
    /// <inheritdoc cref="DistinctChange{T}.CreateAddition"/>
    public static DistinctChange<T> CreateAddition<T>(T item)
        => DistinctChange<T>.CreateAddition(item);
}

public readonly partial record struct DistinctChange<T>
{
    /// <summary>
    /// Creates a new <see cref="DistinctChange{T}"/> representing the <see cref="DistinctChangeType.Addition"/> of a given item.
    /// </summary>
    /// <param name="item">The item being added</param>
    /// <returns>A <see cref="DistinctChange{T}"/> describing the addition of the given item.</returns>
    public static DistinctChange<T> CreateAddition(T item)
        => new()
        {
            Item = item,
            Type = DistinctChangeType.Addition
        };
}
