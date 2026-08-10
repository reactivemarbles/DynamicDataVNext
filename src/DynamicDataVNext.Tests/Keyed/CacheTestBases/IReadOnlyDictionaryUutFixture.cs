namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public interface IReadOnlyCacheUutFixture<out TUutFixture, out TUut>
        : IDisposable
    where TUutFixture : IReadOnlyCacheUutFixture<TUutFixture, TUut>
    where TUut : IReadOnlyCache<string, TestItem>
{
    static abstract TUutFixture Create(
        IEnumerable<TestItem>       items,
        Func<TestItem, string>      keySelector,
        IEqualityComparer<string>?  comparer    = null,
        KeyedItemOptions            options     = default);
    
    TUut Uut { get; }
    
    IEqualityComparer<string> UutComparer { get; }
    
    KeyedItemOptions UutOptions { get; }
}
