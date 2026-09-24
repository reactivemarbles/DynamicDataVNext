namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class AddRangeTests
{
    [TestFixture]
    public partial class ForValuesAndKeySelector
    {
        [Test]
        public void WhenDictionaryHasBeenDisposed_ThrowsException()
        {
            var uut = new ObservableDictionary<string, int>();
        
            uut.Dispose();
        
            var result = uut.Invoking(uut => uut.AddRange(
                    values:         Enumerable.Empty<int>(),
                    keySelector:    static value => value.ToString()))
                .Should().Throw<ObjectDisposedException>()
                .Which;
            
            Console.WriteLine(result);
        }
    }
}
