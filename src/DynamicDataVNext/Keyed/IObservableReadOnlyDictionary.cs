using System;
using System.Collections.Generic;
using System.Reactive;

namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of items, with distinct keys, which may not be mutated by the consumer, and which publishes notifications about its mutations, as they occur.
/// </summary>
/// <typeparam name="TKey">The type of the item keys in the collection.</typeparam>
/// <typeparam name="TValue">The type of the item values in the collection.</typeparam>
public interface IObservableReadOnlyDictionary<TKey, TValue>
    : IObservableReadOnlyCollection<KeyValuePair<TKey, TValue>>,
        IReadOnlyDictionary<TKey, TValue>
{
    /// <inheritdoc cref="IObservableDictionary{TKey, TValue}.ChangeStream"/>
    KeyedChangeStream<TKey, TValue> ChangeStream { get; }

    /// <inheritdoc cref="IObservableDictionary{TKey, TValue}.Keys"/>
    new IReadOnlyCollection<TKey> Keys { get; }

    /// <inheritdoc cref="IObservableDictionary{TKey, TValue}.Values"/>
    new IReadOnlyCollection<TValue> Values { get; }
}
