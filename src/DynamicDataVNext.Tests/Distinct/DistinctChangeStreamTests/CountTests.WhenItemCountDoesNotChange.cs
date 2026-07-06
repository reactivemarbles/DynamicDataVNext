using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

public partial class CountTests
{
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
}
