namespace DynamicDataVNext;

/// <summary>
/// Defines the types of operations that can be used to "select" (I.E. transform) items within a collection.
/// </summary>
/// <remarks>
/// Supplying a <see cref="ItemSelectionType"/> value to a selection operator helps inform the operator about how to handle refresh operations (I.E. when to leave an item as-is, or to re-apply the selector), as well as how to inform downstream operators about the nature of the newly-selected items (I.E. how to set an "ItemOptions" value for the new stream). 
/// </remarks>
public enum ItemSelectionType
{
    /// <summary>
    /// Describes a selection operation that will always produce the same (or equivalent) output items, given the same (or equivalent) input items.  
    /// </summary>
    /// <remarks>
    /// This type of operation implies that both input and output items are immutable.
    /// </remarks>
    Deterministic,
    /// <summary>
    /// Describes a selection operation that may produce different output items, over time, even when given the same input items.
    /// </summary>
    /// <remarks>
    /// This type of operation implies that output items are immutable, but either the input items are mutable, or the output items implement reference-equality semantics, rather than value-equality semantics.  
    /// </remarks>
    NonDeterministic,
    /// <summary>
    /// Describes a selection operation that produces mutable output items, regardless of input.
    /// </summary>
    Mutable
}
