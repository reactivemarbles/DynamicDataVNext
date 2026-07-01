using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

public sealed class UutFixture
    : ISetUutFixture<UutFixture, ChangeTrackingHashSet<int>>,
        IReadOnlySetUutFixture<UutFixture, ChangeTrackingHashSet<int>>
{
    public static UutFixture Create(
            IEqualityComparer<int>?     comparer    = null,
            DistinctItemOptions  options     = default)
        => new(new ChangeTrackingHashSet<int>(
            comparer:   comparer,
            options:    options));

    public static UutFixture Create(
            int                         capacity,
            IEqualityComparer<int>?     comparer    = null,
            DistinctItemOptions  options     = default)
        => new(new ChangeTrackingHashSet<int>(
            capacity:   capacity,
            comparer:   comparer,
            options:    options));

    public static UutFixture Create(
            IEnumerable<int>            items,
            IEqualityComparer<int>?     comparer    = null,
            DistinctItemOptions  options     = default)
        => new(new ChangeTrackingHashSet<int>(
            items:      items,
            comparer:   comparer,
            options:    options));

    private UutFixture(ChangeTrackingHashSet<int> uut)
        => _uut = uut;
    
    public ChangeTrackingHashSet<int> Uut
        => _uut;

    public int UutCapacity
        => _uut.Capacity;

    public IEqualityComparer<int> UutComparer
        => _uut.Comparer;
    
    public DistinctItemOptions UutOptions
        => _uut.Options;

    public void Dispose() { }

    public void AssertItemWasAdded(int addedItem)
    {
        _uut.BufferedChanges.Should().ContainSingle("a single change was made");
        _uut.BufferedChanges[0].Type.Should().Be(DistinctChangeType.Addition, "a single addition was performed");
        _uut.BufferedChanges[0].Item.Should().Be(addedItem, "the given item should have been added");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "adding an item to a non-empty set should produce an update");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();
        
        capturedChangeSet.Changes.Should().ContainSingle("a single change was made");
        capturedChangeSet.Changes[0].Type.Should().Be(DistinctChangeType.Addition, "a single addition was performed");
        capturedChangeSet.Changes[0].Item.Should().Be(addedItem, "the given item should have been added");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "adding an item to a non-empty set should produce an update");
        
        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertItemWasRemoved(int removedItem)
    {
        _uut.BufferedChanges.Should().ContainSingle("a single change was made");
        _uut.BufferedChanges[0].Type.Should().Be(DistinctChangeType.Removal, "a single removal was performed");
        _uut.BufferedChanges[0].Item.Should().Be(removedItem, "the given item should have been removed");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "removing an item from a set of multiple items should produce an update");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();
        
        capturedChangeSet.Changes.Should().ContainSingle("a single change was made");
        capturedChangeSet.Changes[0].Type.Should().Be(DistinctChangeType.Removal, "a single removal was performed");
        capturedChangeSet.Changes[0].Item.Should().Be(removedItem, "the given item should have been removed");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "removing an item from a set of multiple items should produce an update");
        
        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertItemsWereAdded(IReadOnlyList<int> addedItems)
    {
        _uut.BufferedChanges.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "items should only have been added");
        _uut.BufferedChanges.Select(change => change.Item).Should().BeEquivalentTo(addedItems, options => options.WithoutStrictOrdering(), "items not already in the set should have been added");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "adding items to a non-empty set should produce an update");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();
        
        capturedChangeSet.Changes.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "items should only have been added");
        capturedChangeSet.Changes.Select(change => change.Item).Should().BeEquivalentTo(addedItems, options => options.WithoutStrictOrdering(), "items not already in the set should have been added");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "adding items to a non-empty set should produce an update");
        
        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertItemsWereRemoved(
        IReadOnlyList<int>  removedItems,
        string              because)
    {
        _uut.BufferedChanges.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "items should only have been removed");
        _uut.BufferedChanges.Select(change => change.Item).Should().BeEquivalentTo(removedItems, options => options.WithoutStrictOrdering(), because);
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "removing items from a set, without removing them all, should produce an update");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();
        
        capturedChangeSet.Changes.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "items should only have been removed");
        capturedChangeSet.Changes.Select(change => change.Item).Should().BeEquivalentTo(removedItems, options => options.WithoutStrictOrdering(), because);
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "removing items from a set, without removing them all, should produce an update");
        
        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertUutDidNothing()
        => _uut.BufferedChanges.Should().BeEmpty("no changes should have been made");

    public void AssertUutWasCleared(IReadOnlyList<int> items)
    {
        _uut.BufferedChanges.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "items should only have been removed");
        _uut.BufferedChanges.Select(change => change.Item).Should().BeEquivalentTo(items, options => options.WithoutStrictOrdering(), "all items should have been removed");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Clear, "removing all items from a set should produce a clear");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();

        capturedChangeSet.Changes.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "items should only have been removed");
        capturedChangeSet.Changes.Select(change => change.Item).Should().BeEquivalentTo(items, options => options.WithoutStrictOrdering(), "all items should have been removed");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Clear, "removing all items from a set should produce a clear");
        
        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertUutWasReset(
        IReadOnlyList<int> oldItems,
        IReadOnlyList<int> newItems)
    {
        _uut.BufferedChanges.Take(oldItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "all existing items should have been removed");
        _uut.BufferedChanges.Take(oldItems.Count).Select(change => change.Item).Should().BeEquivalentTo(oldItems, options => options.WithoutStrictOrdering(), "all existing items should have been removed");
        _uut.BufferedChanges.Skip(oldItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "all given items should have been added");
        _uut.BufferedChanges.Skip(oldItems.Count).Select(change => change.Item).Should().BeEquivalentTo(newItems, options => options.WithoutStrictOrdering(), "all given items should have been added");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Reset, "removing all items in a set, then adding new items, should produce a reset");
        
        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();
        
        capturedChangeSet.Changes.Take(oldItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "all existing items should have been removed");
        capturedChangeSet.Changes.Take(oldItems.Count).Select(change => change.Item).Should().BeEquivalentTo(oldItems, options => options.WithoutStrictOrdering(), "all existing items should have been removed");
        capturedChangeSet.Changes.Skip(oldItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "all given items should have been added");
        capturedChangeSet.Changes.Skip(oldItems.Count).Select(change => change.Item).Should().BeEquivalentTo(newItems, options => options.WithoutStrictOrdering(), "all given items should have been added");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Reset, "removing all items in a set, then adding new items, should produce a reset");
        
        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertUutWasUpdated(
        IReadOnlyList<int>  removedItems,
        IReadOnlyList<int>  addedItems,
        string              itemsRemovedBecause)
    {
        _uut.BufferedChanges.Take(removedItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, itemsRemovedBecause);
        _uut.BufferedChanges.Take(removedItems.Count).Select(change => change.Item).Should().BeEquivalentTo(removedItems, options => options.WithoutStrictOrdering(), itemsRemovedBecause);
        _uut.BufferedChanges.Skip(removedItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "items not already in the set should have been added");
        _uut.BufferedChanges.Skip(removedItems.Count).Select(change => change.Item).Should().BeEquivalentTo(addedItems, options => options.WithoutStrictOrdering(), "items not already in the set should have been added");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "removing items from a set, without removing all of them, then adding new items, should produce an update");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();
        
        capturedChangeSet.Changes.Take(removedItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, itemsRemovedBecause);
        capturedChangeSet.Changes.Take(removedItems.Count).Select(change => change.Item).Should().BeEquivalentTo(removedItems, options => options.WithoutStrictOrdering(), itemsRemovedBecause);
        capturedChangeSet.Changes.Skip(removedItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "items not already in the set should have been added");
        capturedChangeSet.Changes.Skip(removedItems.Count).Select(change => change.Item).Should().BeEquivalentTo(addedItems, options => options.WithoutStrictOrdering(), "items not already in the set should have been added");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "removing items from a set, without removing all of them, then adding new items, should produce an update");
        
        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }
    
    public void ResetUut(IEnumerable<int> items)
        => _uut.Reset(items);

    private readonly ChangeTrackingHashSet<int> _uut;
}
