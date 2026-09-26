using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class RemoveTests
{
    [TestFixture]
    public partial class ForItem
    {
        [Test]
        public void WhenDictionaryHasBeenDisposed_ThrowsException()
        {
            var item = new TestItem() { Key = "1" };

            var uut = new ObservableCache<string, TestItem>(
                items:          new[] { item },
                keySelector:    TestItem.SelectKey);
        
            uut.Dispose();
        
            var result = uut.Invoking(uut => uut.Remove(item))
                .Should().Throw<ObjectDisposedException>()
                .Which;
            
            Console.WriteLine(result);
        }
    }
}
