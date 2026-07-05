namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of items that can be instructed to expand its internal storage capacity, prior to actually needing it.
/// </summary>
public interface IExpandableCollection
{
    /// <summary>
    /// The maximum number of items that the collection can store without having to perform an internal resizing re-allocation.
    /// </summary>
    int Capacity { get; } 

    /// <summary>
    /// Ensures that <see cref="Capacity"/> is at least a given value, or greater, by performing an internal reallocation, if necessary.
    /// </summary>
    /// <param name="capacity">The desired minimum value of <see cref="Capacity"/>.</param>
    void EnsureCapacity(int capacity);
}
