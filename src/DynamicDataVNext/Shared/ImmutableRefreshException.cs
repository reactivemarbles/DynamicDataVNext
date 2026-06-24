using System;

namespace DynamicDataVNext;

// We COULD just ignore refresh changes for immutable items, but in a scenario where we know that items are immutable,
// a refresh indicates that either the consumer screwed something up, or we did. Best to fail early.
/// <summary>
/// The exception that is thrown when a "Refreshment" change is encountered within the change stream of a collection of immutable items.
/// </summary>
public class ImmutableRefreshException
    : InvalidOperationException
{
    /// <inheritdoc/>
    public ImmutableRefreshException()
        : base(Message)
    { }
    
    private new const string Message = $"{nameof(DistinctChangeType.Refreshment)} changes are invalid for immutable items";
}
