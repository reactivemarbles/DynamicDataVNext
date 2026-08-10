using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingCacheTests;

public sealed class UutFixture
    : ICacheUutFixture<UutFixture, ChangeTrackingCache<string, TestItem>>,
        IReadOnlyCacheUutFixture<UutFixture, ChangeTrackingCache<string, TestItem>>
{
    public static UutFixture Create(
            Func<TestItem, string>      keySelector,
            IEqualityComparer<string>?  comparer    = null,
            KeyedItemOptions            options     = default)
        => new(
            keySelector:    keySelector,
            uut:            new(
                keySelector:    keySelector,
                comparer:       comparer,
                options:        options));

    public static UutFixture Create(
            int                         capacity,
            Func<TestItem, string>      keySelector,
            IEqualityComparer<string>?  comparer    = null,
            KeyedItemOptions            options     = default)
        => new(
            keySelector:    keySelector,
            uut:            new(
                capacity:       capacity,
                keySelector:    keySelector,
                comparer:       comparer,
                options:        options));

    public static UutFixture Create(
            IEnumerable<TestItem>       items,
            Func<TestItem, string>      keySelector,
            IEqualityComparer<string>?  comparer    = null,
            KeyedItemOptions            options     = default)
        => new(
            keySelector:    keySelector,
            uut:            new(
                items:          items,
                keySelector:    keySelector,
                comparer:       comparer,
                options:        options));

    private UutFixture(
        Func<TestItem, string>                  keySelector,
        ChangeTrackingCache<string, TestItem>   uut)
    {
        _keySelector    = keySelector;
        _uut            = uut;
    }
    
    public ChangeTrackingCache<string, TestItem> Uut
        => _uut;

    public IEqualityComparer<string> UutComparer
        => _uut.Comparer;
    
    public KeyedItemOptions UutOptions
        => _uut.Options;

    public void AddRangeToUut(IEnumerable<TestItem> items)
        => _uut.AddRange(items);

    public void AssertItemWasAdded(TestItem addedItem)
    {
        var addedKey = _keySelector.Invoke(addedItem);
    
        _uut.BufferedChanges.Should().ContainSingle("a single change was made");
        _uut.BufferedChanges[0].Type.Should().Be(KeyedChangeType.Addition, "a single addition was performed");
        _uut.BufferedChanges[0].AsAddition().Item.Should().Be(addedItem, "the given item should have been added");
        _uut.BufferedChanges[0].AsAddition().Key.Should().Be(addedKey, "the given item's key should have been retrieved and used");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "adding an item to a non-empty set should produce an update");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();
        
        capturedChangeSet.Changes.Should().ContainSingle("a single change was made");
        capturedChangeSet.Changes[0].Type.Should().Be(KeyedChangeType.Addition, "a single addition was performed");
        capturedChangeSet.Changes[0].AsAddition().Item.Should().Be(addedItem, "the given item should have been added");
        capturedChangeSet.Changes[0].AsAddition().Key.Should().Be(addedKey, "the given item's key should have been retrieved and used");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "adding an item to a non-empty set should produce an update");
        
        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertItemWasRefreshed(TestItem refreshedItem)
    {
        var refreshedKey = _keySelector.Invoke(refreshedItem);

        _uut.BufferedChanges.Should().ContainSingle("a single change was made");
        _uut.BufferedChanges[0].Type.Should().Be(KeyedChangeType.Refreshment, "a single refreshment was performed");
        _uut.BufferedChanges[0].AsRefreshment().Item.Should().Be(refreshedItem, "the given item should have been added");
        _uut.BufferedChanges[0].AsRefreshment().Key.Should().Be(refreshedKey, "the given item's key should have been retrieved and used");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "refreshing an item should produce an update");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();
        
        capturedChangeSet.Changes.Should().ContainSingle("a single change was made");
        capturedChangeSet.Changes[0].Type.Should().Be(KeyedChangeType.Refreshment, "a single refreshment was performed");
        capturedChangeSet.Changes[0].AsRefreshment().Item.Should().Be(refreshedItem, "the given item should have been added");
        capturedChangeSet.Changes[0].AsRefreshment().Key.Should().Be(refreshedKey, "the given item's key should have been retrieved and used");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "refreshing an item should produce an update");
        
        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertItemWasRemoved(TestItem removedItem)
    {
        var removedKey = _keySelector.Invoke(removedItem);

        _uut.BufferedChanges.Should().ContainSingle("a single change was made");
        _uut.BufferedChanges[0].Type.Should().Be(KeyedChangeType.Removal, "a single removal was performed");
        _uut.BufferedChanges[0].AsRemoval().Item.Should().Be(removedItem, "the given item should have been removed");
        _uut.BufferedChanges[0].AsRemoval().Key.Should().Be(removedKey, "the given item's key should have been retrieved and used");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "removing an item from a collection of multiple items should produce an update");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();
        
        capturedChangeSet.Changes.Should().ContainSingle("a single change was made");
        capturedChangeSet.Changes[0].Type.Should().Be(KeyedChangeType.Removal, "a single removal was performed");
        capturedChangeSet.Changes[0].AsRemoval().Item.Should().Be(removedItem, "the given item should have been removed");
        capturedChangeSet.Changes[0].AsRemoval().Key.Should().Be(removedKey, "the given item's key should have been retrieved and used");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "removing an item from a collection of multiple items should produce an update");
        
        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertItemWasReplaced(
        TestItem oldItem,
        TestItem newItem)
    {
        var key = _keySelector.Invoke(newItem);
    
        _uut.BufferedChanges.Should().ContainSingle("a single change was made");
        _uut.BufferedChanges[0].Type.Should().Be(KeyedChangeType.Replacement, "a single replacement was performed");
        _uut.BufferedChanges[0].AsReplacement().Key.Should().Be(key, "the replacement should have occurred for the given key");
        _uut.BufferedChanges[0].AsReplacement().OldItem.Should().Be(oldItem, "the previous item at the given key should have been recorded");
        _uut.BufferedChanges[0].AsReplacement().NewItem.Should().Be(newItem, "the given item should have replaced the previous one");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "replacing an item within a collection should produce an update");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();

        capturedChangeSet.Changes.Should().ContainSingle("a single change was made");
        capturedChangeSet.Changes[0].Type.Should().Be(KeyedChangeType.Replacement, "a single replacement was performed");
        capturedChangeSet.Changes[0].AsReplacement().Key.Should().Be(key, "the replacement should have occurred for the given key");
        capturedChangeSet.Changes[0].AsReplacement().OldItem.Should().Be(oldItem, "the previous item at the given key should have been recorded");
        capturedChangeSet.Changes[0].AsReplacement().NewItem.Should().Be(newItem, "the given item should have replaced the previous one");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "replacing an item within a collection should produce an update");

        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertItemsWereAdded(IReadOnlyList<TestItem> addedItems)
    {
        var additions = addedItems
            .Select(addedItem => new KeyedItem<string, TestItem>()
            {
                Key     = _keySelector.Invoke(addedItem),
                Item    = addedItem
            })
            .ToArray();
    
        _uut.BufferedChanges.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "items should only have been added");
        _uut.BufferedChanges.Select(change => change.AsAddition()).Should().BeEquivalentTo(additions, options => options.WithoutStrictOrdering(), "items should have been added to the collection");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "adding items to a non-empty collection should produce an update");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();
        
        capturedChangeSet.Changes.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "items should only have been added");
        capturedChangeSet.Changes.Select(change => change.AsAddition()).Should().BeEquivalentTo(additions, options => options.WithoutStrictOrdering(), "items should have been added to the collection");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "adding items to a non-empty collection should produce an update");
        
        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertItemsWereMerged(
        IReadOnlyList<TestItem>                             addedItems,
        IReadOnlyList<KeyedReplacement<string, TestItem>>   replacements)
    {
        var additions = addedItems
            .Select(addedItem => new KeyedItem<string, TestItem>()
            {
                Key     = _keySelector.Invoke(addedItem),
                Item    = addedItem
            })
            .ToArray();

        _uut.BufferedChanges.Count.Should().Be(additions.Length + replacements.Count, "all given items not present in the collection should have been merged into it");
        _uut.BufferedChanges.Where(change => change.Type is KeyedChangeType.Addition).Select(change => change.AsAddition()).Should().BeEquivalentTo(additions, options => options.WithoutStrictOrdering(), "all given items whose keys were not present within the collection should have been added");
        _uut.BufferedChanges.Where(change => change.Type is KeyedChangeType.Replacement).Select(change => change.AsReplacement()).Should().BeEquivalentTo(replacements, options => options.WithoutStrictOrdering(), "all given items not present in the collection, but whose keys were, should have been replaced");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "a combination of addition and replacement operations, should produce an update");
        
        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();
        
        capturedChangeSet.Changes.Length.Should().Be(additions.Length + replacements.Count, "all given items not present in the collection should have been merged into it");
        capturedChangeSet.Changes.Where(change => change.Type is KeyedChangeType.Addition).Select(change => change.AsAddition()).Should().BeEquivalentTo(additions, options => options.WithoutStrictOrdering(), "all given items whose keys were not present within the collection should have been added");
        capturedChangeSet.Changes.Where(change => change.Type is KeyedChangeType.Replacement).Select(change => change.AsReplacement()).Should().BeEquivalentTo(replacements, options => options.WithoutStrictOrdering(), "all given items not present in the collection, but whose keys were, should have been replaced");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "a combination of addition and replacement operations, should produce an update");
        
        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertItemsWereRemoved(IReadOnlyList<TestItem> removedItems)
    {
        var removals = removedItems
            .Select(removedItem => new KeyedItem<string, TestItem>()
            {
                Key     = _keySelector.Invoke(removedItem),
                Item    = removedItem
            })
            .ToArray();
    
        _uut.BufferedChanges.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "items should only have been removed");
        _uut.BufferedChanges.Select(change => change.AsRemoval()).Should().BeEquivalentTo(removals, options => options.WithoutStrictOrdering(), "the given items should have been removed from the collection");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "removing items from a collection, without emptying it, should produce an update");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();
        
        capturedChangeSet.Changes.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "items should only have been removed");
        capturedChangeSet.Changes.Select(change => change.AsRemoval()).Should().BeEquivalentTo(removals, options => options.WithoutStrictOrdering(), "the given items should have been removed from the collection");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "removing items from a collection, without emptying it, should produce an update");

        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertKeyWasRefreshed(string refreshedKey)
    {
        _uut.BufferedChanges.Should().ContainSingle("a single change was made");
        _uut.BufferedChanges[0].Type.Should().Be(KeyedChangeType.Refreshment, "a single refreshment was performed");
        _uut.BufferedChanges[0].AsRefreshment().Key.Should().Be(refreshedKey, "the given key should have been retrieved");
        _uut.BufferedChanges[0].AsRefreshment().Item.Key.Should().Be(refreshedKey, "the given key's item should have been retrieved");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "refreshing an item should produce an update");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();
        
        capturedChangeSet.Changes.Should().ContainSingle("a single change was made");
        capturedChangeSet.Changes[0].Type.Should().Be(KeyedChangeType.Refreshment, "a single refreshment was performed");
        capturedChangeSet.Changes[0].AsRefreshment().Key.Should().Be(refreshedKey, "the given key should have been retrieved");
        capturedChangeSet.Changes[0].AsRefreshment().Item.Key.Should().Be(refreshedKey, "the given key's item should have been retrieved");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "refreshing an item should produce an update");
        
        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertUutDidNothing()
        => _uut.BufferedChanges.Should().BeEmpty("no changes should have been made");

    public void AssertUutWasCleared(IReadOnlyList<TestItem> removedItems)
    {
        var removals = removedItems
            .Select(removedItem => new KeyedItem<string, TestItem>()
            {
                Key     = _keySelector.Invoke(removedItem),
                Item    = removedItem
            })
            .ToArray();

        _uut.BufferedChanges.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "items should only have been removed");
        _uut.BufferedChanges.Select(change => change.AsRemoval()).Should().BeEquivalentTo(removals, options => options.WithoutStrictOrdering(), "all items should have been removed");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Clear, "removing all items from a dictionary should produce a clear");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();

        capturedChangeSet.Changes.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "items should only have been removed");
        capturedChangeSet.Changes.Select(change => change.AsRemoval()).Should().BeEquivalentTo(removals, options => options.WithoutStrictOrdering(), "all items should have been removed");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Clear, "removing all items from a dictionary should produce a clear");
        
        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertUutWasReset(
        IReadOnlyList<TestItem> removedItems,
        IReadOnlyList<TestItem> addedItems)
    {
        var removals = removedItems
            .Select(removedItem => new KeyedItem<string, TestItem>()
            {
                Key     = _keySelector.Invoke(removedItem),
                Item    = removedItem
            })
            .ToArray();

        var additions = addedItems
            .Select(addedItem => new KeyedItem<string, TestItem>()
            {
                Key     = _keySelector.Invoke(addedItem),
                Item    = addedItem
            })
            .ToArray();

        _uut.BufferedChanges.Take(removedItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "all existing items should have been removed");
        _uut.BufferedChanges.Take(removedItems.Count).Select(change => change.AsRemoval()).Should().BeEquivalentTo(removals, options => options.WithoutStrictOrdering(), "all existing items should have been removed");
        _uut.BufferedChanges.Skip(removedItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "all given items should have been added");
        _uut.BufferedChanges.Skip(removedItems.Count).Select(change => change.AsAddition()).Should().BeEquivalentTo(additions, options => options.WithoutStrictOrdering(), "all given items should have been added");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Reset, "removing all items in a set, then adding new items, should produce a reset");
        
        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();
        
        capturedChangeSet.Changes.Take(removedItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "all existing items should have been removed");
        capturedChangeSet.Changes.Take(removedItems.Count).Select(change => change.AsRemoval()).Should().BeEquivalentTo(removals, options => options.WithoutStrictOrdering(), "all existing items should have been removed");
        capturedChangeSet.Changes.Skip(removedItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "all given items should have been added");
        capturedChangeSet.Changes.Skip(removedItems.Count).Select(change => change.AsAddition()).Should().BeEquivalentTo(additions, options => options.WithoutStrictOrdering(), "all given items should have been added");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Reset, "removing all items in a set, then adding new items, should produce a reset");
        
        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void Dispose() { }

    public void MergeRangeIntoUut(IEnumerable<TestItem> items)
        => _uut.MergeRange(items);

    public bool RefreshUutItem(TestItem item)
        => _uut.Refresh(item);

    public bool RefreshUutKey(string key)
        => _uut.RefreshKey(key);

    public void RemoveRangeFromUut(IEnumerable<TestItem> items)
        => _uut.RemoveRange(items);

    public void ResetUut(IEnumerable<TestItem> items)
        => _uut.Reset(items);

    private readonly Func<TestItem, string>                 _keySelector;
    private readonly ChangeTrackingCache<string, TestItem>  _uut;
}
