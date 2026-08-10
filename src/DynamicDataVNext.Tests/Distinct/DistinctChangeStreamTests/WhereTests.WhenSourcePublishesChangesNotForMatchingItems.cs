namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

public partial class WhereTests
{
    private static TestCaseData WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
            DistinctItemOptions     options,
            IReadOnlyList<int>      initialItems,
            DistinctChangeSet<int>  changeSet)
        => new(options, initialItems, changeSet);

    public static readonly IReadOnlyList<TestCaseData> WhenSourcePublishesChangesNotForMatchingItems_TestCases
        = new[]
        {
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.Empty<int>())
                .SetName("{m}(Empty Change Set, Empty collection, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new [] { 1 },
                    changeSet:      DistinctChangeSet.Empty<int>())
                .SetName("{m}(Empty Change Set, Single excluded item, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new [] { 2 },
                    changeSet:      DistinctChangeSet.Empty<int>())
                .SetName("{m}(Empty Change Set, Single included item, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new [] { 1, 2, 3, 4 },
                    changeSet:      DistinctChangeSet.Empty<int>())
                .SetName("{m}(Empty Change Set, Multiple items, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 1 }))
                .SetName("{m}(Initial Reset, Single item, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 1, 3, 5 }))
                .SetName("{m}(Initial Reset, Multiple items, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 1 },
                    changeSet:      DistinctChangeSet.CreateForReset(
                        removedItems:   new[] { 1 },
                        addedItems:     new[] { 3 }))
                .SetName("{m}(Subsequent Reset, Single item, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 1, 3, 5 },
                    changeSet:      DistinctChangeSet.CreateForReset(
                        removedItems:   new[] { 1, 3, 5 },
                        addedItems:     new[] { 7, 9, 11 }))
                .SetName("{m}(Subsequent Reset, Multiple items, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 1 },
                    changeSet:      DistinctChangeSet.CreateForClear(new[] { 1 }))
                .SetName("{m}(Clear, Single item, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 1, 3, 5 },
                    changeSet:      DistinctChangeSet.CreateForClear(new[] { 1, 3, 5 }))
                .SetName("{m}(Clear, Multiple items, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 2 },
                    changeSet:      DistinctChangeSet.CreateForAddition(1))
                .SetName("{m}(Add, Single item, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 1, 2, 3, 4 },
                    changeSet:      DistinctChangeSet.CreateForAdditions(new[] { 5, 7, 9 }))
                .SetName("{m}(Add, Multiple items, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 1, 2 },
                    changeSet:      DistinctChangeSet.CreateForRemoval(1))
                .SetName("{m}(Remove, Single item, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = false },
                    initialItems:   new[] { 1, 2, 3, 4, 5, 7, 9 },
                    changeSet:      DistinctChangeSet.CreateForRemovals(new[] { 1, 3, 5 }))
                .SetName("{m}(Remove, Multiple items, Immutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.Empty<int>())
                .SetName("{m}(Empty Change Set, Empty collection, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new [] { 1 },
                    changeSet:      DistinctChangeSet.Empty<int>())
                .SetName("{m}(Empty Change Set, Single excluded item, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new [] { 2 },
                    changeSet:      DistinctChangeSet.Empty<int>())
                .SetName("{m}(Empty Change Set, Single included item, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new [] { 1, 2, 3, 4 },
                    changeSet:      DistinctChangeSet.Empty<int>())
                .SetName("{m}(Empty Change Set, Multiple items, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 1 }))
                .SetName("{m}(Initial Reset, Single item, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   Array.Empty<int>(),
                    changeSet:      DistinctChangeSet.CreateForReset(new[] { 1, 3, 5 }))
                .SetName("{m}(Initial Reset, Multiple items, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 1 },
                    changeSet:      DistinctChangeSet.CreateForReset(
                        removedItems:   new[] { 1 },
                        addedItems:     new[] { 3 }))
                .SetName("{m}(Subsequent Reset, Single item, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 1, 3, 5 },
                    changeSet:      DistinctChangeSet.CreateForReset(
                        removedItems:   new[] { 1, 3, 5 },
                        addedItems:     new[] { 7, 9, 11 }))
                .SetName("{m}(Subsequent Reset, Multiple items, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 1 },
                    changeSet:      DistinctChangeSet.CreateForClear(new[] { 1 }))
                .SetName("{m}(Clear, Single item, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 1, 3, 5 },
                    changeSet:      DistinctChangeSet.CreateForClear(new[] { 1, 3, 5 }))
                .SetName("{m}(Clear, Multiple items, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 2 },
                    changeSet:      DistinctChangeSet.CreateForAddition(1))
                .SetName("{m}(Add, Single item, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 1, 2, 3, 4 },
                    changeSet:      DistinctChangeSet.CreateForAdditions(new[] { 5, 7, 9 }))
                .SetName("{m}(Add, Multiple items, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 1, 2 },
                    changeSet:      DistinctChangeSet.CreateForRemoval(1))
                .SetName("{m}(Remove, Single item, Mutable)"),
            WhenSourcePublishesChangesNotForMatchingItems_CreateTestCase(
                    options:        new() { ItemsAreMutable = true },
                    initialItems:   new[] { 1, 2, 3, 4, 5, 7, 9 },
                    changeSet:      DistinctChangeSet.CreateForRemovals(new[] { 1, 3, 5 }))
                .SetName("{m}(Remove, Multiple items, Mutable)")
        };
    
    [TestCaseSource(nameof(WhenSourcePublishesChangesNotForMatchingItems_TestCases))]
    public void WhenSourcePublishesChangesNotForMatchingItems_NotificationPropagates(
        DistinctItemOptions     options,
        IReadOnlyList<int>      initialItems,
        DistinctChangeSet<int>  changeSet)
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
        results.RecordedChangeSets.Should().BeEmpty("no changes were performed for matching items");
    }
}
