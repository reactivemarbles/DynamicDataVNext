using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

[TestFixture]
public partial class ResetTests
{
    [Test]
    public void WhenDictionaryHasBeenDisposed_ThrowsException()
    {
        var uut = new ObservableCache<string, TestItem>(TestItem.SelectKey);
        
        uut.Dispose();
        
        var result = uut.Invoking(uut => uut.Reset(Array.Empty<TestItem>()))
            .Should().Throw<ObjectDisposedException>()
            .Which;
                
        Console.WriteLine(result);
    }
}
