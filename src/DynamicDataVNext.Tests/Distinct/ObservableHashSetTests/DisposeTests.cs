namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public class DisposeTests
{
    [Test]
    public void WhenChangeStreamSourceHasSubscribers_SubscribersReceiveCompletion()
    {
        var uut = new ObservableHashSet<int>();
        
        using var uutSubscription = uut.ChangeStream.Source
            .RecordValues(out var uutResults);

        uutResults.HasFinalized.Should().BeFalse("the set has not been disposed");
        
        uut.Dispose();
        
        uutResults.Error.Should().BeNull("no errors should have occurred");
        uutResults.HasCompleted.Should().BeTrue("the set has been disposed");
        uutResults.RecordedValues.Should().BeEmpty("no changes were made to the set");
    }

    [Test]
    public void WhenCollectionChangedHasSubscribers_SubscribersReceiveCompletion()
    {
        var uut = new ObservableHashSet<int>();
        
        using var collectionChangedSubscription = uut.CollectionChanged
            .RecordValues(out var collectionChangedResults);

        collectionChangedResults.HasFinalized.Should().BeFalse("the set has not been disposed");
        
        uut.Dispose();
        
        collectionChangedResults.Error.Should().BeNull("no errors should have occurred");
        collectionChangedResults.HasCompleted.Should().BeTrue("the set has been disposed");
        collectionChangedResults.RecordedValues.Should().BeEmpty("no changes were made to the set");
    }
    
    [Test]
    public void WhenSetHasBeenDisposed_DoesNothing()
    {
        var items = new[] { 1, 2, 3 };
        
        var uut = new ObservableHashSet<int>(items: items);
        
        using var collectionChangedSubscription = uut.CollectionChanged
            .RecordValues(out var collectionChangedResults);

        uut.Dispose();
        collectionChangedResults.ClearNotifications();
        
        uut.Invoking(uut => uut.Dispose())
            .Should().NotThrow();
        
        uut.Should().BeEquivalentTo(items, "the set should not have been changed");
        
        collectionChangedResults.RecordedNotifications.Should().BeEmpty("no notifications should have been published");
    }
}
