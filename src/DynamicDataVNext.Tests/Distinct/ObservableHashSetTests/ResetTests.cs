using System;
using System.Collections.Generic;
using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public partial class ResetTests
{
    [Test]
    public void WhenItemsIsNull_ThrowsException()
    {
        var uut = new ObservableHashSet<int>();
        
        var result = uut.Invoking(uut => uut.Reset(null!))
            .Should().Throw<ArgumentNullException>()
            .WithParameterName("items")
            .Which;
            
        Console.WriteLine(result);
    }
    
    [Test]
    public void WhenSetHasBeenDisposed_ThrowsException()
    {
        var uut = new ObservableHashSet<int>();
        
        uut.Dispose();
        
        var result = uut.Invoking(uut => uut.Reset(Array.Empty<int>()))
            .Should().Throw<ObjectDisposedException>()
            .Which;
            
        Console.WriteLine(result);
    }
}
