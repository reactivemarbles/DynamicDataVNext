using System;
using System.Collections.Generic;
using System.Reactive;

namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of items of unspecified nature, which may not be mutated by the consumer, and which publishes notifications about its mutations, as they occur.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public interface IObservableReadOnlyCollection<out T>
    : IReadOnlyCollection<T>
{
    /// <inheritdoc cref="IObservableCollection{T}.CollectionChanged"/>
    IObservable<Unit> CollectionChanged { get; }
}
