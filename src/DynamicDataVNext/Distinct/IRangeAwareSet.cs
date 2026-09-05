namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of distinct items, which supports various atomic operations upon ranges of items.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public interface IRangeAwareSet<in T>
{
    /// <summary>
    /// Performs a <see cref="ChangeSetType.Reset"/> operation upon the collection, by removing any existing items within the collection, and replacing them with the given items. 
    /// </summary>
    /// <typeparam name="TItems">The type of the <paramref name="items"/> sequence.</typeparam>
    /// <param name="items">The new set of items to be loaded into the collection.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    /// <remarks>
    /// Any duplicate items within <paramref name="items"/> are automatically ignored.
    /// </remarks>
    void Reset<TItems>(TItems items)
        where TItems : IEnumerable<T>;
}
