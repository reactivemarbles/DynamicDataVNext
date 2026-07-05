using System;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public partial class AddTests
{
    [Test]
    public void WhenSetHasBeenDisposed_ThrowsException()
    {
        var uut = new ObservableHashSet<int>();
        
        uut.Dispose();
        
        var result = uut.Invoking(uut => uut.Add(1))
            .Should().Throw<ObjectDisposedException>()
            .Which;
            
        Console.WriteLine(result);
    }
}
