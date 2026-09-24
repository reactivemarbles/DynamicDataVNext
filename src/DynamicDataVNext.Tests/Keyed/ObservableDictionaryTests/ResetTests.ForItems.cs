namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class ResetTests
{
    [TestFixture]
    public partial class ForItems
    {
        [Test]
        public void WhenDictionaryHasBeenDisposed_ThrowsException()
        {
            var uut = new ObservableDictionary<string, int>();
            
            uut.Dispose();
            
            var result = uut.Invoking(uut => uut.Reset(Array.Empty<KeyValuePair<string, int>>()))
                .Should().Throw<ObjectDisposedException>()
                .Which;
                
            Console.WriteLine(result);
        }
    }
}
