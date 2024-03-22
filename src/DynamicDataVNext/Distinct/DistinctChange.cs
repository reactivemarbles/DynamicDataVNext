namespace DynamicDataVNext;

/// <summary>
/// Contains convenience methods for creating <see cref="DistinctChange{T}"/> values.
/// </summary>
public static class DistinctChange
{
    /// <inheritdoc cref="DistinctChange{T}.Addition(T)"/>
    public static DistinctChange<T> Addition<T>(T item)
        => new()
        {
            Item = item,
            Type = DistinctChangeType.Addition
        };

    /// <inheritdoc cref="DistinctChange{T}.Removal(T)"/>
    public static DistinctChange<T> Removal<T>(T item)
        => new()
        {
            Item = item,
            Type = DistinctChangeType.Removal
        };
}

/// <summary>
/// Describes a single-item change made (or to be made) upon a collection of distinct items.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public readonly record struct DistinctChange<T>
{
    /// <summary>
    /// Creates a new <see cref="DistinctChange{T}"/> representing the addition of a given item.
    /// </summary>
    /// <param name="item">The item being added</param>
    /// <returns>A <see cref="DistinctChange{T}"/> describing the addition of the given item.</returns>
    public static DistinctChange<T> Addition(T item)
        => new()
        {
            Item = item,
            Type = DistinctChangeType.Addition
        };

    /// <summary>
    /// Creates a new <see cref="DistinctChange{T}"/> representing the addition of a given item.
    /// </summary>
    /// <param name="item">The item being added</param>
    /// <returns>A <see cref="DistinctChange{T}"/> describing the addition of the given item.</returns>
    public static DistinctChange<T> Removal(T item)
        => new()
        {
            Item = item,
            Type = DistinctChangeType.Removal
        };

    /// <summary>
    /// The subject item of the change being made.
    /// </summary>
    public required T Item { get; init; }

    /// <summary>
    /// The type of single-item change being made.
    /// </summary>
    public required DistinctChangeType Type { get; init; }
}
