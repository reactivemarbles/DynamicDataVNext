using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

[TestFixture]
public partial class MergeRangeTests
{
    [Test]
    public void WhenCacheHasBeenDisposed_ThrowsException()
    {
        var uut = new ObservableCache<string, TestItem>(TestItem.SelectKey);
        
        uut.Dispose();
        
        var result = uut.Invoking(uut => uut.MergeRange(Enumerable.Empty<TestItem>()))
            .Should().Throw<ObjectDisposedException>()
            .Which;
            
        Console.WriteLine(result);
    }
}
