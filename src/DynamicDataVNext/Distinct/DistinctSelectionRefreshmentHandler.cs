namespace DynamicDataVNext;

/// <summary>
/// Describes a handler for processing a <see cref="DistinctChangeType.Refreshment"/> change, within a selection (I.E. transformation) operator.
/// </summary>
/// <param name="inputItem">The item that is being refreshed, upstream of the selection operator.</param>
/// <param name="priorOutputItem">The last output item that was produced by the selector, for <paramref name="inputItem"/>.</param>
/// <typeparam name="TIn">The type of the input items, for the selection.</typeparam>
/// <typeparam name="TOut">The type of the output items, for the selection.</typeparam>
/// <returns>
/// A new output item to be used to replace <paramref name="priorOutputItem"/>, downstream of the selection operator, if specified. Otherwise <paramref name="priorOutputItem"/> is refreshed, instead of replaced.
/// </returns>
/// <remarks>
/// This allows consumers of a selection operator to define a variety of different behaviors for handling <see cref="DistinctChangeType.Refreshment"/> changes, including re-selecting and replacing items, mutating downstream items in-place, to match upstream mutations, and even ignoring refreshment changes entirely..  
/// </remarks>
public delegate Optional<TOut> DistinctSelectionRefreshmentHandler<in TIn, TOut>(
    TIn     inputItem,
    TOut    priorOutputItem);
