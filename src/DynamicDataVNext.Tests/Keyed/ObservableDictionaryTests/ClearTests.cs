namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

[TestFixture]
public partial class ClearTests
{
    [Test]
    public void WhenDictionaryHasBeenDisposed_ThrowsException()
    {
        var uut = new ObservableDictionary<string, int>();
        
        uut.Dispose();
        
        var result = uut.Invoking(uut => uut.Clear())
            .Should().Throw<ObjectDisposedException>()
            .Which;
            
        Console.WriteLine(result);
    }
}
