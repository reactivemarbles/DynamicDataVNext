namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class IndexerTests
{
    [TestFixture]
    public partial class Set
    {
        [Test]
        public void WhenDictionaryHasBeenDisposed_ThrowsException()
        {
            var uut = new ObservableDictionary<string, int>();
        
            uut.Dispose();
        
            var result = uut.Invoking(uut => uut["1"] = 1)
                .Should().Throw<ObjectDisposedException>()
                .Which;
            
            Console.WriteLine(result);
        }
    }
}
