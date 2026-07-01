using System;
using System.Collections.Generic;

namespace DynamicDataVNext.Tests.Distinct;

public interface ISetUutFixture<out TUutFixture, out TUut>
        : IDisposable
    where TUutFixture : ISetUutFixture<TUutFixture, TUut>
    where TUut : ISet<int>
{
    static abstract TUutFixture Create(
        IEqualityComparer<int>?     comparer    = null,
        DistinctItemOptions  options     = default);

    static abstract TUutFixture Create(
        int                         capacity,
        IEqualityComparer<int>?     comparer    = null,
        DistinctItemOptions  options     = default);

    static abstract TUutFixture Create(
        IEnumerable<int>            items,
        IEqualityComparer<int>?     comparer    = null,
        DistinctItemOptions  options     = default);
    
    TUut Uut { get; }
    
    int UutCapacity { get; }
    
    IEqualityComparer<int> UutComparer { get; }
    
    DistinctItemOptions UutOptions { get; }

    void AssertItemWasAdded(int addedItem);

    void AssertItemWasRemoved(int removedItem);

    void AssertItemsWereAdded(IReadOnlyList<int> addedItems);

    void AssertItemsWereRemoved(
        IReadOnlyList<int>  removedItems,
        string              because);

    void AssertUutDidNothing();

    void AssertUutWasCleared(IReadOnlyList<int> items);

    void AssertUutWasReset(
        IReadOnlyList<int> oldItems,
        IReadOnlyList<int> newItems);

    void AssertUutWasUpdated(
        IReadOnlyList<int>  removedItems,
        IReadOnlyList<int>  addedItems,
        string              itemsRemovedBecause);
    
    void ResetUut(IEnumerable<int> items);
}
