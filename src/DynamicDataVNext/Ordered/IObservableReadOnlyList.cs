using System;
using System.Collections.Generic;
using System.Reactive;

namespace DynamicDataVNext;

/// <summary>
/// Describes a collection of ordered items, which may not be mutated by the consumer, and which publishes notifications about its mutations, as they occur.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public interface IObservableReadOnlyList<T>
    : IObservableReadOnlyCollection<T>,
        IReadOnlyList<T>
{
    /// <inheritdoc cref="IObservableList{T}.ChangeStream"/>
    OrderedChangeStream<T> ChangeStream { get; }
}
