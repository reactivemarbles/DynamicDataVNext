namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

[TestFixture]
public partial class RefreshTests
{
    [Test]
    public void WhenDictionaryHasBeenDisposed_ThrowsException()
    {
        var uut = new ObservableDictionary<string, int>(items: new[]
        {
            new KeyValuePair<string, int>("1", 1)
        });
        
        uut.Dispose();
        
        var result = uut.Invoking(uut => uut.Refresh("1"))
            .Should().Throw<ObjectDisposedException>()
            .Which;
            
        Console.WriteLine(result);
    }
}
