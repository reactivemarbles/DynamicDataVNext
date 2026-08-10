using System.Diagnostics.CodeAnalysis;


namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
[SuppressMessage("ReSharper", "CollectionNeverUpdated.Local", Justification = "Collection is being tested")]
public class IsReadOnlyTests
{
    [Test]
    public void SetHasBeenDisposed_ReturnsTrue()
    {
        var uut = new ObservableHashSet<int>();
        
        uut.Dispose();
        
        uut.IsReadOnly.Should().BeTrue("mutation is not allowed after disposal");
    }
        
    [Test]
    public void SetHasNotBeenDisposed_ReturnsFalse()
    {
        var uut = new ObservableHashSet<int>();
        
        uut.IsReadOnly.Should().BeFalse("the set could still be mutated");
    }
}
