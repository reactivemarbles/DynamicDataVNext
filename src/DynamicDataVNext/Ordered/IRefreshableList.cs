namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of ordered items, which can report on mutations made to its items.
/// </summary>
public interface IRefreshableList
{
    /// <summary>
    /// Signals that the given item within the collection has been externally mutated.
    /// </summary>
    /// <param name="index">The index of the item that was refreshed.</param>
    /// <exception cref="IndexOutOfRangeException">Throws when <paramref name="index"/> does not represent a valid index of an item within the list.</exception>
    /// <exception cref="ImmutableRefreshException">Throws if the items in the collection are immutable. This is generally the result of a collection being initialized with a <see cref="OrderedItemOptions.ItemsAreMutable"/> value of <see langword="false"/>.</exception>
    void RefreshAt(int index);
}
