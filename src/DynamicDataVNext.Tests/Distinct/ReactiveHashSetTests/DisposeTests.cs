using System.Reactive.Linq;
using System.Reactive.Subjects;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ReactiveHashSetTests;

[TestFixture]
public class DisposeTests
{
    [Test]
    public void Always_UnsubscribesFromSource()
    {
        using var source = new Subject<DistinctChangeSet<int>>();
        
        var uut = new ReactiveHashSet<int>(source);
        
        uut.Dispose();
        
        source.HasObservers.Should().BeFalse("the source subscription should have been disposed");
    }
    
    [Test]
    public void WhenChangeStreamSourceHasSubscribers_SubscribersReceiveCompletion()
    {
        var source = Observable.Never<DistinctChangeSet<int>>();

        var uut = new ReactiveHashSet<int>(source);
        
        using var subscription = uut.ChangeStream.Source
            .RecordValues(out var results);
        
        uut.Dispose();
        
        results.HasCompleted.Should().BeTrue("no further notifications should occur");
    }

    [Test]
    public void WhenCollectionChangedHasSubscribers_SubscribersReceiveCompletion()
    {
        var source = Observable.Never<DistinctChangeSet<int>>();

        var uut = new ReactiveHashSet<int>(source);
        
        using var subscription = uut.CollectionChanged
            .RecordValues(out var results);
        
        uut.Dispose();
        
        results.HasCompleted.Should().BeTrue("no further notifications should occur");
    }
    
    [Test]
    public void WhenSetHasBeenDisposed_DoesNothing()
    {
        var items = new[] { 1, 2, 3 };
        
        var source = Observable.Return(DistinctChangeSet.CreateForReset(addedItems: items));

        var uut = new ReactiveHashSet<int>(source);
        
        using var changeStreamSourceSubscription = uut.ChangeStream.Source
            .RecordValues(out var changeStreamSourceResults);

        using var collectionChangedSubscription = uut.CollectionChanged
            .RecordValues(out var collectionChangedResults);
        
        uut.Dispose();
        
        changeStreamSourceResults.ClearNotifications();
        collectionChangedResults.ClearNotifications();
        
        uut.Dispose();
        
        changeStreamSourceResults.RecordedNotifications.Should().BeEmpty("redundant disposal should do nothing");
        collectionChangedResults.RecordedNotifications.Should().BeEmpty("redundant disposal should do nothing");
        
        uut.Should().BeEquivalentTo(items, options => options.WithoutStrictOrdering(), "disposal should not mutate the set");
    }
}
