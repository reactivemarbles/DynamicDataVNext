using System;
using System.Collections.Generic;

using ReactiveUI.Primitives.Signals;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

public partial class ContainsTests
{
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
        using var streamSource = new Signal<DistinctChangeSet<int>>();
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = Signal.Concat(
                Signal.Return(DistinctChangeSet.CreateForReset(testCase.Items)),
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
}
