namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public interface ICacheUutFixture<out TUutFixture, out TUut>
        : IDisposable
    where TUutFixture : ICacheUutFixture<TUutFixture, TUut>
    where TUut : ICache<string, TestItem>
{
    static abstract TUutFixture Create(
        Func<TestItem, string>      keySelector,
        IEqualityComparer<string>?  comparer    = null,
        KeyedItemOptions            options     = default);

    static abstract TUutFixture Create(
        int                         capacity,
        Func<TestItem, string>      keySelector,
        IEqualityComparer<string>?  comparer    = null,
        KeyedItemOptions            options     = default);

    static abstract TUutFixture Create(
        IEnumerable<TestItem>       items,
        Func<TestItem, string>      keySelector,
        IEqualityComparer<string>?  comparer    = null,
        KeyedItemOptions            options     = default);
    
    TUut Uut { get; }
    
    IEqualityComparer<string> UutComparer { get; }
    
    KeyedItemOptions UutOptions { get; }

    void AssertItemWasAdded(TestItem addedItem);

    void AssertItemWasRefreshed(TestItem refreshedItem);

    void AssertItemWasRemoved(TestItem removedItem);

    void AssertItemWasReplaced(
        TestItem oldItem,
        TestItem newItem);

    void AssertItemsWereAdded(IReadOnlyList<TestItem> addedItems);

    void AssertItemsWereMerged(
        IReadOnlyList<TestItem>                             addedItems,
        IReadOnlyList<KeyedReplacement<string, TestItem>>   replacements);

    void AssertItemsWereRemoved(IReadOnlyList<TestItem> removedItems);

    void AssertKeyWasRefreshed(string key);

    void AssertUutDidNothing();

    void AssertUutWasCleared(IReadOnlyList<TestItem> removedItems);

    void AssertUutWasReset(
        IReadOnlyList<TestItem> removedItems,
        IReadOnlyList<TestItem> addedItems);
}
