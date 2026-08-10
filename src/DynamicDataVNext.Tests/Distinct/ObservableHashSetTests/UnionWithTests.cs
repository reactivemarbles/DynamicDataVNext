namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public partial class UnionWithTests
{
    [Test]
    public void WhenOtherIsNull_ThrowsException()
    {
        var uut = new ObservableHashSet<int>();
        
        var result = uut.Invoking(uut => uut.UnionWith(null!))
            .Should().Throw<ArgumentNullException>()
            .WithParameterName("other")
            .Which;
            
        Console.WriteLine(result);
    }
    
    [Test]
    public void WhenSetHasBeenDisposed_ThrowsException()
    {
        var uut = new ObservableHashSet<int>();
        
        uut.Dispose();
        
        var result = uut.Invoking(uut => uut.UnionWith(Array.Empty<int>()))
            .Should().Throw<ObjectDisposedException>()
            .Which;
            
        Console.WriteLine(result);
    }
}
