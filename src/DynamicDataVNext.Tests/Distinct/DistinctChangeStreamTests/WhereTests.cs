using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

[TestFixture]
public class WhereTests
{
    [Test]
    public void WhenAdditionContainsFirstMatchingItem_NotificationPropagatesAsReset()
    {
        using var source = new Subject<DistinctChangeSet<int>>(); 
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Options     = new() { ItemsAreMutable = true },
            Source      = Observable.Concat(
                Observable.Return(DistinctChangeSet.CreateForReset(new[] { 1, 3, 5 })),
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
        using var source = new Subject<DistinctChangeSet<int>>(); 
        
        var items = new[]
        {
            1, 2, 3
        };
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Options     = new() { ItemsAreMutable = true },
            Source      = Observable.Concat(
                Observable.Return(DistinctChangeSet.CreateForReset(items)),
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

    public class WhenPredicateThrows_Item
    {
        public required int Id { get; init; }
        
        public TestException? Error { get; init; }
    }

    public static readonly IReadOnlyList<TestCaseData> WhenPredicateThrows_TestCases
        = new[]
        {
            new TestCaseData(
                    new[] {  new WhenPredicateThrows_Item() { Id = 1, Error = new TestException() } },
                    new DistinctItemOptions() { ItemsAreMutable = false })
                .SetName("{m})(Single immutable item, excluded by predicate)"),
            new TestCaseData(
                    new[] {  new WhenPredicateThrows_Item() { Id = 2, Error = new TestException() } },
                    new DistinctItemOptions() { ItemsAreMutable = false })
                .SetName("{m})(Single immutable item, matching predicate)"),
            new TestCaseData(
                    new[]
                    {
                        new WhenPredicateThrows_Item() { Id = 1 },
                        new WhenPredicateThrows_Item() { Id = 2, Error = new TestException() },
                        new WhenPredicateThrows_Item() { Id = 3 },
                    },
                    new DistinctItemOptions() { ItemsAreMutable = false })
                .SetName("{m})(Multiple immutable items)"),
            new TestCaseData(
                    new[] {  new WhenPredicateThrows_Item() { Id = 1, Error = new TestException() } },
                    new DistinctItemOptions() { ItemsAreMutable = true })
                .SetName("{m})(Single mutable item, excluded by predicate)"),
            new TestCaseData(
                    new[] {  new WhenPredicateThrows_Item() { Id = 2, Error = new TestException() } },
                    new DistinctItemOptions() { ItemsAreMutable = true })
                .SetName("{m})(Single mutable item, matching predicate)"),
            new TestCaseData(
                    new[]
                    {
                        new WhenPredicateThrows_Item() { Id = 1 },
                        new WhenPredicateThrows_Item() { Id = 2, Error = new TestException() },
                        new WhenPredicateThrows_Item() { Id = 3 },
                    },
                    new DistinctItemOptions() { ItemsAreMutable = true })
                .SetName("{m})(Multiple mutable items)")
        };
    
    [Test]
    public void WhenRemovalContainsLastMatchingItem_NotificationPropagatesAsClear()
    {
        using var source = new Subject<DistinctChangeSet<int>>(); 
        
        var items = new[]
        {
            1, 2, 3, 4, 5
        };
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Options     = new() { ItemsAreMutable = true },
            Source      = Observable.Concat(
                Observable.Return(DistinctChangeSet.CreateForReset(items)),
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

    [TestCaseSource(nameof(WhenPredicateThrows_TestCases))]
    public void WhenPredicateThrows_ErrorPropagates(
            IReadOnlyList<WhenPredicateThrows_Item> items,
            DistinctItemOptions                     options)
    {
        using var source = new Subject<DistinctChangeSet<WhenPredicateThrows_Item>>(); 
        
        var stream = new DistinctChangeStream<WhenPredicateThrows_Item>()
        {
            Comparer    = EqualityComparer<WhenPredicateThrows_Item>.Default,
            Source      = source
        };
        
        using var subscription = stream.Where(static item => (item.Error is null)
                    ? (item.Id % 2) is 0
                    : throw item.Error)
            .ValidateChangeSets()
            .RecordItems(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().BeEmpty("there were no initial items in the collection");

        source.OnNext(DistinctChangeSet.CreateForReset(items));

        var expectedError = items
            .Select(static item => item.Error)
            .First(static error => error is not null);
        results.Error.Should().Be(expectedError, "consumer errors should propagate downstream");
        results.RecordedChangeSets.Should().BeEmpty("an error occurred during processing of changes");
    }    

    public static readonly IReadOnlyList<TestCaseData> WhenSourceCompletesAsynchronously_TestCases
        = new[]
        {
            new TestCaseData(new DistinctItemOptions() { ItemsAreMutable = false})
                .SetName("{m}(Immutable Items)"),
            new TestCaseData(new DistinctItemOptions() { ItemsAreMutable = true })
                .SetName("{m}(Mutable Items)")
        };
    
    [TestCaseSource(nameof(WhenSourceCompletesAsynchronously_TestCases))]
    public void WhenSourceCompletesAsynchronously_CompletionPropagates(DistinctItemOptions options)
    {
        using var streamSource = new Subject<DistinctChangeSet<int>>();
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Options     = options,
            Source      = streamSource
        };
        
        using var subscription = stream.Where(IsEven)
            .ValidateChangeSets()
            .RecordItems(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().BeEmpty("there were no initial items in the collection");
        
        streamSource.OnCompleted();
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeTrue("the source cannot publish any further notifications");
        results.RecordedChangeSets.Should().BeEmpty("no change operations were performed");
    }

    public static readonly IReadOnlyList<TestCaseData> WhenSourceCompletesImmediately_TestCases
        = new[]
        {
            new TestCaseData(new DistinctItemOptions() { ItemsAreMutable = false})
                .SetName("{m}(Immutable Items)"),
            new TestCaseData(new DistinctItemOptions() { ItemsAreMutable = true })
                .SetName("{m}(Mutable Items)")
        };
    
    [TestCaseSource(nameof(WhenSourceCompletesImmediately_TestCases))]
    public void WhenSourceCompletesImmediately_CompletionPropagates(DistinctItemOptions options)
    {
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Options     = options,
            Source      = Observable.Empty<DistinctChangeSet<int>>()
        };
        
        using var subscription = stream.Where(IsEven)
            .ValidateChangeSets()
            .RecordItems(out var results);

        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeTrue("the source cannot publish any further notifications");
        results.RecordedChangeSets.Should().BeEmpty("there were no initial items in the collection");
    }

    public static readonly IReadOnlyList<TestCaseData> WhenSourceFailsAsynchronously_TestCases
        = new[]
        {
            new TestCaseData(new DistinctItemOptions() { ItemsAreMutable = false})
                .SetName("{m}(Immutable Items)"),
            new TestCaseData(new DistinctItemOptions() { ItemsAreMutable = true })
                .SetName("{m}(Mutable Items)")
        };
    
    [TestCaseSource(nameof(WhenSourceFailsAsynchronously_TestCases))]
    public void WhenSourceFailsAsynchronously_ErrorPropagates(DistinctItemOptions options)
    {
        using var streamSource = new Subject<DistinctChangeSet<int>>();
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Options     = options,
            Source      = streamSource
        };
        
        using var subscription = stream.Where(IsEven)
            .ValidateChangeSets()
            .RecordItems(out var results);

        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().BeEmpty("there were no initial items in the collection");
        
        var error = new TestException();
        
        streamSource.OnError(error);
        
        results.Error.Should().BeSameAs(error, "errors should propagate to subscribers");
        results.RecordedChangeSets.Should().BeEmpty("no change operation were performed");
    }

    public static readonly IReadOnlyList<TestCaseData> WhenSourceFailsImmediately_TestCases
        = new[]
        {
            new TestCaseData(new DistinctItemOptions() { ItemsAreMutable = false})
                .SetName("{m}(Immutable Items)"),
            new TestCaseData(new DistinctItemOptions() { ItemsAreMutable = true })
                .SetName("{m}(Mutable Items)")
        };
    
    [TestCaseSource(nameof(WhenSourceFailsImmediately_TestCases))]
    public void WhenSourceFailsImmediately_ErrorPropagates(DistinctItemOptions options)
    {
        var error = new TestException();
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Options     = options,
            Source      = Observable.Throw<DistinctChangeSet<int>>(error)
        };
        
        using var subscription = stream.Where(IsEven)
            .ValidateChangeSets()
            .RecordItems(out var results);
        
        results.Error.Should().BeSameAs(error, "errors should propagate to subscribers");
        results.RecordedChangeSets.Should().BeEmpty("an error occurred during initial subscription");
    }

    private static TestCaseData WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
            DistinctItemOptions     options,
            IReadOnlyList<int>      initialItems,
            DistinctChangeSet<int>  changeSet,
            IReadOnlyList<int>      finalItems)
        => new(options, initialItems, changeSet, finalItems);

    public static readonly IReadOnlyList<TestCaseData> WhenSourcePublishesChangesForMatchingItems_TestCases
        = new[]
        {
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 2 }),
                    finalItems:     new[] { 2 })
                .SetName("{m}(Initial Reset, Single item, Immutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 1, 2, 3, 4 }),
                    finalItems:     new[] { 2, 4 })
                .SetName("{m}(Initial Reset, Multiple items, Immutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 2 },
                    changeSet:      DistinctChangeSet.CreateForReset(
                        removedItems:   new[] { 2 },
                        addedItems:     new[] { 4 }),
                    finalItems:     new[] { 4 })
                .SetName("{m}(Subsequent Reset, Single item, Immutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 1, 2, 3, 4 },
                    changeSet:      DistinctChangeSet.CreateForReset(
                        removedItems:   new[] { 1, 2, 3, 4 },
                        addedItems:     new[] { 5, 6, 7, 8 }),
                    finalItems:     new[] { 6, 8 })
                .SetName("{m}(Subsequent Reset, Multiple items, Immutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 2 },
                    changeSet:      DistinctChangeSet.CreateForClear(new[] { 2 }),
                    finalItems:     Array.Empty<int>())
                .SetName("{m}(Clear, Single item, Immutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 1, 2, 3, 4 },
                    changeSet:      DistinctChangeSet.CreateForClear(new[] { 1, 2, 3, 4 }),
                    finalItems:     Array.Empty<int>())
                .SetName("{m}(Clear, Multiple items, Immutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 2 },
                    changeSet:      DistinctChangeSet.CreateForAddition(4),
                    finalItems:     new[] { 2, 4 })
                .SetName("{m}(Add, Single item, Immutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 1, 2, 3, 4 },
                    changeSet:      DistinctChangeSet.CreateForAdditions(new[] { 5, 6, 7, 8 }),
                    finalItems:     new[] { 2, 4, 6, 8 })
                .SetName("{m}(Add, Multiple items, Immutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 2, 4 },
                    changeSet:      DistinctChangeSet.CreateForRemoval(2),
                    finalItems:     new[] { 4 })
                .SetName("{m}(Remove, Single item, Immutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 1, 2, 3, 4, 5, 6, 7, 8 },
                    changeSet:      DistinctChangeSet.CreateForRemovals(new[] { 1, 2, 3, 4 }),
                    finalItems:     new[] { 6, 8 })
                .SetName("{m}(Remove, Multiple items, Immutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 2 }),
                    finalItems:     new[] { 2 })
                .SetName("{m}(Initial Reset, Single item, Mutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 1, 2, 3, 4 }),
                    finalItems:     new[] { 2, 4 })
                .SetName("{m}(Initial Reset, Multiple items, Mutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 2 },
                    changeSet:      DistinctChangeSet.CreateForReset(
                        removedItems:   new[] { 2 },
                        addedItems:     new[] { 4 }),
                    finalItems:     new[] { 4 })
                .SetName("{m}(Subsequent Reset, Single item, Mutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 1, 2, 3, 4 },
                    changeSet:      DistinctChangeSet.CreateForReset(
                        removedItems:   new[] { 1, 2, 3, 4 },
                        addedItems:     new[] { 5, 6, 7, 8 }),
                    finalItems:     new[] { 6, 8 })
                .SetName("{m}(Subsequent Reset, Multiple items, Mutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 2 },
                    changeSet:      DistinctChangeSet.CreateForClear(new[] { 2 }),
                    finalItems:     Array.Empty<int>())
                .SetName("{m}(Clear, Single item, Mutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 1, 2, 3, 4 },
                    changeSet:      DistinctChangeSet.CreateForClear(new[] { 1, 2, 3, 4 }),
                    finalItems:     Array.Empty<int>())
                .SetName("{m}(Clear, Multiple items, Mutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 2 },
                    changeSet:      DistinctChangeSet.CreateForAddition(4),
                    finalItems:     new[] { 2, 4 })
                .SetName("{m}(Add, Single item, Mutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 1, 2, 3, 4 },
                    changeSet:      DistinctChangeSet.CreateForAdditions(new[] { 5, 6, 7, 8 }),
                    finalItems:     new[] { 2, 4, 6, 8 })
                .SetName("{m}(Add, Multiple items, Mutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 2, 4 },
                    changeSet:      DistinctChangeSet.CreateForRemoval(2),
                    finalItems:     new[] { 4 })
                .SetName("{m}(Remove, Single item, Mutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 1, 2, 3, 4, 5, 6, 7, 8 },
                    changeSet:      DistinctChangeSet.CreateForRemovals(new[] { 1, 2, 3, 4 }),
                    finalItems:     new[] { 6, 8 })
                .SetName("{m}(Remove, Multiple items, Mutable)")
        };
    
    [TestCaseSource(nameof(WhenSourcePublishesChangesForMatchingItems_TestCases))]
    public void WhenSourcePublishesChangesForMatchingItems_NotificationPropagates(
        DistinctItemOptions     options,
        IReadOnlyList<int>      initialItems,
        DistinctChangeSet<int>  changeSet,
        IReadOnlyList<int>      finalItems)
    {
        using var source = new Subject<DistinctChangeSet<int>>(); 
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Options     = options,
            Source      = Observable.Concat(
                Observable.Return(DistinctChangeSet.CreateForReset(initialItems)),
                source)
        };
        
        using var subscription = stream.Where(IsEven)
            .ValidateChangeSets()
            .RecordItems(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        if (initialItems.Any(IsEven))
        {
            results.RecordedChangeSets.Should().ContainSingle("there were initial matching items in the collection");
            results.RecordedItems.Should().BeEquivalentTo(
                expectation:    initialItems.Where(IsEven),
                config:         options => options.WithoutStrictOrdering(),
                because:        "all changes for items matching the predicate should propagate downstream");
            results.ClearNotifications();
        }
        else
            results.RecordedChangeSets.Should().BeEmpty("there were no initial matching items in the collection");

        source.OnNext(changeSet);

        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
        results.RecordedItems.Should().BeEquivalentTo(
            expectation:    finalItems,
            config:         options => options.WithoutStrictOrdering(),
            because:        "all changes for items matching the predicate should propagate downstream");
    }

    private static TestCaseData WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
            DistinctItemOptions     options,
            IReadOnlyList<int>      initialItems,
            DistinctChangeSet<int>  changeSet)
        => new(options, initialItems, changeSet);

    public static readonly IReadOnlyList<TestCaseData> WhenSourcePublishesChangesNotForMatchingItems_TestCases
        = new[]
        {
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.Empty<int>())
                .SetName("{m}(Empty Change Set, Empty collection, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new [] { 1 },
                    changeSet:      DistinctChangeSet.Empty<int>())
                .SetName("{m}(Empty Change Set, Single excluded item, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new [] { 2 },
                    changeSet:      DistinctChangeSet.Empty<int>())
                .SetName("{m}(Empty Change Set, Single included item, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new [] { 1, 2, 3, 4 },
                    changeSet:      DistinctChangeSet.Empty<int>())
                .SetName("{m}(Empty Change Set, Multiple items, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 1 }))
                .SetName("{m}(Initial Reset, Single item, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 1, 3, 5 }))
                .SetName("{m}(Initial Reset, Multiple items, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 1 },
                    changeSet:      DistinctChangeSet.CreateForReset(
                        removedItems:   new[] { 1 },
                        addedItems:     new[] { 3 }))
                .SetName("{m}(Subsequent Reset, Single item, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 1, 3, 5 },
                    changeSet:      DistinctChangeSet.CreateForReset(
                        removedItems:   new[] { 1, 3, 5 },
                        addedItems:     new[] { 7, 9, 11 }))
                .SetName("{m}(Subsequent Reset, Multiple items, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 1 },
                    changeSet:      DistinctChangeSet.CreateForClear(new[] { 1 }))
                .SetName("{m}(Clear, Single item, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 1, 3, 5 },
                    changeSet:      DistinctChangeSet.CreateForClear(new[] { 1, 3, 5 }))
                .SetName("{m}(Clear, Multiple items, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 2 },
                    changeSet:      DistinctChangeSet.CreateForAddition(1))
                .SetName("{m}(Add, Single item, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 1, 2, 3, 4 },
                    changeSet:      DistinctChangeSet.CreateForAdditions(new[] { 5, 7, 9 }))
                .SetName("{m}(Add, Multiple items, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 1, 2 },
                    changeSet:      DistinctChangeSet.CreateForRemoval(1))
                .SetName("{m}(Remove, Single item, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 1, 2, 3, 4, 5, 7, 9 },
                    changeSet:      DistinctChangeSet.CreateForRemovals(new[] { 1, 3, 5 }))
                .SetName("{m}(Remove, Multiple items, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.Empty<int>())
                .SetName("{m}(Empty Change Set, Empty collection, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new [] { 1 },
                    changeSet:      DistinctChangeSet.Empty<int>())
                .SetName("{m}(Empty Change Set, Single excluded item, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new [] { 2 },
                    changeSet:      DistinctChangeSet.Empty<int>())
                .SetName("{m}(Empty Change Set, Single included item, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new [] { 1, 2, 3, 4 },
                    changeSet:      DistinctChangeSet.Empty<int>())
                .SetName("{m}(Empty Change Set, Multiple items, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 1 }))
                .SetName("{m}(Initial Reset, Single item, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 1, 3, 5 }))
                .SetName("{m}(Initial Reset, Multiple items, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 1 },
                    changeSet:      DistinctChangeSet.CreateForReset(
                        removedItems:   new[] { 1 },
                        addedItems:     new[] { 3 }))
                .SetName("{m}(Subsequent Reset, Single item, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 1, 3, 5 },
                    changeSet:      DistinctChangeSet.CreateForReset(
                        removedItems:   new[] { 1, 3, 5 },
                        addedItems:     new[] { 7, 9, 11 }))
                .SetName("{m}(Subsequent Reset, Multiple items, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 1 },
                    changeSet:      DistinctChangeSet.CreateForClear(new[] { 1 }))
                .SetName("{m}(Clear, Single item, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 1, 3, 5 },
                    changeSet:      DistinctChangeSet.CreateForClear(new[] { 1, 3, 5 }))
                .SetName("{m}(Clear, Multiple items, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 2 },
                    changeSet:      DistinctChangeSet.CreateForAddition(1))
                .SetName("{m}(Add, Single item, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 1, 2, 3, 4 },
                    changeSet:      DistinctChangeSet.CreateForAdditions(new[] { 5, 7, 9 }))
                .SetName("{m}(Add, Multiple items, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 1, 2 },
                    changeSet:      DistinctChangeSet.CreateForRemoval(1))
                .SetName("{m}(Remove, Single item, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 1, 2, 3, 4, 5, 7, 9 },
                    changeSet:      DistinctChangeSet.CreateForRemovals(new[] { 1, 3, 5 }))
                .SetName("{m}(Remove, Multiple items, Mutable)")
        };
    
    [TestCaseSource(nameof(WhenSourcePublishesChangesNotForMatchingItems_TestCases))]
    public void WhenSourcePublishesChangesNotForMatchingItems_NotificationPropagates(
        DistinctItemOptions     options,
        IReadOnlyList<int>      initialItems,
        DistinctChangeSet<int>  changeSet)
    {
        using var source = new Subject<DistinctChangeSet<int>>(); 
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Options     = options,
            Source      = Observable.Concat(
                Observable.Return(DistinctChangeSet.CreateForReset(initialItems)),
                source)
        };
        
        using var subscription = stream.Where(IsEven)
            .ValidateChangeSets()
            .RecordItems(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        if (initialItems.Any(IsEven))
        {
            results.RecordedChangeSets.Should().ContainSingle("there were initial matching items in the collection");
            results.RecordedItems.Should().BeEquivalentTo(
                expectation:    initialItems.Where(IsEven),
                config:         options => options.WithoutStrictOrdering(),
                because:        "all changes for items matching the predicate should propagate downstream");
            results.ClearNotifications();
        }
        else
            results.RecordedChangeSets.Should().BeEmpty("there were no initial matching items in the collection");

        source.OnNext(changeSet);

        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().BeEmpty("no changes were performed for matching items");
    }

    private static bool IsEven(int item)
        => (item % 2) == 0;
}
