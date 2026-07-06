using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

[TestFixture]
public class CountTests
{
    [TestCase(false,    TestName = "{m}(Source is empty)")]
    [TestCase(true,     TestName = "{m}(Source is not empty)")]
    public void Always_PublishesImmediateNotification(bool isSourceEmpty)
    {
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = isSourceEmpty
                ? Observable.Never<DistinctChangeSet<int>>()
                : Observable.Return(DistinctChangeSet.CreateForReset(new[] { 1, 2, 3 }))
                    .Concat(Observable.Never<DistinctChangeSet<int>>())
        };
        
        using var subscription = stream.Count()
            .RecordValues(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedValues.Should().ContainSingle("an initial value should always be published");
    }

    public static readonly IReadOnlyList<TestCaseData> WhenItemsAreAdded_TestCases
        = new[]
        {
            new TestCaseData(new OperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForReset(new[] { 1 }),
                    Items       = Array.Empty<int>()
                })
                .SetName("{m}(Single addition, empty collection)"),
            new TestCaseData(new OperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForReset(new[] { 1, 2, 3 }),
                    Items       = Array.Empty<int>()
                })
                .SetName("{m}(Multiple additions, empty collection)"),
            new TestCaseData(new OperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForAddition(2),
                    Items       = new[] { 1 }
                })
                .SetName("{m}(Single addition, single item in collection)"),
            new TestCaseData(new OperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForAdditions(new[] { 2, 3, 4 }),
                    Items       = new[] { 1 }
                })
                .SetName("{m}(Multiple additions, single item in collection)"),
            new TestCaseData(new OperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForAddition(4),
                    Items       = new[] { 1, 2, 3 }
                })
                .SetName("{m}(Single addition, multiple items in collection)"),
            new TestCaseData(new OperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForAdditions(new[] { 4, 5, 6 }),
                    Items       = new[] { 1, 2, 3 }
                })
                .SetName("{m}(Multiple additions, multiple items in collection)"),
            new TestCaseData(new OperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForUpdate(new DistinctChange<int>[]
                    {
                        new() { Item = 4, Type = DistinctChangeType.Addition },
                        new() { Item = 5, Type = DistinctChangeType.Addition },
                        new() { Item = 4, Type = DistinctChangeType.Removal },
                        new() { Item = 6, Type = DistinctChangeType.Addition },
                        new() { Item = 5, Type = DistinctChangeType.Removal },
                    }),
                    Items       = new[] { 1, 2, 3 }
                })
                .SetName("{m}(More additions than removals)")
        };
    [TestCaseSource(nameof(WhenItemsAreAdded_TestCases))]
    public void WhenItemsAreAdded_ResultIncreases(OperatorTestCase testCase)
    {
        using var streamSource = new Subject<DistinctChangeSet<int>>();
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = Observable.Concat(
                Observable.Return(DistinctChangeSet.CreateForReset(testCase.Items)),
                streamSource)
        };
        
        using var subscription = stream.Count()
            .RecordValues(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedValues.Should().ContainSingle("an initial value should always be published");
        results.RecordedValues.Should().HaveElementAt(0, testCase.Items.Count, "the initial number of items should have been published");
        results.ClearNotifications();
        
        streamSource.OnNext(testCase.ChangeSet);
        
        var additionCount = testCase.ChangeSet.Changes
            .Count(change => change.Type is DistinctChangeType.Addition);
        
        var removalCount = testCase.ChangeSet.Changes
            .Count(change => change.Type is DistinctChangeType.Removal);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedValues.Should().ContainSingle("items were added to the collection");
        results.RecordedValues.Should().HaveElementAt(0, testCase.Items.Count + additionCount - removalCount, "items were added to the collection");
    }

    public static readonly IReadOnlyList<TestCaseData> WhenItemsAreRemoved_TestCases
        = new[]
        {
            new TestCaseData(new OperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForClear(new[] { 1 }),
                    Items       = new[] { 1 }
                })
                .SetName("{m}(Single removal, leaving collection empty)"),
            new TestCaseData(new OperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForClear(new[] { 1, 2, 3 }),
                    Items       = new[] { 1, 2, 3 }
                })
                .SetName("{m}(Multiple removals, leaving collection empty)"),
            new TestCaseData(new OperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForRemoval(1),
                    Items       = new[] { 1, 2, 3 }
                })
                .SetName("{m}(Single removal, leaving remaining items)"),
            new TestCaseData(new OperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForRemovals(new[] { 1, 2, 3 }),
                    Items       = new[] { 1, 2, 3, 4, 5, 6 }
                })
                .SetName("{m}(Multiple removals, leaving remaining items)"),
            new TestCaseData(new OperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForUpdate(new DistinctChange<int>[]
                    {
                        new() { Item = 2, Type = DistinctChangeType.Removal },
                        new() { Item = 3, Type = DistinctChangeType.Removal },
                        new() { Item = 2, Type = DistinctChangeType.Addition },
                        new() { Item = 4, Type = DistinctChangeType.Removal },
                        new() { Item = 3, Type = DistinctChangeType.Addition },
                    }),
                    Items       = new[] { 1, 2, 3, 4, 5 }
                })
                .SetName("{m}(More removals than additions)")
        };
    [TestCaseSource(nameof(WhenItemsAreRemoved_TestCases))]
    public void WhenItemsAreRemoved_ResultDecreases(OperatorTestCase testCase)
    {
        using var streamSource = new Subject<DistinctChangeSet<int>>();
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = Observable.Concat(
                Observable.Return(DistinctChangeSet.CreateForReset(testCase.Items)),
                streamSource)
        };
        
        using var subscription = stream.Count()
            .RecordValues(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedValues.Should().ContainSingle("an initial value should always be published");
        results.RecordedValues.Should().HaveElementAt(0, testCase.Items.Count, "the initial number of items should have been published");
        results.ClearNotifications();
        
        streamSource.OnNext(testCase.ChangeSet);
        
        var additionCount = testCase.ChangeSet.Changes
            .Count(change => change.Type is DistinctChangeType.Addition);
        
        var removalCount = testCase.ChangeSet.Changes
            .Count(change => change.Type is DistinctChangeType.Removal);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedValues.Should().ContainSingle("items were removed from the collection");
        results.RecordedValues.Should().HaveElementAt(0, testCase.Items.Count + additionCount - removalCount, "items were removed from the collection");
    }

    public static readonly IReadOnlyList<TestCaseData> WhenItemCountDoesNotChange_TestCases
        = new[]
        {
            new TestCaseData(new OperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.Empty<int>(),
                    Items       = Array.Empty<int>()
                })
                .SetName("{m}(Empty changeset, empty collection)"),
            new TestCaseData(new OperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.Empty<int>(),
                    Items       = new[] { 1 }
                })
                .SetName("{m}(Empty changeset, single item in collection)"),
            new TestCaseData(new OperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.Empty<int>(),
                    Items       = new[] { 1, 2, 3 }
                })
                .SetName("{m}(Empty changeset, multiple items in collection)"),
            new TestCaseData(new OperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForUpdate(new DistinctChange<int>[]
                    {
                        new() { Item = 1, Type = DistinctChangeType.Addition },
                        new() { Item = 1, Type = DistinctChangeType.Removal }
                    }),
                    Items       = Array.Empty<int>()
                })
                .SetName("{m}(Single addition and removal, empty collection)"),
            new TestCaseData(new OperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForUpdate(new DistinctChange<int>[]
                    {
                        new() { Item = 2, Type = DistinctChangeType.Addition },
                        new() { Item = 2, Type = DistinctChangeType.Removal }
                    }),
                    Items       = new[] { 1 }
                })
                .SetName("{m}(Single addition and removal, single item in collection)"),
            new TestCaseData(new OperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForUpdate(new DistinctChange<int>[]
                    {
                        new() { Item = 4, Type = DistinctChangeType.Addition },
                        new() { Item = 4, Type = DistinctChangeType.Removal }
                    }),
                    Items       = new[] { 1, 2, 3 }
                })
                .SetName("{m}(Single addition and removal, multiple items in collection)"),
            new TestCaseData(new OperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForUpdate(new DistinctChange<int>[]
                    {
                        new() { Item = 1, Type = DistinctChangeType.Addition },
                        new() { Item = 2, Type = DistinctChangeType.Addition },
                        new() { Item = 3, Type = DistinctChangeType.Addition },
                        new() { Item = 1, Type = DistinctChangeType.Removal },
                        new() { Item = 2, Type = DistinctChangeType.Removal },
                        new() { Item = 3, Type = DistinctChangeType.Removal }
                    }),
                    Items       = Array.Empty<int>()
                })
                .SetName("{m}(Multiple additions and removals, empty collection)"),
            new TestCaseData(new OperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForUpdate(new DistinctChange<int>[]
                    {
                        new() { Item = 2, Type = DistinctChangeType.Addition },
                        new() { Item = 3, Type = DistinctChangeType.Addition },
                        new() { Item = 4, Type = DistinctChangeType.Addition },
                        new() { Item = 4, Type = DistinctChangeType.Removal },
                        new() { Item = 3, Type = DistinctChangeType.Removal },
                        new() { Item = 2, Type = DistinctChangeType.Removal }
                    }),
                    Items       = new[] { 1 }
                })
                .SetName("{m}(Multiple additions and removals, single item in collection)"),
            new TestCaseData(new OperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForUpdate(new DistinctChange<int>[]
                    {
                        new() { Item = 4, Type = DistinctChangeType.Addition },
                        new() { Item = 4, Type = DistinctChangeType.Removal },
                        new() { Item = 5, Type = DistinctChangeType.Addition },
                        new() { Item = 5, Type = DistinctChangeType.Removal },
                        new() { Item = 6, Type = DistinctChangeType.Addition },
                        new() { Item = 6, Type = DistinctChangeType.Removal }
                    }),
                    Items       = new[] { 1, 2, 3 }
                })
                .SetName("{m}(Multiple additions and removals, multiple items in collection)")
        };
    [TestCaseSource(nameof(WhenItemCountDoesNotChange_TestCases))]
    public void WhenItemCountDoesNotChange_NotificationDoesNotPropagate(OperatorTestCase testCase)
    {
        using var streamSource = new Subject<DistinctChangeSet<int>>();
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = Observable.Concat(
                Observable.Return(DistinctChangeSet.CreateForReset(testCase.Items)),
                streamSource)
        };
        
        using var subscription = stream.Count()
            .RecordValues(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedValues.Should().ContainSingle("an initial value should always be published");
        results.RecordedValues.Should().HaveElementAt(0, testCase.Items.Count, "the initial number of items should have been published");
        results.ClearNotifications();
        
        streamSource.OnNext(testCase.ChangeSet);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedValues.Should().BeEmpty("the total number of items in the collection did not change");
    }

    [Test]
    public void WhenSourceCompletesAsynchronously_CompletionPropagates()
    {
        using var streamSource = new Subject<DistinctChangeSet<int>>();
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = streamSource
        };
        
        using var subscription = stream.Count()
            .RecordValues(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedValues.Should().ContainSingle("an initial value should always be published");
        results.ClearNotifications();
        
        streamSource.OnCompleted();
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeTrue("the source cannot publish any further notifications");
        results.RecordedValues.Should().BeEmpty("no item changes were published");
    }

    [Test]
    public void WhenSourceCompletesImmediately_CompletionPropagates()
    {
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = Observable.Empty<DistinctChangeSet<int>>()
        };
        
        using var subscription = stream.Count()
            .RecordValues(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeTrue("the source cannot publish any further notifications");
        results.RecordedValues.Should().ContainSingle("an initial value should always be published");
    }

    [Test]
    public void WhenSourceFailsAsynchronously_ErrorPropagates()
    {
        using var streamSource = new Subject<DistinctChangeSet<int>>();
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = streamSource
        };
        
        using var subscription = stream.Count()
            .RecordValues(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedValues.Should().ContainSingle("an initial value should always be published");
        results.ClearNotifications();
        
        var error = new TestException();
        
        streamSource.OnError(error);
        
        results.Error.Should().BeSameAs(error, "errors should propagate to subscribers");
        results.RecordedValues.Should().BeEmpty("no item changes were published");
    }

    [Test]
    public void WhenSourceFailsImmediately_ErrorPropagates()
    {
        var error = new TestException();
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = Observable.Throw<DistinctChangeSet<int>>(error)
        };
        
        using var subscription = stream.Count()
            .RecordValues(out var results);
        
        results.Error.Should().BeSameAs(error, "errors should propagate to subscribers");
        results.RecordedValues.Should().BeEmpty("an error occurred during initial subscription");
    }
}
