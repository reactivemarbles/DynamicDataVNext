using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

[TestFixture]
public partial class ClearTests
{
    [Test]
    public void WhenCAcheHasBeenDisposed_ThrowsException()
    {
        var uut = new ObservableCache<string, TestItem>(TestItem.SelectKey);
        
        uut.Dispose();
        
        var result = uut.Invoking(uut => uut.Clear())
            .Should().Throw<ObjectDisposedException>()
            .Which;
            
        Console.WriteLine(result);
    }
}
