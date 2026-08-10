namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public partial class RemoveTests
{
    [Test]
    public void WhenSetHasBeenDisposed_ThrowsException()
    {
        var uut = new ObservableHashSet<int>(new[] { 1 });
        
        uut.Dispose();
        
        var result = uut.Invoking(uut => uut.Remove(1))
            .Should().Throw<ObjectDisposedException>()
            .Which;
            
        Console.WriteLine(result);
    }
}
