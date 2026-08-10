namespace DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

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
        string  refreshedKey,
        int     refreshedValue);

    void AssertItemWasRemoved(
        string  removedKey,
        int     removedValue);

    void AssertItemWasReplaced(
        string  replacementKey,
        int     replacedValue,
        int     replacementValue);

    void AssertItemsWereAdded(IReadOnlyList<KeyValuePair<string, int>> addedItems);

    void AssertUutDidNothing();

    void AssertUutWasCleared(IReadOnlyList<KeyValuePair<string, int>> removedItems);

    void AssertUutWasReset(
        IReadOnlyList<KeyValuePair<string, int>> removedItems,
        IReadOnlyList<KeyValuePair<string, int>> addedItems);

    bool RefreshUut(string key);
    
    void ResetUut(
        IEnumerable<int>    values,
        Func<int, string>   keySelector);
}
