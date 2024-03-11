using System;
using System.Collections.Generic;
using System.Reactive;

namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of items, with distinct keys, which may not be mutated by the consumer, and which publishes notifications about its mutations to subscribers, as they occur.
/// </summary>
/// <typeparam name="TKey">The type of the item keys in the collection.</typeparam>
/// <typeparam name="TValue">The type of the item values in the collection.</typeparam>
public interface IObservableReadOnlyDictionary<TKey, TValue>
    : IReadOnlyDictionary<TKey, TValue>,
        IObservable<KeyedChange<TKey, TValue>>
{
    /// <inheritdoc cref="IObservableDictionary{TKey, TValue}.CollectionChanged"/>
    IObservable<Unit> CollectionChanged { get; }

    /// <inheritdoc cref="IDictionary{TKey, TValue}.Keys"/>
    new IReadOnlyCollection<TKey> Keys { get; }

    /// <inheritdoc cref="IDictionary{TKey, TValue}.Values"/>
    new IReadOnlyCollection<TValue> Values { get; }

    /// <inheritdoc cref="IObservableDictionary{TKey, TValue}.ObserveValue(TKey)"/>
    IObservable<TValue> ObserveValue(TKey key);

    /// <inheritdoc cref="IObservableDictionary{TKey, TValue}.SuspendNotifications"/>
    IDisposable SuspendNotifications();
}
