namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public partial class ExceptWithTests
{
    [Test]
    public void WhenOtherIsNull_ThrowsException()
    {
        var uut = new ObservableHashSet<int>();
        
        var result = uut.Invoking(uut => uut.ExceptWith(null!))
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
        
        var result = uut.Invoking(uut => uut.ExceptWith(Array.Empty<int>()))
            .Should().Throw<ObjectDisposedException>()
            .Which;
            
        Console.WriteLine(result);
    }
}
