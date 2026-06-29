using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DynamicDataVNext;

public static partial class OrderedChangeSet
{
    /// <inheritdoc cref="OrderedChangeSet{T}.CreateForInsertions(int, IEnumerable{T})"/>
    /// <typeparam name="T">The type of items being inserted.</typeparam>
    public static OrderedChangeSet<T> CreateForInsertions<T>(
            int             index,
            IEnumerable<T>  items)
        => OrderedChangeSet<T>.CreateForInsertions(
            index:  index,
            items:  items);

    /// <inheritdoc cref="OrderedChangeSet{T}.CreateForInsertions(int, ReadOnlySpan{T})"/>
    /// <typeparam name="T">The type of items being inserted.</typeparam>
    public static OrderedChangeSet<T> CreateForInsertions<T>(
            int             index,
            ReadOnlySpan<T> items)
        => OrderedChangeSet<T>.CreateForInsertions(
            index:  index,
            items:  items);
}

public readonly partial record struct OrderedChangeSet<T>
{
    /// <inheritdoc cref="CreateForInsertions(int, ReadOnlySpan{T})"/>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    public static OrderedChangeSet<T> CreateForInsertions(
        int             index,
        IEnumerable<T>  items)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index);
        ArgumentNullException.ThrowIfNull(items);
    
        var isItemCountValid = items.TryGetNonEnumeratedCount(out var itemCount);
        if (isItemCountValid && (itemCount is 0))
            return Empty;

        var changes = isItemCountValid
            ? ImmutableArray.CreateBuilder<OrderedChange<T>>(initialCapacity: itemCount)
            : ImmutableArray.CreateBuilder<OrderedChange<T>>();

        var insertionIndex = index;
        foreach(var item in items)
            changes.Add(OrderedChange.CreateInsertion(
                index:  insertionIndex++,
                item:   item));

        if (changes.Count is 0)
            return Empty;

        return new()
        {
            Changes = changes.MoveToOrCreateImmutable(),
            Type    = ChangeSetType.Update
        };
    }

    /// <summary>
    /// Creates a new <see cref="OrderedChangeSet{T}"/> representing the <see cref="OrderedChangeType.Insertion"/> of a range of items.
    /// </summary>
    /// <param name="index">The index at which the insertion occurs.</param>
    /// <param name="items">The inserted items.</param>
    /// <returns>An <see cref="OrderedChangeSet{T}"/> describing the insertion of the given items.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if <paramref name="index"/> is negative.</exception>
    public static OrderedChangeSet<T> CreateForInsertions(
        int             index,
        ReadOnlySpan<T> items)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index);
    
        if (items.Length is 0)
            return Empty;

        var changes = ImmutableArray.CreateBuilder<OrderedChange<T>>(initialCapacity: items.Length);

        var insertionIndex = index;
        foreach(var item in items)
            changes.Add(OrderedChange.CreateInsertion(
                index:  insertionIndex++,
                item:   item));

        return new()
        {
            Changes = changes.MoveToImmutable(),
            Type    = ChangeSetType.Update
        };
    }
}
