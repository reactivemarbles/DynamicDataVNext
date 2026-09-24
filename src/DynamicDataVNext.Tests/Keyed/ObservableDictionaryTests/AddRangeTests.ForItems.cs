namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class AddRangeTests
{
    [TestFixture]
    public partial class ForItems
    {
        [Test]
        public void WhenDictionaryHasBeenDisposed_ThrowsException()
        {
            var uut = new ObservableDictionary<string, int>();
        
            uut.Dispose();
        
            var result = uut.Invoking(uut => uut.AddRange(Enumerable.Empty<KeyValuePair<string, int>>()))
                .Should().Throw<ObjectDisposedException>()
                .Which;
            
            Console.WriteLine(result);
        }
    }
}
