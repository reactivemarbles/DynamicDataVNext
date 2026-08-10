namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public partial class ClearTests
{
    [Test]
    public void WhenSetHasBeenDisposed_ThrowsException()
    {
        var uut = new ObservableHashSet<int>();
        
        uut.Dispose();
        
        var result = uut.Invoking(uut => uut.Clear())
            .Should().Throw<ObjectDisposedException>()
            .Which;
            
        Console.WriteLine(result);
    }
}
