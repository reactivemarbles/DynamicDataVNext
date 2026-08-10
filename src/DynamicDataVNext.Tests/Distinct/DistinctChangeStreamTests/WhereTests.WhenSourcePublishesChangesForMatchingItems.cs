namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

public partial class WhereTests
{
    private static TestCaseData WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
            DistinctItemOptions     options,
            IReadOnlyList<int>      initialItems,
            DistinctChangeSet<int>  changeSet,
            IReadOnlyList<int>      finalItems)
        => new(options, initialItems, changeSet, finalItems);

    public static readonly IReadOnlyList<TestCaseData> WhenSourcePublishesChangesForMatchingItems_TestCases
        = new[]
        {
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 2 }),
                    finalItems:     new[] { 2 })
                .SetName("{m}(Initial Reset, Single item, Immutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 1, 2, 3, 4 }),
                    finalItems:     new[] { 2, 4 })
                .SetName("{m}(Initial Reset, Multiple items, Immutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 2 },
                    changeSet:      DistinctChangeSet.CreateForReset(
                        removedItems:   new[] { 2 },
                        addedItems:     new[] { 4 }),
                    finalItems:     new[] { 4 })
                .SetName("{m}(Subsequent Reset, Single item, Immutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 1, 2, 3, 4 },
                    changeSet:      DistinctChangeSet.CreateForReset(
                        removedItems:   new[] { 1, 2, 3, 4 },
                        addedItems:     new[] { 5, 6, 7, 8 }),
                    finalItems:     new[] { 6, 8 })
                .SetName("{m}(Subsequent Reset, Multiple items, Immutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 2 },
                    changeSet:      DistinctChangeSet.CreateForClear(new[] { 2 }),
                    finalItems:     Array.Empty<int>())
                .SetName("{m}(Clear, Single item, Immutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 1, 2, 3, 4 },
                    changeSet:      DistinctChangeSet.CreateForClear(new[] { 1, 2, 3, 4 }),
                    finalItems:     Array.Empty<int>())
                .SetName("{m}(Clear, Multiple items, Immutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 2 },
                    changeSet:      DistinctChangeSet.CreateForAddition(4),
                    finalItems:     new[] { 2, 4 })
                .SetName("{m}(Add, Single item, Immutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 1, 2, 3, 4 },
                    changeSet:      DistinctChangeSet.CreateForAdditions(new[] { 5, 6, 7, 8 }),
                    finalItems:     new[] { 2, 4, 6, 8 })
                .SetName("{m}(Add, Multiple items, Immutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 2, 4 },
                    changeSet:      DistinctChangeSet.CreateForRemoval(2),
                    finalItems:     new[] { 4 })
                .SetName("{m}(Remove, Single item, Immutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 1, 2, 3, 4, 5, 6, 7, 8 },
                    changeSet:      DistinctChangeSet.CreateForRemovals(new[] { 1, 2, 3, 4 }),
                    finalItems:     new[] { 6, 8 })
                .SetName("{m}(Remove, Multiple items, Immutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 2 }),
                    finalItems:     new[] { 2 })
                .SetName("{m}(Initial Reset, Single item, Mutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 1, 2, 3, 4 }),
                    finalItems:     new[] { 2, 4 })
                .SetName("{m}(Initial Reset, Multiple items, Mutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 2 },
                    changeSet:      DistinctChangeSet.CreateForReset(
                        removedItems:   new[] { 2 },
                        addedItems:     new[] { 4 }),
                    finalItems:     new[] { 4 })
                .SetName("{m}(Subsequent Reset, Single item, Mutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 1, 2, 3, 4 },
                    changeSet:      DistinctChangeSet.CreateForReset(
                        removedItems:   new[] { 1, 2, 3, 4 },
                        addedItems:     new[] { 5, 6, 7, 8 }),
                    finalItems:     new[] { 6, 8 })
                .SetName("{m}(Subsequent Reset, Multiple items, Mutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 2 },
                    changeSet:      DistinctChangeSet.CreateForClear(new[] { 2 }),
                    finalItems:     Array.Empty<int>())
                .SetName("{m}(Clear, Single item, Mutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 1, 2, 3, 4 },
                    changeSet:      DistinctChangeSet.CreateForClear(new[] { 1, 2, 3, 4 }),
                    finalItems:     Array.Empty<int>())
                .SetName("{m}(Clear, Multiple items, Mutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 2 },
                    changeSet:      DistinctChangeSet.CreateForAddition(4),
                    finalItems:     new[] { 2, 4 })
                .SetName("{m}(Add, Single item, Mutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 1, 2, 3, 4 },
                    changeSet:      DistinctChangeSet.CreateForAdditions(new[] { 5, 6, 7, 8 }),
                    finalItems:     new[] { 2, 4, 6, 8 })
                .SetName("{m}(Add, Multiple items, Mutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 2, 4 },
                    changeSet:      DistinctChangeSet.CreateForRemoval(2),
                    finalItems:     new[] { 4 })
                .SetName("{m}(Remove, Single item, Mutable)"),
            WhenSourcePublishesChangesForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 1, 2, 3, 4, 5, 6, 7, 8 },
                    changeSet:      DistinctChangeSet.CreateForRemovals(new[] { 1, 2, 3, 4 }),
                    finalItems:     new[] { 6, 8 })
                .SetName("{m}(Remove, Multiple items, Mutable)")
        };
    
    [TestCaseSource(nameof(WhenSourcePublishesChangesForMatchingItems_TestCases))]
    public void WhenSourcePublishesChangesForMatchingItems_NotificationPropagates(
        DistinctItemOptions     options,
        IReadOnlyList<int>      initialItems,
        DistinctChangeSet<int>  changeSet,
        IReadOnlyList<int>      finalItems)
    {
        using var source = new Signal<DistinctChangeSet<int>>(); 
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Options     = options,
            Source      = Signal.Concat(
                Signal.Return(DistinctChangeSet.CreateForReset(initialItems)),
                source)
        };
        
        using var subscription = stream.Where(IsEven)
            .ValidateChangeSets()
            .RecordItems(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        if (initialItems.Any(IsEven))
        {
            results.RecordedChangeSets.Should().ContainSingle("there were initial matching items in the collection");
            results.RecordedItems.Should().BeEquivalentTo(
                expectation:    initialItems.Where(IsEven),
                config:         options => options.WithoutStrictOrdering(),
                because:        "all changes for items matching the predicate should propagate downstream");
            results.ClearNotifications();
        }
        else
            results.RecordedChangeSets.Should().BeEmpty("there were no initial matching items in the collection");

        source.OnNext(changeSet);

        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
        results.RecordedItems.Should().BeEquivalentTo(
            expectation:    finalItems,
            config:         options => options.WithoutStrictOrdering(),
            because:        "all changes for items matching the predicate should propagate downstream");
    }
}
