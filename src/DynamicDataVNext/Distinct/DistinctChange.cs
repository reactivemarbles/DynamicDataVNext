namespace DynamicDataVNext;

/// <summary>
/// Contains convenience methods for creating <see cref="DistinctChange{T}"/> values.
/// </summary>
public static partial class DistinctChange { }

/// <summary>
/// Describes a single-item change to a collection of distinct items.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public readonly partial record struct DistinctChange<T>
    : IChange<DistinctChangeType>
{
    /// <inheritdoc/>
    public ChangeCategory Category
        => Type switch
        {
            DistinctChangeType.None         => ChangeCategory.None,
            DistinctChangeType.Addition     => ChangeCategory.Addition,
            DistinctChangeType.Removal      => ChangeCategory.Removal,
            _                               => ChangeCategory.Other
        };
    
    /// <summary>
    /// The subject item of the change being described.
    /// </summary>
    public required T Item { get; init; }

    /// <inheritdoc/>
    public required DistinctChangeType Type { get; init; }
}
