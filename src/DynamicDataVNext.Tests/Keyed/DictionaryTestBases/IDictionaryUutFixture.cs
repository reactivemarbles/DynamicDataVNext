using System;
using System.Collections.Generic;

namespace DynamicDataVNext.Tests.Keyed;

public interface IDictionaryUutFixture<out TUutFixture, out TUut>
        : IDisposable
    where TUutFixture : IDictionaryUutFixture<TUutFixture, TUut>
    where TUut : IDictionary<string, int>
{
    static abstract TUutFixture Create(
        IEqualityComparer<string>?  comparer    = null,
        KeyedItemOptions            options     = default);

    static abstract TUutFixture Create(
        int                         capacity,
        IEqualityComparer<string>?  comparer    = null,
        KeyedItemOptions            options     = default);

    static abstract TUutFixture Create(
        IEnumerable<KeyValuePair<string, int>>  items,
        IEqualityComparer<string>?              comparer    = null,
        KeyedItemOptions                        options     = default);
    
    TUut Uut { get; }
    
    int UutCapacity { get; }
    
    IEqualityComparer<string> UutComparer { get; }
    
    KeyedItemOptions UutOptions { get; }

    void AddRangeToUut(IEnumerable<KeyValuePair<string, int>> items);

    void AddRangeToUut(
        IEnumerable<int>    values,
        Func<int, string>   keySelector);
    
    void AssertItemWasAdded(
        string  addedKey,
        int     addedValue);

    void AssertItemWasRefreshed(
        string  key,
        int     value);

    void AssertItemWasRemoved(
        string  removedKey,
        int     removedValue);

    void AssertItemsWereAdded(IReadOnlyList<KeyValuePair<string, int>> addedItems);

    // void AssertItemsWereRemoved(
    //     IReadOnlyList<int>  removedItems,
    //     string              because);

    void AssertUutDidNothing();

    void AssertUutWasCleared(IReadOnlyList<KeyValuePair<string, int>> items);

    void AssertUutWasReset(
        IReadOnlyList<KeyValuePair<string, int>> oldItems,
        IReadOnlyList<KeyValuePair<string, int>> newItems);

    // void AssertUutWasUpdated(
    //     IReadOnlyList<int>  removedItems,
    //     IReadOnlyList<int>  addedItems,
    //     string              itemsRemovedBecause);
    
    bool RefreshUut(string key);
    
    void ResetUut(
        IEnumerable<int>    values,
        Func<int, string>   keySelector);
}
