using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

public partial class ContainsTests
{
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
