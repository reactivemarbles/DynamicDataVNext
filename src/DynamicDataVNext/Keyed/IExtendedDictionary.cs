using System;
using System.Collections.Generic;

namespace DynamicDataVNext;

/// <summary>
/// Describes an extended version of <see cref="IDictionary{TKey, TValue}"/>, supporting range operations.
/// </summary>
/// <typeparam name="TKey">The type of the item keys in the collection.</typeparam>
/// <typeparam name="TValue">The type of the item values in the collection.</typeparam>
/// <remarks>
/// It's worth noting that the lack of an AddRange() method on this interface is intentional. Such a method would intuitively need to follow the pattern of <see cref="IDictionary{TKey, TValue}.Add(TKey, TValue)"/> and throw an exception when a key is presented that is already present in the collection, which would then introduce the possibility for an AddRange() operation to only be half-applied to the collection. The most appropriate thing to do would be to roll back any items already added to the collection, when the exception is thrown, which would require quite a lot of additional state tracking and item iteration. The <see cref="AddOrReplaceRange(IEnumerable{KeyValuePair{TKey, TValue}})"/> should usually be preferred, for adding batches of items to a collection, and if key validation is desired, it can be implemented on the consumer's end.
/// </remarks>
public interface IExtendedDictionary<TKey, TValue>
    : IDictionary<TKey, TValue>
{
    /// <inheritdoc cref="IDictionary{TKey, TValue}.Keys"/>
    new IReadOnlyCollection<TKey> Keys { get; }

    /// <inheritdoc cref="IDictionary{TKey, TValue}.Values"/>
    new IReadOnlyCollection<TValue> Values { get; }

    /// <summary>
    /// Applies a range of items to the collection, as a single operation, by either adding or replacing each item, based on its selected key value.
    /// </summary>
    /// <param name="values">The values to use as <see cref="KeyValuePair{TKey, TValue}.Value"/> for each applied item.</param>
    /// <param name="keySelector">A selector to select a <see cref="KeyValuePair{TKey, TValue}.Key"/> value for each applied item.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="values"/> and <paramref name="keySelector"/>.</exception>
    void AddOrReplaceRange(
        IEnumerable<TValue> values,
        Func<TValue, TKey>  keySelector);

    /// <summary>
    /// Applies a range of items to the collection, as a single operation, by either adding or replacing each item, based on its selected key value.
    /// </summary>
    /// <param name="values">The values to use as <see cref="KeyValuePair{TKey, TValue}.Value"/> for each applied item.</param>
    /// <param name="keySelector">A selector to select a <see cref="KeyValuePair{TKey, TValue}.Key"/> value for each applied item.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="keySelector"/>.</exception>
    void AddOrReplaceRange(
        ReadOnlySpan<TValue>    values,
        Func<TValue, TKey>      keySelector);

    /// <summary>
    /// Applies a range of items to the collection, as a single operation, by either adding or replacing each item, based on its key value.
    /// </summary>
    /// <param name="items">The keys and values to be applied.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    void AddOrReplaceRange(IEnumerable<KeyValuePair<TKey, TValue>> items);

    /// <summary>
    /// Applies a range of items to the collection, as a single operation, by either adding or replacing each item, based on its key value.
    /// </summary>
    /// <param name="items">The keys and values to be applied.</param>
    void AddOrReplaceRange(ReadOnlySpan<KeyValuePair<TKey, TValue>> items);

    /// <summary>
    /// Removes a set of items, by key, from the collection, as a single operation.
    /// </summary>
    /// <param name="keys">The set of keys to be removed.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="keys"/>.</exception>
    /// <remarks>
    /// Note that the collection will silently ignore keys that are not present in the collection.
    /// </remarks>
    void RemoveRange(IEnumerable<TKey> keys);

    /// <summary>
    /// Removes a set of items, by key, from the collection, as a single operation.
    /// </summary>
    /// <param name="keys">The set of keys to be removed.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="keys"/>.</exception>
    /// <remarks>
    /// Note that the collection will silently ignore keys that are not present in the collection.
    /// </remarks>
    void RemoveRange(ReadOnlySpan<TKey> keys);

    /// <summary>
    /// Resets the items within the collection to the given set of items. I.E. clears and then re-populates the collection, as a single operation.
    /// </summary>
    /// <param name="values">The set of items with which to populate the collection.</param>
    /// <param name="keySelector">A selector to select a <see cref="KeyValuePair{TKey, TValue}.Key"/> value for each new item.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="values"/> and <paramref name="keySelector"/>.</exception>
    void Reset(
        IEnumerable<TValue> values,
        Func<TValue, TKey>  keySelector);

    /// <summary>
    /// Resets the items within the collection to the given set of items. I.E. clears and then re-populates the collection, as a single operation.
    /// </summary>
    /// <param name="values">The set of items with which to populate the collection.</param>
    /// <param name="keySelector">A selector to select a <see cref="KeyValuePair{TKey, TValue}.Key"/> value for each new item.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="keySelector"/>.</exception>
    void Reset(
        ReadOnlySpan<TValue>    values,
        Func<TValue, TKey>      keySelector);

    /// <summary>
    /// Resets the items within the collection to the given set of items. I.E. clears and then re-populates the collection, as a single operation.
    /// </summary>
    /// <param name="items">The keys vand values to be added.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="keys"/>.</exception>
    void Reset(IEnumerable<KeyValuePair<TKey, TValue>> items);

    /// <summary>
    /// Resets the items within the collection to the given set of items. I.E. clears and then re-populates the collection, as a single operation.
    /// </summary>
    /// <param name="items">The keys vand values to be added.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="keys"/>.</exception>
    void Reset(ReadOnlySpan<KeyValuePair<TKey, TValue>> items);
}
