using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ReactiveHashSetTests;

public partial class ConstructorTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenSourceIsNotEmpty_TestCases
        = new[]
        {
            new TestCaseData(new ChangeOperationTestCase()
                {
                    InitialItems    = Array.Empty<int>(),
                    ChangeSet       = DistinctChangeSet.CreateForReset(addedItems: new[] { 1 }),
                    ExpectedItems   = new[] { 1 },
                    Because         = "a single item was added to the set"
                })
                .SetName("{m}(Single item added, Empty set)"),
            new TestCaseData(new ChangeOperationTestCase()
                {
                    InitialItems    = Array.Empty<int>(),
                    ChangeSet       = DistinctChangeSet.CreateForReset(addedItems: new[] { 1, 2, 3 }),
                    ExpectedItems   = new[] { 1, 2, 3 },
                    Because         = "multiple items were added to the set"
                })
                .SetName("{m}(Multiple items added, Empty set)"),
            new TestCaseData(new ChangeOperationTestCase()
                {
                    InitialItems    = new[] { 1, 2, 3 },
                    ChangeSet       = DistinctChangeSet.CreateForUpdate(new DistinctChange<int>() { Item = 4, Type = DistinctChangeType.Addition }),
                    ExpectedItems   = new[] { 1, 2, 3, 4 },
                    Because         = "a single item was added to the set"
                })
                .SetName("{m}(Single item added, Non-empty set)"),
            new TestCaseData(new ChangeOperationTestCase()
                {
                    InitialItems    = new[] { 1, 2, 3 },
                    ChangeSet       = DistinctChangeSet.CreateForUpdate(new[]
                    {
                        new DistinctChange<int>() { Item = 4, Type = DistinctChangeType.Addition },
                        new DistinctChange<int>() { Item = 5, Type = DistinctChangeType.Addition },
                        new DistinctChange<int>() { Item = 6, Type = DistinctChangeType.Addition }
                    }),
                    ExpectedItems   = new[] { 1, 2, 3, 4, 5, 6 },
                    Because         = "multiple items were added to the set"
                })
                .SetName("{m}(Multiple items added, Empty set)"),
            new TestCaseData(new ChangeOperationTestCase()
                {
                    InitialItems    = new[] { 1 },
                    ChangeSet       = DistinctChangeSet.CreateForClear(new[] { 1 }),
                    ExpectedItems   = Array.Empty<int>(),
                    Because         = "the set's only item was removed"
                })
                .SetName("{m}(Single item removed, Leaving empty set)"),
            new TestCaseData(new ChangeOperationTestCase()
                {
                    InitialItems    = new[] { 1, 2, 3 },
                    ChangeSet       = DistinctChangeSet.CreateForClear(new[] { 1, 2, 3 }),
                    ExpectedItems   = Array.Empty<int>(),
                    Because         = "all items in the set were removed"
                })
                .SetName("{m}(Multiple items removed, Leaving empty set)"),
            new TestCaseData(new ChangeOperationTestCase()
                {
                    InitialItems    = new[] { 1, 2, 3, 4 },
                    ChangeSet       = DistinctChangeSet.CreateForUpdate(new DistinctChange<int>() { Item = 1, Type = DistinctChangeType.Removal }),
                    ExpectedItems   = new[] { 2, 3, 4 },
                    Because         = "a single item was removed from the set"
                })
                .SetName("{m}(Single item removed, Leaving non-empty set)"),
            new TestCaseData(new ChangeOperationTestCase()
                {
                    InitialItems    = new[] { 1, 2, 3, 4, 5, 6 },
                    ChangeSet       = DistinctChangeSet.CreateForUpdate(new[]
                    {
                        new DistinctChange<int>() { Item = 1, Type = DistinctChangeType.Removal },
                        new DistinctChange<int>() { Item = 2, Type = DistinctChangeType.Removal },
                        new DistinctChange<int>() { Item = 3, Type = DistinctChangeType.Removal },
                    }),
                    ExpectedItems   = new[] { 4, 5, 6 },
                    Because         = "multiple items were removed from the set"
                })
                .SetName("{m}(Multiple items removed, Leaving non-empty set)"),
            new TestCaseData(new ChangeOperationTestCase()
                {
                    InitialItems    = new[] { 1, 2, 3 },
                    ChangeSet       = DistinctChangeSet.CreateForReset(
                        removedItems:   new[] { 1, 2, 3 },
                        addedItems:     new[] { 4, 5, 6 }),
                    ExpectedItems   = new[] { 4, 5, 6 },
                    Because         = "all items were removed from the set, then new items were added"
                })
                .SetName("{m}(Multiple item reset)")
        };
    [TestCaseSource(nameof(WhenSourceIsNotEmpty_TestCases))]
    public void WhenSourceIsNotEmpty_ResultMatchesSource(ChangeOperationTestCase testCase)
    {
        using var sourceSource = new Subject<DistinctChangeSet<int>>();

        using var uut = new ReactiveHashSet<int>(
            source:     Observable
                .Return(DistinctChangeSet.CreateForReset(addedItems: testCase.InitialItems))
                .Concat(sourceSource),
            comparer:   EqualityComparer<int>.Default,
            options:    default);

        uut.Should().BeEquivalentTo(testCase.InitialItems, options => options.WithoutStrictOrdering(), "an initial set of items was given");
        uut.ChangeStream.Comparer.Should().BeSameAs(EqualityComparer<int>.Default, "no equality comparer was specified");
        uut.ChangeStream.Options.Should().Be(default(DistinctItemOptions), "no change tracking options were specified");
        
        using var collectionChangeSubscription = uut.CollectionChanged
            .RecordValues(out var collectionChangedResults);

        collectionChangedResults.Error.Should().BeNull("no errors should have occurred");
        collectionChangedResults.HasCompleted.Should().BeFalse("the source can still publish notifications");
        collectionChangedResults.RecordedValues.Should().BeEmpty("change events cannot occur during subscription");
        
        using var changeStreamSubscription = uut.ChangeStream
            .RecordItems(out var changeStreamResults);
        
        changeStreamResults.Error.Should().BeNull("no errors should have occurred");
        changeStreamResults.HasCompleted.Should().BeFalse("the source can still publish notifications");
        changeStreamResults.RecordedItems.Should().BeEquivalentTo(testCase.InitialItems, options => options.WithoutStrictOrdering(), "subscribers should be initialized to match the set");
        
        sourceSource.OnNext(testCase.ChangeSet);
        
        uut.Should().BeEquivalentTo(testCase.ExpectedItems, options => options.WithoutStrictOrdering(), testCase.Because);
        
        collectionChangedResults.Error.Should().BeNull("no errors should have occurred");
        collectionChangedResults.HasCompleted.Should().BeFalse("the source can still publish notifications");
        collectionChangedResults.RecordedValues.Should().ContainSingle("the published changeset should have mutated the set");
        
        changeStreamResults.Error.Should().BeNull("no errors should have occurred");
        changeStreamResults.HasCompleted.Should().BeFalse("the source can still publish notifications");
        changeStreamResults.RecordedChangeSets.Should().NotBeEmpty("the published changeset should have propagated to subscribers");
        changeStreamResults.RecordedItems.Should().BeEquivalentTo(testCase.ExpectedItems, options => options.WithoutStrictOrdering(), testCase.Because);
    }
}
