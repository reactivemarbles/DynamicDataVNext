using DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

public sealed class UutFixture
    : IDictionaryUutFixture<UutFixture, ChangeTrackingDictionary<string, int>>,
        IReadOnlyDictionaryUutFixture<UutFixture, ChangeTrackingDictionary<string, int>>
{
    public static UutFixture Create(
            IEqualityComparer<string>?  comparer    = null,
            KeyedItemOptions            options     = default)
        => new(new(
            comparer:   comparer,
            options:    options));

    public static UutFixture Create(
            int                         capacity,
            IEqualityComparer<string>?  comparer    = null,
            KeyedItemOptions            options     = default)
        => new(new(
            capacity:   capacity,
            comparer:   comparer,
            options:    options));

    public static UutFixture Create(
            IEnumerable<KeyValuePair<string, int>>  items,
            IEqualityComparer<string>?              comparer    = null,
            KeyedItemOptions                        options     = default)
        => new(new(
            items:      items,
            comparer:   comparer,
            options:    options));

    private UutFixture(ChangeTrackingDictionary<string, int> uut)
        => _uut = uut;
    
    public ChangeTrackingDictionary<string, int> Uut
        => _uut;

    public int UutCapacity
        => _uut.Capacity;

    public IEqualityComparer<string> UutComparer
        => _uut.Comparer;
    
    public KeyedItemOptions UutOptions
        => _uut.Options;

    public void AddRangeToUut(IEnumerable<KeyValuePair<string, int>> items)
        => _uut.AddRange(items);

    public void AddRangeToUut(
            IEnumerable<int>    values,
            Func<int, string>   keySelector)
        => _uut.AddRange(values, keySelector);

    public void AssertItemWasAdded(
        string  addedKey,
        int     addedValue)
    {
        _uut.BufferedChanges.Should().ContainSingle("a single change was made");
        _uut.BufferedChanges[0].Type.Should().Be(KeyedChangeType.Addition, "a single addition was performed");
        _uut.BufferedChanges[0].AsAddition().Key.Should().Be(addedKey, "the given item should have been added");
        _uut.BufferedChanges[0].AsAddition().Item.Should().Be(addedValue, "the given item should have been added");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "adding an item to a non-empty set should produce an update");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();
        
        capturedChangeSet.Changes.Should().ContainSingle("a single change was made");
        capturedChangeSet.Changes[0].Type.Should().Be(KeyedChangeType.Addition, "a single addition was performed");
        capturedChangeSet.Changes[0].AsAddition().Key.Should().Be(addedKey, "the given item should have been added");
        capturedChangeSet.Changes[0].AsAddition().Item.Should().Be(addedValue, "the given item should have been added");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "adding an item to a non-empty set should produce an update");
        
        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertItemWasRefreshed(
        string  key,
        int     value)
    {
        _uut.BufferedChanges.Should().ContainSingle("a single change was made");
        _uut.BufferedChanges[0].Type.Should().Be(KeyedChangeType.Refreshment, "a single refreshment was performed");
        _uut.BufferedChanges[0].AsRefreshment().Key.Should().Be(key, "the given item should have been added");
        _uut.BufferedChanges[0].AsRefreshment().Item.Should().Be(value, "the given item should have been added");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "refreshing an item should produce an update");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();
        
        capturedChangeSet.Changes.Should().ContainSingle("a single change was made");
        capturedChangeSet.Changes[0].Type.Should().Be(KeyedChangeType.Refreshment, "a single refreshment was performed");
        capturedChangeSet.Changes[0].AsRefreshment().Key.Should().Be(key, "the given item should have been added");
        capturedChangeSet.Changes[0].AsRefreshment().Item.Should().Be(value, "the given item should have been added");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "refreshing an item should produce an update");
        
        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertItemWasRemoved(
        string  removedKey,
        int     removedValue)
    {
        _uut.BufferedChanges.Should().ContainSingle("a single change was made");
        _uut.BufferedChanges[0].Type.Should().Be(KeyedChangeType.Removal, "a single removal was performed");
        _uut.BufferedChanges[0].AsRemoval().Key.Should().Be(removedKey, "the given item should have been removed");
        _uut.BufferedChanges[0].AsRemoval().Item.Should().Be(removedValue, "the given item should have been removed");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "removing an item from a collection of multiple items should produce an update");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();

        capturedChangeSet.Changes.Should().ContainSingle("a single change was made");
        capturedChangeSet.Changes[0].Type.Should().Be(KeyedChangeType.Removal, "a single removal was performed");
        capturedChangeSet.Changes[0].AsRemoval().Key.Should().Be(removedKey, "the given item should have been removed");
        capturedChangeSet.Changes[0].AsRemoval().Item.Should().Be(removedValue, "the given item should have been removed");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "removing an item from a collection of multiple items should produce an update");

        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertItemWasReplaced(
        string  replacementKey,
        int     replacedValue,
        int     replacementValue)
    {
        _uut.BufferedChanges.Should().ContainSingle("a single change was made");
        _uut.BufferedChanges[0].Type.Should().Be(KeyedChangeType.Replacement, "a single replacement was performed");
        _uut.BufferedChanges[0].AsReplacement().Key.Should().Be(replacementKey, "the replacement should have occurred for the given key");
        _uut.BufferedChanges[0].AsReplacement().OldItem.Should().Be(replacedValue, "the previous item at the given key should have been recorded");
        _uut.BufferedChanges[0].AsReplacement().NewItem.Should().Be(replacementValue, "the given item should have replaced the previous one");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "replacing an item within a collection should produce an update");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();

        capturedChangeSet.Changes.Should().ContainSingle("a single change was made");
        capturedChangeSet.Changes[0].Type.Should().Be(KeyedChangeType.Replacement, "a single replacement was performed");
        capturedChangeSet.Changes[0].AsReplacement().Key.Should().Be(replacementKey, "the replacement should have occurred for the given key");
        capturedChangeSet.Changes[0].AsReplacement().OldItem.Should().Be(replacedValue, "the previous item at the given key should have been recorded");
        capturedChangeSet.Changes[0].AsReplacement().NewItem.Should().Be(replacementValue, "the given item should have replaced the previous one");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "replacing an item within a collection should produce an update");

        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertItemsWereAdded(IReadOnlyList<KeyValuePair<string, int>> addedItems)
    {
        _uut.BufferedChanges.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "items should only have been added");
        _uut.BufferedChanges.Select(change => (KeyValuePair<string, int>)change.AsAddition()).Should().BeEquivalentTo(addedItems, options => options.WithoutStrictOrdering(), "items should have been added to the dictionary");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "adding items to a non-empty dictionary should produce an update");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();
        
        capturedChangeSet.Changes.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "items should only have been added");
        capturedChangeSet.Changes.Select(change => (KeyValuePair<string, int>)change.AsAddition()).Should().BeEquivalentTo(addedItems, options => options.WithoutStrictOrdering(), "items should have been added to the dictionary");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "adding items to a non-empty dictionary should produce an update");
        
        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertUutDidNothing()
        => _uut.BufferedChanges.Should().BeEmpty("no changes should have been made");

    public void AssertUutWasCleared(IReadOnlyList<KeyValuePair<string, int>> items)
    {
        _uut.BufferedChanges.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "items should only have been removed");
        _uut.BufferedChanges.Select(change => (KeyValuePair<string, int>)change.AsRemoval()).Should().BeEquivalentTo(items, options => options.WithoutStrictOrdering(), "all items should have been removed");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Clear, "removing all items from a dictionary should produce a clear");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();

        capturedChangeSet.Changes.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "items should only have been removed");
        capturedChangeSet.Changes.Select(change => (KeyValuePair<string, int>)change.AsRemoval()).Should().BeEquivalentTo(items, options => options.WithoutStrictOrdering(), "all items should have been removed");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Clear, "removing all items from a dictionary should produce a clear");
        
        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertUutWasReset(
        IReadOnlyList<KeyValuePair<string, int>> oldItems,
        IReadOnlyList<KeyValuePair<string, int>> newItems)
    {
        _uut.BufferedChanges.Take(oldItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "all existing items should have been removed");
        _uut.BufferedChanges.Take(oldItems.Count).Select(change => (KeyValuePair<string, int>)change.AsRemoval()).Should().BeEquivalentTo(oldItems, options => options.WithoutStrictOrdering(), "all existing items should have been removed");
        _uut.BufferedChanges.Skip(oldItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "all given items should have been added");
        _uut.BufferedChanges.Skip(oldItems.Count).Select(change => (KeyValuePair<string, int>)change.AsAddition()).Should().BeEquivalentTo(newItems, options => options.WithoutStrictOrdering(), "all given items should have been added");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Reset, "removing all items in a set, then adding new items, should produce a reset");
        
        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();
        
        capturedChangeSet.Changes.Take(oldItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "all existing items should have been removed");
        capturedChangeSet.Changes.Take(oldItems.Count).Select(change => (KeyValuePair<string, int>)change.AsRemoval()).Should().BeEquivalentTo(oldItems, options => options.WithoutStrictOrdering(), "all existing items should have been removed");
        capturedChangeSet.Changes.Skip(oldItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "all given items should have been added");
        capturedChangeSet.Changes.Skip(oldItems.Count).Select(change => (KeyValuePair<string, int>)change.AsAddition()).Should().BeEquivalentTo(newItems, options => options.WithoutStrictOrdering(), "all given items should have been added");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Reset, "removing all items in a set, then adding new items, should produce a reset");
        
        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void Dispose() { }

    public bool RefreshUut(string key)
        => _uut.Refresh(key);

    public void ResetUut(
            IEnumerable<int>    values,
            Func<int, string>   keySelector)
        => _uut.Reset(values, keySelector);

    private readonly ChangeTrackingDictionary<string, int> _uut;
}
