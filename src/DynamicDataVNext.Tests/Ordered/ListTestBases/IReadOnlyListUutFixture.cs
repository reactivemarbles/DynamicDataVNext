namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public interface IReadOnlyListUutFixture<out TUutFixture, out TUut>
        : IDisposable
    where TUutFixture : IReadOnlyListUutFixture<TUutFixture, TUut>
    where TUut : IReadOnlyList<string?>
{
    static abstract TUutFixture Create(
        IEnumerable<string?>    items,
        OrderedItemOptions      options = default);
    
    TUut Uut { get; }
    
    OrderedItemOptions UutOptions { get; }
}
