using System;
using System.Collections.Generic;

namespace DynamicDataVNext;

/// <summary>
/// Describes a virtualized collection of keyed items, in the form of a stream change notifications with additional metadata, which listeners may use to re-create the source collection, or create derived collections, over time.
/// </summary>
/// <typeparam name="TKey">The type of the item keys in the collection.</typeparam>
/// <typeparam name="TItem">The type of the item values in the collection.</typeparam>
public readonly record struct KeyedChangeStream<TKey, TItem>
{
    /// <summary>
    /// The comparer to be used by listeners, when comparing item keys for equality. 
    /// </summary>
    public required IEqualityComparer<TKey> KeyComparer { get; init; }

    /// <summary>
    /// A set of options describing the nature of items in the collection.
    /// </summary>
    public KeyedItemOptions Options { get; init; }
    
    /// <summary>
    /// The stream of change notifications that listeners may subscribe to.
    /// </summary>
    /// <remarks>
    /// <para>Upon subscription, if the source collection is not empty, a <see cref="ChangeSetType.Reset"/> notification is immediately published, containing the collection's full current state.</para>
    /// <para>Thereafter, additional notifications are published for each change operation made to the source collection.</para>  
    /// </remarks>
    public required IObservable<KeyedChangeSet<TKey, TItem>> Source { get; init; }
}
