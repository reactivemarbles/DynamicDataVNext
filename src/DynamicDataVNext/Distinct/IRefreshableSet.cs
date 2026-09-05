namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of distinct items, which supports <see cref="ChangeCategory.Refreshment"/> operations.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public interface IRefreshableSet<in T>
{
    /// <summary>
    /// Signals that the given item within the collection has been externally mutated.
    /// </summary>
    /// <param name="item">The item that was mutated.</param>
    /// <returns><see langword="false"/> if the collection does not actually contain <paramref name="item"/>. Otherwise, <see langword="true"/>.</returns>
    /// <exception cref="ImmutableRefreshException">Throws if the items in the collection are immutable. This is generally the result of a collection being initialized with a <see cref="DistinctItemOptions.ItemsAreMutable"/> value of <see langword="false"/>.</exception>
    bool Refresh(T item);
}
