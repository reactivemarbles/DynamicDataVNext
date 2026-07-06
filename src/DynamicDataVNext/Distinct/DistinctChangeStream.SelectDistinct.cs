using System;
using System.Collections.Generic;

namespace DynamicDataVNext;

public static partial class DistinctChangeStream
{
    /// <summary>
    /// Applies a given transformation to each item in a virtual collection, represented by a given change stream, and eliminates duplicate output items. 
    /// </summary>
    /// <param name="stream">The stream whose items are to be transformed.</param>
    /// <param name="selector">A delegate to be used to transform items.</param>
    /// <param name="comparer">The comparer to be used here, and downstream, for determining whether output items are equal to each other.</param>
    /// <param name="options">A set of options describing the functional nature of the output items, for the sake of downstream listeners.</param>
    /// <param name="refreshmentHandler">A handler to be used to perform item-refreshment operations, within the operator.</param>
    /// <typeparam name="TIn">The type of the input items, for the transformation.</typeparam>
    /// <typeparam name="TOut">The type of the output items, for the transformation.</typeparam>
    /// <returns>A change stream representing the transformed and de-duplicated virtual collection.</returns>
    /// <exception cref="NotImplementedException">This operator has not yet been implemented.</exception>
    public static DistinctChangeStream<TOut> SelectDistinct<TIn, TOut>(
            this    DistinctChangeStream<TIn>                       stream,
                    Func<TIn, TOut>                                 selector,
                    IEqualityComparer<TOut>?                        comparer            = null,
                    DistinctItemSelectionOptions?                   options             = null,
                    DistinctSelectionRefreshmentHandler<TIn, TOut>? refreshmentHandler  = null)
        => throw new NotImplementedException();
}
