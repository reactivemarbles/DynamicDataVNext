using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

public partial class SelectTests
{
    private static TestCaseData WhenSourcePublishesChanges_CreateTestCase(
            DistinctItemSelectionOptions    options,
            IReadOnlyList<int>              initialItems,
            DistinctChangeSet<int>          changeSet,
            IReadOnlyList<int>              finalItems)
        => new(options, initialItems, changeSet, finalItems);
    
    public static readonly IReadOnlyList<TestCaseData> WhenSourcePublishesChanges_TestCases
        = new[]
        {
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.Deterministic },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 1 }),
                    finalItems:     new[] { 1 })
                .SetName("{m}(Initial Reset, Single item, Deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.Deterministic },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 1, 2, 3 }),
                    finalItems:     new[] { 1, 2, 3 })
                .SetName("{m}(Initial Reset, Multiple items, Deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.Deterministic },
                    initialItems:   new[] { 1 },
                    changeSet:      DistinctChangeSet.CreateForReset(
                        removedItems:   new[] { 1 },
                        addedItems:     new[] { 2 }),
                    finalItems:     new[] { 2 })
                .SetName("{m}(Subsequent Reset, Single item, Deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.Deterministic },
                    initialItems:   new[] { 1, 2, 3 },
                    changeSet:      DistinctChangeSet.CreateForReset(
                        removedItems:   new[] { 1, 2, 3 },
                        addedItems:     new[] { 4, 5, 6 }),
                    finalItems:     new[] { 4, 5, 6 })
                .SetName("{m}(Subsequent Reset, Multiple items, Deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.Deterministic },
                    initialItems:   new[] { 1 },
                    changeSet:      DistinctChangeSet.CreateForClear(new[] { 1 }),
                    finalItems:     Array.Empty<int>())
                .SetName("{m}(Clear, Single item, Deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.Deterministic },
                    initialItems:   new[] { 1, 2, 3 },
                    changeSet:      DistinctChangeSet.CreateForClear(new[] { 1, 2, 3 }),
                    finalItems:     Array.Empty<int>())
                .SetName("{m}(Clear, Multiple items, Deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.Deterministic },
                    initialItems:   new[] { 1 },
                    changeSet:      DistinctChangeSet.CreateForAddition(2),
                    finalItems:     new[] { 1, 2 })
                .SetName("{m}(Add, Single item, Deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.Deterministic },
                    initialItems:   new[] { 1, 2, 3 },
                    changeSet:      DistinctChangeSet.CreateForAdditions(new[] { 4, 5, 6 }),
                    finalItems:     new[] { 1, 2, 3, 4, 5, 6 })
                .SetName("{m}(Add, Multiple items, Deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.Deterministic },
                    initialItems:   new[] { 1, 2 },
                    changeSet:      DistinctChangeSet.CreateForRemoval(1),
                    finalItems:     new[] { 2 })
                .SetName("{m}(Remove, Single item, Deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.Deterministic },
                    initialItems:   new[] { 1, 2, 3, 4, 5, 6 },
                    changeSet:      DistinctChangeSet.CreateForRemovals(new[] { 1, 2, 3 }),
                    finalItems:     new[] { 4, 5, 6 })
                .SetName("{m}(Remove, Multiple items, Deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.NonDeterministic },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 1 }),
                    finalItems:     new[] { 1 })
                .SetName("{m}(Initial Reset, Single item, Non-deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.NonDeterministic },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 1, 2, 3 }),
                    finalItems:     new[] { 1, 2, 3 })
                .SetName("{m}(Initial Reset, Multiple items, Non-deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.NonDeterministic },
                    initialItems:   new[] { 1 },
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 2 }),
                    finalItems:     new[] { 2 })
                .SetName("{m}(Subsequent Reset, Single item, Non-deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.NonDeterministic },
                    initialItems:   new[] { 1, 2, 3 },
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 4, 5, 6 }),
                    finalItems:     new[] { 4, 5, 6 })
                .SetName("{m}(Subsequent Reset, Multiple items, Non-deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.NonDeterministic },
                    initialItems:   new[] { 1 },
                    changeSet:      DistinctChangeSet.CreateForClear(new[] { 1 }),
                    finalItems:     Array.Empty<int>())
                .SetName("{m}(Clear, Single item, Non-deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.NonDeterministic },
                    initialItems:   new[] { 1, 2, 3 },
                    changeSet:      DistinctChangeSet.CreateForClear(new[] { 1, 2, 3 }),
                    finalItems:     Array.Empty<int>())
                .SetName("{m}(Clear, Multiple items, Non-deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.NonDeterministic },
                    initialItems:   new[] { 1 },
                    changeSet:      DistinctChangeSet.CreateForAddition(2),
                    finalItems:     new[] { 1, 2 })
                .SetName("{m}(Add, Single item, Non-deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.NonDeterministic },
                    initialItems:   new[] { 1, 2, 3 },
                    changeSet:      DistinctChangeSet.CreateForAdditions(new[] { 4, 5, 6 }),
                    finalItems:     new[] { 1, 2, 3, 4, 5, 6 })
                .SetName("{m}(Add, Multiple items, Non-deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.NonDeterministic },
                    initialItems:   new[] { 1, 2 },
                    changeSet:      DistinctChangeSet.CreateForRemoval(1),
                    finalItems:     new[] { 2 })
                .SetName("{m}(Remove, Single item, Non-deterministic selection)"),
            WhenSourcePublishesChanges_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.NonDeterministic },
                    initialItems:   new[] { 1, 2, 3, 4, 5, 6 },
                    changeSet:      DistinctChangeSet.CreateForRemovals(new[] { 1, 2, 3 }),
                    finalItems:     new[] { 4, 5, 6 })
                .SetName("{m}(Remove, Multiple items, Non-deterministic selection)")
        };
    [TestCaseSource(nameof(WhenSourcePublishesChanges_TestCases))]
    public void WhenSourcePublishesChanges_NotificationPropagates(
        DistinctItemSelectionOptions    options,
        IReadOnlyList<int>              initialItems,
        DistinctChangeSet<int>          changeSet,
        IReadOnlyList<int>              finalItems)
    {
        using var source = new Subject<DistinctChangeSet<int>>(); 
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = Observable.Concat(
                (initialItems.Count is 0)
                    ? Observable.Empty<DistinctChangeSet<int>>()
                    : Observable.Return(DistinctChangeSet.CreateForReset(initialItems)),
                source)
        };
        
        using var subscription = stream.Select(
                selector:   static item => item.ToString(),
                options:    options)
            .ValidateChangeSets()
            .RecordItems(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        if (initialItems.Count is 0)
            results.RecordedChangeSets.Should().BeEmpty("there were no initial items in the collection");
        else
        {
            results.RecordedChangeSets.Should().ContainSingle("there were initial items in the collection");
            results.RecordedItems.Should().BeEquivalentTo(
                expectation:    initialItems.Select(static item => item.ToString()),
                config:         options => options.WithoutStrictOrdering(),
                because:        "all changes should propagate downstream, after having the selector applied");
            results.ClearNotifications();
        }

        source.OnNext(changeSet);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().ContainSingle("a single change operation occurred");
        results.RecordedItems.Should().BeEquivalentTo(
            expectation:    finalItems.Select(static item => item.ToString()),
            config:         options => options.WithoutStrictOrdering(),
            because:        "all changes should propagate downstream, after having the selector applied");
    }
}
