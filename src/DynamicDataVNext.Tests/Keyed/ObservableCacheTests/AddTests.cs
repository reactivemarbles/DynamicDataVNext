using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

[TestFixture]
public partial class AddTests
{
    [Test]
    public void WhenCacheHasBeenDisposed_ThrowsException()
    {
        var uut = new ObservableCache<string, TestItem>(TestItem.SelectKey);
            
        uut.Dispose();
            
        var result = uut.Invoking(uut => uut.Add(new() { Key = "1" }))
            .Should().Throw<ObjectDisposedException>()
            .Which;
                
        Console.WriteLine(result);
    }
}
