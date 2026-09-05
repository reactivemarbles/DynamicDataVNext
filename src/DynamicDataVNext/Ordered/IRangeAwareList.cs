namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of ordered items, which supports various atomic operations upon ranges of items.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public interface IRangeAwareList<in T>
{
    /// <summary>
    /// Adds a range of items to the end of the list.
    /// </summary>
    /// <param name="items">The items to be added.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    void AddRange(IEnumerable<T> items);

    /// <summary>
    /// Inserts a range of items into the list, as an atomic operation.
    /// </summary>
    /// <param name="index">The index at which the first item in the range should be inserted.</param>
    /// <param name="items">The items to be inserted.</param>
    /// <exception cref="IndexOutOfRangeException">Throws when <paramref name="index"/> does not represent a valid index of an item in the list, or the next available index of the list.</exception>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    void InsertRange(
        int             index,
        IEnumerable<T>  items);

    /// <summary>
    /// Removes a range of consecutive items from the list, as an atomic operation.
    /// </summary>
    /// <param name="index">The index of the first item to be removed.</param>
    /// <param name="count">The number of items to be removed.</param>
    /// <exception cref="IndexOutOfRangeException">Throws when <paramref name="index"/> does not represent a valid index of an item within the list.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Throws when <paramref name="count"/> and <paramref name="index"/> define a range that extends beyond the end of the list.</exception>
    void RemoveRange(
        int index,
        int count);

    /// <summary>
    /// Performs a <see cref="ChangeSetType.Reset"/> operation upon the collection, by removing any existing items within the collection, and replacing them with the given items. 
    /// </summary>
    /// <typeparam name="TItems">The type of the <paramref name="items"/> sequence.</typeparam>
    /// <param name="items">The new set of items to be loaded into the collection.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    void Reset<TItems>(TItems items)
        where TItems : IEnumerable<T>;
}
