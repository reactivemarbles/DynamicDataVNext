namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of keyed items, which supports various atomic operations upon ranges of items.
/// </summary>
/// <typeparam name="TItem">The type of the items in the collection.</typeparam>
public interface IRangeAwareCache<in TItem>
{
    /// <summary>
    /// Adds a range of items to the collection, as a single operation.
    /// </summary>
    /// <param name="items">The items to be added.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    /// <exception cref="ArgumentException">Throws if <paramref name="items"/> contains any items whose key value is duplicated or already in the collection.</exception>
    void AddRange(IEnumerable<TItem> items);

    /// <summary>
    /// Merges a range of items into the collection by either adding or replacing each one, based on whether an item with the same key is already present.
    /// </summary>
    /// <param name="items">The items to be added or replaced</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    void MergeRange(IEnumerable<TItem> items);

    /// <summary>
    /// Removes a set of items from the collection, as a single operation.
    /// </summary>
    /// <param name="items">The set of items to be removed.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    /// <remarks>
    /// Note that the collection will silently ignore items that are not present in the collection.
    /// </remarks>
    void RemoveRange(IEnumerable<TItem> items);

    /// <summary>
    /// Performs a <see cref="ChangeSetType.Reset"/> operation upon the collection, by removing any existing items within the collection, and replacing them with the given items. 
    /// </summary>
    /// <typeparam name="TItems">The type of the <paramref name="items"/> sequence.</typeparam>
    /// <param name="items">The new set of items to be loaded into the collection.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    void Reset<TItems>(TItems items)
        where TItems : IEnumerable<TItem>;
}
