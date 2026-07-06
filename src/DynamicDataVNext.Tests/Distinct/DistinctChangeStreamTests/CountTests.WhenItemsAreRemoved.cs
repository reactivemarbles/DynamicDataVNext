using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

public partial class CountTests
{
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
}
