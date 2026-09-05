namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of ordered items, which supports atomic operations related to movement of items, within the list.
/// </summary>
public interface IMovementAwareList
{
    /// <summary>
    /// Moves an item within the list, as an atomic operation.
    /// </summary>
    /// <param name="oldIndex">The index of the item to be moved, before the operation.</param>
    /// <param name="newIndex">The desired index of the item to be moved, after the operation.</param>
    /// <exception cref="IndexOutOfRangeException">Throws when <paramref name="oldIndex"/> or <paramref name="newIndex"/> does not represent a valid index of an item in the list.</exception>
    void Move(
        int oldIndex,
        int newIndex);
}
