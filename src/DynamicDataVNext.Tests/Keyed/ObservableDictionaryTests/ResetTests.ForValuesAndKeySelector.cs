namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class ResetTests
{
    [TestFixture]
    public partial class ForValuesAndKeySelector
    {
        [Test]
        public void WhenDictionaryHasBeenDisposed_ThrowsException()
        {
            var uut = new ObservableDictionary<string, int>();
            
            uut.Dispose();
            
            var result = uut.Invoking(uut => uut.Reset(
                    values:         Array.Empty<int>(),
                    keySelector:    static value => value.ToString()))
                .Should().Throw<ObjectDisposedException>()
                .Which;
                
            Console.WriteLine(result);
        }
    }
}
