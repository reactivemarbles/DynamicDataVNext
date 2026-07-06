using System;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public class SuspendNotificationsTests
{
    [Test]
    public void WhenNotificationsAreSuspended_ThrowsException()
    {
        var uut = new ObservableHashSet<int>();
        
        using var suspension = uut.SuspendNotifications();
        
        var result = uut.Invoking(uut => uut.SuspendNotifications())
            .Should().Throw<InvalidOperationException>("nested suspensions are not supported")
            .Which;
        
        Console.WriteLine(result);
    }
    
    [Test]
    public void WhenSetIsDisposedDuringSuspension_SuspensionDisposalDoesNotThrow()
    {
        var uut = new ObservableHashSet<int>();
        
        var result = uut.SuspendNotifications();
        
        uut.Dispose();
        
        result.Invoking(result => result.Dispose())
            .Should().NotThrow("object disposal should never throw");
    }
    
    [Test]
    public void WhenSuspensionHasBeenDisposed_SuspensionDisposalDoesNothing()
    {
        var items = new[] { 1, 2, 3 };
        
        var uut = new ObservableHashSet<int>(items);
        
        var result = uut.SuspendNotifications();
        
        using var subscription = uut.ChangeStream.Source
            .RecordValues(out var observer);
        
        result.Dispose();
        observer.ClearNotifications();
        
        result.Dispose();
        
        observer.RecordedNotifications.Should().BeEmpty("no notifications should have been published");

        uut.Should().BeEquivalentTo(items, "the set should not have been changed");
    }
    
    // Remaining scenarios are covered by tests for mutation methods (.Add(), .Clear(), etc.)
}
