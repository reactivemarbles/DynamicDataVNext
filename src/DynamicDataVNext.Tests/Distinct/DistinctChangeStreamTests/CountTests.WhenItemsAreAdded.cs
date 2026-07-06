using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

public partial class CountTests
{
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
}
