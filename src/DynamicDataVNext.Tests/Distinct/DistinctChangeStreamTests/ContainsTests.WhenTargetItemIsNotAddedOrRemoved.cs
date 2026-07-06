using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

public partial class ContainsTests
{
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
}
