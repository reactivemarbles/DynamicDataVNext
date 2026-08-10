using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using ReactiveUI.Primitives;
using ReactiveUI.Primitives.Signals;

namespace DynamicDataVNext;

public static partial class DistinctChangeStream
{
    /// <summary>
    /// Applies a given transformation to each item in a virtual collection, represented by a given change stream. 
    /// </summary>
    /// <param name="stream">The stream whose items are to be transformed.</param>
    /// <param name="selector">A delegate to be used to transform items.</param>
    /// <param name="comparer">The comparer to be used here, and downstream, for determining whether output items are equal to each other.</param>
    /// <param name="options">A set of options describing the functional nature of the output items, for the sake of downstream listeners.</param>
    /// <param name="refreshmentHandler">A handler to be used to perform item-refreshment operations, within the operator.</param>
    /// <typeparam name="TIn">The type of the input items, for the transformation.</typeparam>
    /// <typeparam name="TOut">The type of the output items, for the transformation.</typeparam>
    /// <returns>A change stream representing the transformed virtual collection.</returns>
    /// <exception cref="ArgumentException">Throws when <paramref name="refreshmentHandler"/> is given and <paramref name="stream"/> contains immutable items.</exception>
    /// <remarks>
    /// <para>
    /// Note that this operator does not guarantee the distinction of output items. That is, if two different input items produce the same output, those items will not be de-duplicated in the downstream collection. This could result in corrupt state within downstream operators, that assume upstream items are distinct. If de-duplication is desired, use the <see cref="SelectDistinct()"/> operators. 
    /// </para>
    /// <para>
    /// When <paramref name="options"/> is not given, the operator makes some reasonable assumptions for common scenarios, rather than just assuming a worst-case scenario, so that the consumer can still enjoy performance optimizations, without having to always specify options when using the operator.
    /// <br/>
    /// These assumptions include:
    /// <list type="bullet">
    /// <item><description>The selector itself is stateless. I.E. it's not a closure with captured, mutable, inputs that can affect its outputs. If a stateful selector is needed, use an overload of `<see cref="Select{TIn,TOut}"/> that accommodates selector state.</description></item>
    /// <item><description>The selector output type will usually have the same mutability/immutability as the input type, unless the operator can prove otherwise.</description></item>
    /// <item><description>Reference types that implement IEquatable{T} for themselves behave like value types, and are thus immutable. If this is not the case, supply <see cref="ItemSelectionType.Mutable"/> within <paramref name="options"/>.</description></item>
    /// <item><description>Reference types that don't implement IEquatable{T} for themselves utilize reference-equality semantics, making deterministic outputs impossible.</description></item>
    /// </list>
    /// <br/>
    /// The operator also informs its assumptions based on some factual deductions:
    /// <list type="bullet">
    /// <item><description>Value types are, by definition, always immutable.</description></item>
    /// <item><description>When the selector input is mutable, its output cannot be deterministic.</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// When no <paramref name="refreshmentHandler"/> is given, <paramref name="selector"/> will always be invoked for <see cref="DistinctChangeType.Refreshment"/> changes, and <paramref name="comparer"/> will be used to determine whether the output item has changed, and should be replaced, or should be left as-is. 
    /// </para>
    /// <para>
    /// <see cref="DistinctChangeType.Refreshment"/> changes are not forwarded downstream.
    /// </para>
    /// </remarks>
    public static DistinctChangeStream<TOut> Select<TIn, TOut>(
        this    DistinctChangeStream<TIn>                       stream,
                Func<TIn, TOut>                                 selector,
                IEqualityComparer<TOut>?                        comparer            = null,
                DistinctItemSelectionOptions?                   options             = null,
                DistinctSelectionRefreshmentHandler<TIn, TOut>? refreshmentHandler  = null)
    {
        var resolvedComparer = comparer ?? EqualityComparer<TOut>.Default;
        
        var resolvedOptions = options ?? new()
        {
            Type = (stream.Options.ItemsAreMutable,
                    typeof(TOut).IsValueType,
                    typeof(TOut).IsAssignableTo(typeof(IEquatable<>).MakeGenericType(typeof(TOut))))
                switch
            {
                (false, true,   _)      => ItemSelectionType.Deterministic,
                (false, _,      true)   => ItemSelectionType.Deterministic,
                (true,  false,  false)  => ItemSelectionType.Mutable,
                _                       => ItemSelectionType.NonDeterministic
            }
        };
        
        var resolvedItemOptions = new DistinctItemOptions()
        {
            ItemsAreMutable = resolvedOptions.Type is ItemSelectionType.Mutable
        };

        if (resolvedItemOptions.ItemsAreMutable && (refreshmentHandler is not null))
            throw new ArgumentException(
                message:    $"A {nameof(refreshmentHandler)} handler is not supported when {nameof(TIn)} represents immutable items, as {nameof(DistinctChangeType.Refreshment)} changes are not supported, and should not occur.",
                paramName:  nameof(refreshmentHandler));
        
        return new()
        {
            Comparer    = resolvedComparer,
            Options     = resolvedItemOptions,
            Source      = Signal.Create<DistinctChangeSet<TOut>>((resolvedOptions.Type is ItemSelectionType.Deterministic)
                ? SubscribeDeterministic
                : SubscribeNonDeterministic)
        };
        

        IDisposable SubscribeDeterministic(IObserver<DistinctChangeSet<TOut>> downstreamObserver)
            => stream.Source
                .Where(upstreamChangeSet => upstreamChangeSet.Type is not ChangeSetType.Empty)
                .Select(upstreamChangeSet =>
                {
                    var downstreamChanges = ImmutableArray.CreateBuilder<DistinctChange<TOut>>(initialCapacity: upstreamChangeSet.Changes.Length);
                    
                    foreach (var upstreamChange in upstreamChangeSet.Changes)
                    {
                        // Ignoring the possibility of refresh changes here, since we never assume a deterministic
                        // selection from mutable items. I.E. A refresh can only occur here when the consumer explicitly
                        // told us that the selection is deterministic, despite the items being mutable, meaning the
                        // consumer agrees not to mutate items in a way that would change the selection output.

                        downstreamChanges.Add(new()
                        {
                            Item = selector.Invoke(upstreamChange.Item),
                            Type = upstreamChange.Type
                        });
                    }
                    
                    return new DistinctChangeSet<TOut>()
                    {
                        Changes             = downstreamChanges.MoveToImmutable(),
                        FirstAdditionIndex  = upstreamChangeSet.FirstAdditionIndex, 
                        Type                = upstreamChangeSet.Type,
                    };
                })
                .SubscribeSafe(downstreamObserver);

        IDisposable SubscribeNonDeterministic(IObserver<DistinctChangeSet<TOut>> downstreamObserver)
        {
            var downstreamItemsByUpstreamItem = new Dictionary<Optional<TIn>, TOut>(comparer: new OptionalEqualityComparer<TIn>(stream.Comparer));

            return stream.Source
                .Where(upstreamChangeSet => upstreamChangeSet.Type is not ChangeSetType.Empty)
                .Select(upstreamChangeSet =>
                {
                    var downstreamChanges = ImmutableArray.CreateBuilder<DistinctChange<TOut>>(initialCapacity: upstreamChangeSet.Changes.Length);
                    
                    foreach (var upstreamChange in upstreamChangeSet.Changes)
                    {
                        switch (upstreamChange.Type)
                        {
                            case DistinctChangeType.Addition:
                                {
                                    var downstreamItem = selector.Invoke(upstreamChange.Item);

                                    downstreamItemsByUpstreamItem.Add(
                                        key:    upstreamChange.Item,
                                        value:  downstreamItem);
                                    
                                    downstreamChanges.Add(new()
                                    {
                                        Item = downstreamItem,
                                        Type = upstreamChange.Type
                                    });
                                }
                                break;
                            
                            case DistinctChangeType.Refreshment:
                                {
                                    var priorDownstreamItem = downstreamItemsByUpstreamItem[upstreamChange.Item];
                                    
                                    TOut newDownstreamItem;
                                    if (refreshmentHandler is null)
                                    {
                                        newDownstreamItem = selector.Invoke(upstreamChange.Item);
                                        
                                        if (resolvedComparer.Equals(priorDownstreamItem, newDownstreamItem))
                                            continue;
                                    }
                                    else
                                    {
                                        var handlerResult = refreshmentHandler.Invoke(
                                            inputItem:          upstreamChange.Item,
                                            priorOutputItem:    priorDownstreamItem);
                                        
                                        if (!handlerResult.IsSpecified)
                                            continue;

                                        newDownstreamItem = handlerResult.Value;
                                    }

                                    downstreamItemsByUpstreamItem[upstreamChange.Item] = newDownstreamItem;
                                    downstreamChanges.Add(new()
                                    {
                                        Type = DistinctChangeType.Removal,
                                        Item = priorDownstreamItem
                                    });
                                    downstreamChanges.Add(new()
                                    {
                                        Type = DistinctChangeType.Addition,
                                        Item = newDownstreamItem
                                    });
                                }
                                break;

                            case DistinctChangeType.Removal:
                                downstreamChanges.Add(new()
                                {
                                    Item = downstreamItemsByUpstreamItem[upstreamChange.Item],
                                    Type = upstreamChange.Type
                                });
                                
                                downstreamItemsByUpstreamItem.Remove(upstreamChange.Item);
                                break;
                        }
                    }
                        
                    return new DistinctChangeSet<TOut>()
                    {
                        Changes             = downstreamChanges.MoveToImmutable(),
                        FirstAdditionIndex  = upstreamChangeSet.FirstAdditionIndex, 
                        Type                = upstreamChangeSet.Type,
                    };
                })
                .SubscribeSafe(downstreamObserver);
        }
    }
}
