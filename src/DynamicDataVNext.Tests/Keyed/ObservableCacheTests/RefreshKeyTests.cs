using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

[TestFixture]
public partial class RefreshKeyTests
{
    [Test]
    public void WhenCacheHasBeenDisposed_ThrowsException()
    {
        var item = new TestItem() { Key = "1" };

        var uut = new ObservableCache<string, TestItem>(
            items:          new[] { item },
            keySelector:    TestItem.SelectKey);
        
        uut.Dispose();
        
        var result = uut.Invoking(uut => uut.RefreshKey(item.Key))
            .Should().Throw<ObjectDisposedException>()
            .Which;
            
        Console.WriteLine(result);
    }
}
