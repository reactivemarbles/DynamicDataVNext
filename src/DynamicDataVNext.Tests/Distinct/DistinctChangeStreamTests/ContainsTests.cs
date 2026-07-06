using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

[TestFixture]
public class ContainsTests
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
                : Observable.Concat(
                    Observable.Return(DistinctChangeSet.CreateForReset(new[] { 1, 2, 3 })),
                    Observable.Never<DistinctChangeSet<int>>())
        };
        
        using var subscription = stream.Contains(default)
            .RecordValues(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedValues.Should().ContainSingle("an initial value should always be published");
    }
    
    [Test]
    public void Always_UsesSourceComparer()
    {
        var stream = new DistinctChangeStream<string>()
        {
            Comparer    = StringComparer.OrdinalIgnoreCase,
            Source      = Observable.Concat(
                Observable.Return(DistinctChangeSet.CreateForReset(new[]
                {
                    "Test"
                })),
                Observable.Never<DistinctChangeSet<string>>())
        };
        
        using var subscription = stream.Contains("test")
            .RecordValues(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedValues.Should().ContainSingle("an initial value should always be published");
        results.RecordedValues.Should().HaveElementAt(0, true, "the case-insensitive comparer should have been used");
    }

    [Test]
    public void WhenOptionsSupportsMutableItems_ThrowsUnsupported()
    {
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Options     = new() { ItemsAreMutable = true },
            Source      = Observable.Never<DistinctChangeSet<int>>()
        };
        
        var result = FluentActions.Invoking(
                () => _ = stream.Contains(default))
            .Should().Throw<NotSupportedException>()
            .Which;
        
        Console.WriteLine(result);
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
        
        using var subscription = stream.Contains(default)
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
        
        using var subscription = stream.Contains(default)
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
        
        using var subscription = stream.Contains(default)
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
        
        using var subscription = stream.Contains(default)
            .RecordValues(out var results);
        
        results.Error.Should().BeSameAs(error, "errors should propagate to subscribers");
        results.RecordedValues.Should().BeEmpty("an error occurred during initial subscription");
    }

    public static readonly IReadOnlyList<TestCaseData> WhenTargetItemIsAdded_TestCases
        = new[]
        {
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    Item    = 1,
                    Items   = Array.Empty<int>()
                })
                .SetName("{m}(Empty collection)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    Item    = 1,
                    Items   = new[] { 2 }
                    
                })
                .SetName("{m}(Single item in collection)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    Item    = 1,
                    Items   = new[] { 2, 3, 4 }
                })
                .SetName("{m}(Multiple items in collection)")
        };
    [TestCaseSource(nameof(WhenTargetItemIsAdded_TestCases))]
    public void WhenTargetItemIsAdded_ResultChangesToTrue(SingleItemOperationTestCase testCase)
    {
        using var streamSource = new Subject<DistinctChangeSet<int>>();
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = Observable.Concat(
                Observable.Return(DistinctChangeSet.CreateForReset(testCase.Items)),
                streamSource)
        };
        
        using var subscription = stream.Contains(testCase.Item)
            .RecordValues(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedValues.Should().ContainSingle("an initial value should always be published");
        results.RecordedValues.Should().HaveElementAt(0, false, "the target item is not currently in the collection");
        results.ClearNotifications();
        
        streamSource.OnNext(DistinctChangeSet.CreateForUpdate(new DistinctChange<int>()
        {
            Item = testCase.Item,
            Type = DistinctChangeType.Addition
        }));

        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedValues.Should().ContainSingle("a source operation added the target item");
        results.RecordedValues.Should().HaveElementAt(0, true, "the target item was added to the collection");
    }

    public static readonly IReadOnlyList<TestCaseData> WhenTargetItemIsNotAddedOrRemoved_TestCases
        = new[]
        {
            new TestCaseData(new SingleItemOperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.Empty<int>(),
                    Item        = 1,
                    Items       = Array.Empty<int>()
                })
                .SetName("{m}(Empty changeset, empty collection)"),
            new TestCaseData(new SingleItemOperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.Empty<int>(),
                    Item        = 1,
                    Items       = new[] { 1 }
                })
                .SetName("{m}(Empty changeset, target item in collection)"),
            new TestCaseData(new SingleItemOperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.Empty<int>(),
                    Item        = 1,
                    Items       = new[] { 2 }
                })
                .SetName("{m}(Empty changeset, single non-target item in collection)"),
            new TestCaseData(new SingleItemOperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.Empty<int>(),
                    Item        = 1,
                    Items       = new[] { 1, 2, 3 }
                })
                .SetName("{m}(Empty changeset, multiple items, including target, in collection)"),
            new TestCaseData(new SingleItemOperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.Empty<int>(),
                    Item        = 1,
                    Items       = new[] { 2, 3, 4 }
                })
                .SetName("{m}(Empty changeset, multiple non-target items in collection)"),
            new TestCaseData(new SingleItemOperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForReset(new[] { 2 }),
                    Item        = 1,
                    Items       = Array.Empty<int>()
                })
                .SetName("{m}(Non-target addition, empty collection)"),
            new TestCaseData(new SingleItemOperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForAddition(2),
                    Item        = 1,
                    Items       = new[] { 1 }
                })
                .SetName("{m}(Non-target addition, target item in collection)"),
            new TestCaseData(new SingleItemOperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForAddition(3),
                    Item        = 1,
                    Items       = new[] { 2 }
                })
                .SetName("{m}(Non-target addition, single non-target item in collection)"),
            new TestCaseData(new SingleItemOperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForAddition(4),
                    Item        = 1,
                    Items       = new[] { 1, 2, 3 }
                })
                .SetName("{m}(Non-target addition, multiple items, including target, in collection)"),
            new TestCaseData(new SingleItemOperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForAddition(5),
                    Item        = 1,
                    Items       = new[] { 2, 3, 4 }
                })
                .SetName("{m}(Non-target addition, multiple non-target items in collection)"),
            new TestCaseData(new SingleItemOperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForRemoval(2),
                    Item        = 1,
                    Items       = new[] { 1, 2 }
                })
                .SetName("{m}(Non-target removal, target item in collection)"),
            new TestCaseData(new SingleItemOperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForClear(new[] { 2 }),
                    Item        = 1,
                    Items       = new[] { 2 }
                })
                .SetName("{m}(Non-target removal, single non-target item in collection)"),
            new TestCaseData(new SingleItemOperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForRemoval(2),
                    Item        = 1,
                    Items       = new[] { 1, 2, 3 }
                })
                .SetName("{m}(Non-target removal, multiple items, including target, in collection)"),
            new TestCaseData(new SingleItemOperatorTestCase()
                {
                    ChangeSet   = DistinctChangeSet.CreateForRemoval(3),
                    Item        = 1,
                    Items       = new[] { 2, 3, 4 }
                })
                .SetName("{m}(Non-target removal, multiple non-target items in collection)")
        };
    [TestCaseSource(nameof(WhenTargetItemIsNotAddedOrRemoved_TestCases))]
    public void WhenTargetItemIsNotAddedOrRemoved_NotificationDoesNotPropagates(SingleItemOperatorTestCase testCase)
    {
        using var streamSource = new Subject<DistinctChangeSet<int>>();
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = Observable.Concat(
                Observable.Return(DistinctChangeSet.CreateForReset(testCase.Items)),
                streamSource)
        };
        
        using var subscription = stream.Contains(testCase.Item)
            .RecordValues(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedValues.Should().ContainSingle("an initial value should always be published");
        results.ClearNotifications();
        
        streamSource.OnNext(testCase.ChangeSet);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedValues.Should().BeEmpty("the source operation did not add or remove the target item");
    }
    
    public static readonly IReadOnlyList<TestCaseData> WhenTargetItemIsRemoved_TestCases
        = new[]
        {
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    Item    = 1,
                    Items   = new[] { 1 }
                })
                .SetName("{m}(Single item in collection)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    Item    = 1,
                    Items   = new[] { 1, 2, 3 }
                })
                .SetName("{m}(Multiple items in collection)")
        };
    [TestCaseSource(nameof(WhenTargetItemIsRemoved_TestCases))]
    public void WhenTargetItemIsRemoved_ResultChangesToFalse(SingleItemOperationTestCase testCase)
    {
        using var streamSource = new Subject<DistinctChangeSet<int>>();
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = Observable.Concat(
                Observable.Return(DistinctChangeSet.CreateForReset(testCase.Items)),
                streamSource)
        };
        
        using var subscription = stream.Contains(testCase.Item)
            .RecordValues(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedValues.Should().ContainSingle("an initial value should always be published");
        results.RecordedValues.Should().HaveElementAt(0, true, "the target item is currently in the collection");
        results.ClearNotifications();
        
        streamSource.OnNext(DistinctChangeSet.CreateForUpdate(new DistinctChange<int>()
        {
            Item = testCase.Item,
            Type = DistinctChangeType.Removal
        }));
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedValues.Should().ContainSingle("a source operation removed the target item");
        results.RecordedValues.Should().HaveElementAt(0, false, "the target item was removed from the collection");
    }
}
