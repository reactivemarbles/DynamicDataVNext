namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class AddTests
{
    [TestFixture]
    public partial class ForKeyValuePair
    {
        [Test]
        public void WhenDictionaryHasBeenDisposed_ThrowsException()
        {
            var uut = new ObservableDictionary<string, int>();
            
            uut.Dispose();
            
            var result = uut.Invoking(uut => uut.Add(new KeyValuePair<string, int>(
                    key:    "1",
                    value:  1)))
                .Should().Throw<ObjectDisposedException>()
                .Which;
                
            Console.WriteLine(result);
        }
    }
}
