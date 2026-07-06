namespace DynamicDataVNext;

/// <summary>
/// A set of options describing the nature of a selection (I.E. transformation) operation, and the nature of the new items it produces.
/// </summary>
public readonly record struct DistinctItemSelectionOptions
{
    /// <summary>
    /// The type of selection being performed.
    /// </summary>
    public ItemSelectionType Type { get; init; }
}
