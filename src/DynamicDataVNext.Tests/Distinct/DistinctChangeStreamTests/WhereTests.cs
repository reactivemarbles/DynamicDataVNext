using System.Collections.Generic;
using System.Linq;

using ReactiveUI.Primitives.Signals;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

[TestFixture]
public partial class WhereTests
{
    [Test]
    public void WhenAdditionContainsFirstMatchingItem_NotificationPropagatesAsReset()
    {
        using var source = new Signal<DistinctChangeSet<int>>(); 
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Options     = new() { ItemsAreMutable = true },
            Source      = Signal.Concat(
                Signal.Return(DistinctChangeSet.CreateForReset(new[] { 1, 3, 5 })),
                source)
        };
        
        using var subscription = stream.Where(IsEven)
            .ValidateChangeSets()
            .RecordItems(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().BeEmpty("none of the initial items matched the predicate");

        var additionalItems = new[] { 6, 7, 8, 9 };
        source.OnNext(DistinctChangeSet.CreateForAdditions(additionalItems));

        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
        results.RecordedItems.Should().BeEquivalentTo(
            expectation:    additionalItems.Where(IsEven),
            config:         options => options.WithoutStrictOrdering(),
            because:        "all changes for items matching the predicate should propagate downstream");
        results.RecordedChangeSets.ElementAt(0).Type.Should().Be(ChangeSetType.Reset, "adding items to an empty collection should result in a reset");
    }
    
    [Test]
    public void WhenItemsAreMutable_PredicateIsOnlyInvokedOncePerItem()
    {
        using var source = new Signal<DistinctChangeSet<int>>(); 
        
        var items = new[]
        {
            1, 2, 3
        };
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Options     = new() { ItemsAreMutable = true },
            Source      = Signal.Concat(
                Signal.Return(DistinctChangeSet.CreateForReset(items)),
                source)
        };
        
        var predicateInvocations = new List<int>();

        using var subscription = stream.Where(item =>
            {
                predicateInvocations.Add(item);
                return IsEven(item);
            })
            .ValidateChangeSets()
            .RecordItems(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().ContainSingle("there were initial matching items in the collection");
        results.RecordedItems.Should().BeEquivalentTo(
            expectation:    items.Where(IsEven),
            config:         options => options.WithoutStrictOrdering(),
            because:        "all changes for items matching the predicate should propagate downstream");
        results.ClearNotifications();

        predicateInvocations.Should().BeEquivalentTo(
            expectation:    items,
            config:         options => options.WithoutStrictOrdering(),
            because:        "the predicate should have been invoked for each added item");
        predicateInvocations.Clear();
        
        source.OnNext(DistinctChangeSet.CreateForClear(items));

        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
        results.RecordedItems.Should().BeEmpty("all changes for items matching the predicate should propagate downstream");
        
        predicateInvocations.Should().BeEmpty("the predicate should only be invoked once, per-item");
    }

    [Test]
    public void WhenRemovalContainsLastMatchingItem_NotificationPropagatesAsClear()
    {
        using var source = new Signal<DistinctChangeSet<int>>(); 
        
        var items = new[]
        {
            1, 2, 3, 4, 5
        };
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Options     = new() { ItemsAreMutable = true },
            Source      = Signal.Concat(
                Signal.Return(DistinctChangeSet.CreateForReset(items)),
                source)
        };
        
        using var subscription = stream.Where(IsEven)
            .ValidateChangeSets()
            .RecordItems(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().ContainSingle("there were initial matching items in the collection");
        results.RecordedItems.Should().BeEquivalentTo(
            expectation:    items.Where(IsEven),
            config:         options => options.WithoutStrictOrdering(),
            because:        "all changes for items matching the predicate should propagate downstream");
        results.ClearNotifications();

        source.OnNext(DistinctChangeSet.CreateForRemovals(new[] { 2, 3, 4 }));

        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
        results.RecordedItems.Should().BeEmpty("all matching items were removed from the collection");
        results.RecordedChangeSets.ElementAt(0).Type.Should().Be(ChangeSetType.Clear, "removing all items from a collection should result in a clear");
    }

    private static bool IsEven(int item)
        => (item % 2) == 0;
}
