using DynamicDataVNext.Tests.Ordered.ListTestBases;

namespace DynamicDataVNext.Tests.Ordered.ChangeTrackingListTests;

public sealed class UutFixture
    : IListUutFixture<UutFixture, ChangeTrackingList<string?>>,
        IReadOnlyListUutFixture<UutFixture, ChangeTrackingList<string?>>
{
    public static UutFixture Create(OrderedItemOptions options = default)
        => new(new ChangeTrackingList<string?>(options: options));

    public static UutFixture Create(
            int                     capacity,
            OrderedItemOptions      options     = default)
        => new(new ChangeTrackingList<string?>(
            capacity:   capacity,
            options:    options));

    public static UutFixture Create(
            IEnumerable<string?>    items,
            OrderedItemOptions      options = default)
        => new(new ChangeTrackingList<string?>(
            items:      items,
            options:    options));

    private UutFixture(ChangeTrackingList<string?> uut)
        => _uut = uut;
    
    public ChangeTrackingList<string?> Uut
        => _uut;

    public int UutCapacity
        => _uut.Capacity;

    public OrderedItemOptions UutOptions
        => _uut.Options;

    public void AssertItemsWereInserted(
        int                     insertionIndex,
        IReadOnlyList<string?>  insertedItems)
    {
        var insertions = insertedItems
            .Select((item, index) => new OrderedItem<string?>()
            {
                Index   = index + insertionIndex,
                Item    = item
            })
            .ToArray();

        _uut.BufferedChanges.Count.Should().Be(insertedItems.Count, "a single change should have been buffered for each given item");
        _uut.BufferedChanges.Select(change => change.Type).Should().AllBeEquivalentTo(OrderedChangeType.Insertion, "an insertion should have been performed for each given item");
        _uut.BufferedChanges.Select(change => change.AsInsertion()).Should().BeEquivalentTo(insertions, options => options.WithStrictOrdering(), "all given items should have been inserted");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "adding items to a non-empty collection should produce an update");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();

        capturedChangeSet.Changes.Length.Should().Be(insertedItems.Count, "a single change should have been buffered for each given item");
        capturedChangeSet.Changes.Select(change => change.Type).Should().AllBeEquivalentTo(OrderedChangeType.Insertion, "an insertion should have been performed for each given item");
        capturedChangeSet.Changes.Select(change => change.AsInsertion()).Should().BeEquivalentTo(insertions, options => options.WithStrictOrdering(), "all given items should have been inserted");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "adding items to a non-empty collection should produce an update");

        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertItemsWereRemoved(IReadOnlyList<OrderedItem<string?>> removals)
    {
        _uut.BufferedChanges.Count.Should().Be(removals.Count, "a single change should have been buffered for each removed item");
        _uut.BufferedChanges.Select(change => change.Type).Should().AllBeEquivalentTo(OrderedChangeType.Removal, "a removal should have been performed for each given item");
        _uut.BufferedChanges.Select(change => change.AsRemoval()).Should().BeEquivalentTo(removals, options => options.WithStrictOrdering(), "all removals performed should have been recorded");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "removing items from a collection should produce an update, unless all items are removed, in reverse order, and no other changes are made");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();

        capturedChangeSet.Changes.Length.Should().Be(removals.Count, "a single change should have been buffered for each removed item");
        capturedChangeSet.Changes.Select(change => change.Type).Should().AllBeEquivalentTo(OrderedChangeType.Removal, "a removal should have been performed for each given item");
        capturedChangeSet.Changes.Select(change => change.AsRemoval()).Should().BeEquivalentTo(removals, options => options.WithStrictOrdering(), "all removals performed should have been recorded");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "removing items from a collection should produce an update, unless all items are removed, in reverse order, and no other changes are made");

        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertItemWasInserted(
        int     insertionIndex,
        string? insertedItem)
    {
        _uut.BufferedChanges.Should().ContainSingle("a single change was made");
        _uut.BufferedChanges[0].Type.Should().Be(OrderedChangeType.Insertion, "a single insertion was performed");
        _uut.BufferedChanges[0].AsInsertion().Index.Should().Be(insertionIndex, "the insertion should have occurred at the given index");
        _uut.BufferedChanges[0].AsInsertion().Item.Should().Be(insertedItem, "the given item should have been inserted");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "adding an item to a non-empty collection should produce an update");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();

        capturedChangeSet.Changes.Should().ContainSingle("a single change was made");
        capturedChangeSet.Changes[0].Type.Should().Be(OrderedChangeType.Insertion, "a single insertion was performed");
        capturedChangeSet.Changes[0].AsInsertion().Index.Should().Be(insertionIndex, "the insertion should have occurred at the given index");
        capturedChangeSet.Changes[0].AsInsertion().Item.Should().Be(insertedItem, "the given item should have been inserted");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "adding an item to a non-empty collection should produce an update");

        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertItemWasMoved(
        int     oldIndex,
        int     newIndex,
        string? movedItem)
    {
        _uut.BufferedChanges.Should().ContainSingle("a single change was made");
        _uut.BufferedChanges[0].Type.Should().Be(OrderedChangeType.Movement, "a single movement was performed");
        _uut.BufferedChanges[0].AsMovement().OldIndex.Should().Be(oldIndex, "the movement should have occurred from the given old index");
        _uut.BufferedChanges[0].AsMovement().NewIndex.Should().Be(newIndex, "the movement should have occurred toward the given new index");
        _uut.BufferedChanges[0].AsMovement().Item.Should().Be(movedItem, "the item at the given old index should have been retrieved and moved");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "moving an item within a collection should produce an update");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();

        capturedChangeSet.Changes.Should().ContainSingle("a single change was made");
        capturedChangeSet.Changes[0].Type.Should().Be(OrderedChangeType.Movement, "a single movement was performed");
        capturedChangeSet.Changes[0].AsMovement().OldIndex.Should().Be(oldIndex, "the movement should have occurred from the given old index");
        capturedChangeSet.Changes[0].AsMovement().NewIndex.Should().Be(newIndex, "the movement should have occurred toward the given new index");
        capturedChangeSet.Changes[0].AsMovement().Item.Should().Be(movedItem, "the item at the given old index should have been retrieved and moved");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "moving an item within a collection should produce an update");

        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertItemWasRefreshed(
        int     refreshmentIndex,
        string? refreshedItem)
    {
        _uut.BufferedChanges.Should().ContainSingle("a single change was made");
        _uut.BufferedChanges[0].Type.Should().Be(OrderedChangeType.Refreshment, "a single refreshment was performed");
        _uut.BufferedChanges[0].AsRefreshment().Index.Should().Be(refreshmentIndex, "the refreshment should have occurred at the given index");
        _uut.BufferedChanges[0].AsRefreshment().Item.Should().Be(refreshedItem, "the item at the given index should have been refreshment");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "refreshing an item should produce an update");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();

        capturedChangeSet.Changes.Should().ContainSingle("a single change was made");
        capturedChangeSet.Changes[0].Type.Should().Be(OrderedChangeType.Refreshment, "a single refreshment was performed");
        capturedChangeSet.Changes[0].AsRefreshment().Index.Should().Be(refreshmentIndex, "the refreshment should have occurred at the given index");
        capturedChangeSet.Changes[0].AsRefreshment().Item.Should().Be(refreshedItem, "the item at the given index should have been refreshment");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "refreshing an item should produce an update");

        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertItemWasRemoved(
        int     removalIndex,
        string? removedItem)
    {
        _uut.BufferedChanges.Should().ContainSingle("a single change was made");
        _uut.BufferedChanges[0].Type.Should().Be(OrderedChangeType.Removal, "a single removal was performed");
        _uut.BufferedChanges[0].AsRemoval().Index.Should().Be(removalIndex, "the removal should have occurred at the given index");
        _uut.BufferedChanges[0].AsRemoval().Item.Should().Be(removedItem, "the item at the given index should have been removed");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "removing an item within a collection containing more than one item should produce an update");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();

        capturedChangeSet.Changes.Should().ContainSingle("a single change was made");
        capturedChangeSet.Changes[0].Type.Should().Be(OrderedChangeType.Removal, "a single removal was performed");
        capturedChangeSet.Changes[0].AsRemoval().Index.Should().Be(removalIndex, "the removal should have occurred at the given index");
        capturedChangeSet.Changes[0].AsRemoval().Item.Should().Be(removedItem, "the item at the given index should have been removed");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "removing an item within a collection containing more than one item should produce an update");

        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertItemWasReplaced(
        int     replacementIndex,
        string? replacedItem,
        string? replacementItem)
    {
        _uut.BufferedChanges.Should().ContainSingle("a single change was made");
        _uut.BufferedChanges[0].Type.Should().Be(OrderedChangeType.Replacement, "a single replacement was performed");
        _uut.BufferedChanges[0].AsReplacement().Index.Should().Be(replacementIndex, "the replacement should have occurred at the given index");
        _uut.BufferedChanges[0].AsReplacement().OldItem.Should().Be(replacedItem, "the previous item at the given index should have been recorded");
        _uut.BufferedChanges[0].AsReplacement().NewItem.Should().Be(replacementItem, "the given item should have replaced the previous one");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Update, "replacing an item within a collection should produce an update");

        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();

        capturedChangeSet.Changes.Should().ContainSingle("a single change was made");
        capturedChangeSet.Changes[0].Type.Should().Be(OrderedChangeType.Replacement, "a single replacement was performed");
        capturedChangeSet.Changes[0].AsReplacement().Index.Should().Be(replacementIndex, "the replacement should have occurred at the given index");
        capturedChangeSet.Changes[0].AsReplacement().OldItem.Should().Be(replacedItem, "the previous item at the given index should have been recorded");
        capturedChangeSet.Changes[0].AsReplacement().NewItem.Should().Be(replacementItem, "the given item should have replaced the previous one");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Update, "replacing an item within a collection should produce an update");

        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertUutDidNothing()
        => _uut.BufferedChanges.Should().BeEmpty("no changes should have been made");

    public void AssertUutWasCleared(IReadOnlyList<string?> removedItems)
    {
        var removals = removedItems
            .Select((item, index) => new OrderedItem<string?>()
            {
                Index   = index,
                Item    = item
            })
            .ToArray();

        _uut.BufferedChanges.Select(change => change.Type).Should().AllBeEquivalentTo(OrderedChangeType.Removal, "all existing items should have been removed");
        _uut.BufferedChanges.Select(change => change.AsRemoval()).Should().BeEquivalentTo(removals.Reverse(), options => options.WithStrictOrdering(), "all existing items should have been removed, in reverse order");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Clear, "removing all items in a collection should produce a clear");
        
        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();
        
        capturedChangeSet.Changes.Select(change => change.Type).Should().AllBeEquivalentTo(OrderedChangeType.Removal, "all existing items should have been removed");
        capturedChangeSet.Changes.Select(change => change.AsRemoval()).Should().BeEquivalentTo(removals.Reverse(), options => options.WithStrictOrdering(), "all existing items should have been removed, in reverse order");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Clear, "removing all items in a collection should produce a clear");
        
        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void AssertUutWasReset(
        IReadOnlyList<string?> removedItems,
        IReadOnlyList<string?> insertedItems)
    {
        var removals = removedItems
            .Select((item, index) => new OrderedItem<string?>()
            {
                Index   = index,
                Item    = item
            })
            .ToArray();

        var insertions = insertedItems
            .Select((item, index) => new OrderedItem<string?>()
            {
                Index   = index,
                Item    = item
            })
            .ToArray();

        _uut.BufferedChanges.Take(removedItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(OrderedChangeType.Removal, "all existing items should have been removed");
        _uut.BufferedChanges.Take(removedItems.Count).Select(change => change.AsRemoval()).Should().BeEquivalentTo(removals.Reverse(), options => options.WithStrictOrdering(), "all existing items should have been removed, in reverse order");
        _uut.BufferedChanges.Skip(removedItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(OrderedChangeType.Insertion, "all given items should have been added");
        _uut.BufferedChanges.Skip(removedItems.Count).Select(change => change.AsInsertion()).Should().BeEquivalentTo(insertions, options => options.WithStrictOrdering(), "all given items should have been added");
        _uut.BufferedChanges.CurrentSetType.Should().Be(ChangeSetType.Reset, "removing all items in a collection, then adding new items, should produce a reset");
        
        var capturedChangeSet = _uut.BufferedChanges.CaptureAndClear();
        
        capturedChangeSet.Changes.Take(removedItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(OrderedChangeType.Removal, "all existing items should have been removed");
        capturedChangeSet.Changes.Take(removedItems.Count).Select(change => change.AsRemoval()).Should().BeEquivalentTo(removals.Reverse(), options => options.WithStrictOrdering(), "all existing items should have been removed, in reverse order");
        capturedChangeSet.Changes.Skip(removedItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(OrderedChangeType.Insertion, "all given items should have been added");
        capturedChangeSet.Changes.Skip(removedItems.Count).Select(change => change.AsInsertion()).Should().BeEquivalentTo(insertions, options => options.WithStrictOrdering(), "all given items should have been added");
        capturedChangeSet.Type.Should().Be(ChangeSetType.Reset, "removing all items in a collection, then adding new items, should produce a reset");
        
        _uut.BufferedChanges.Should().BeEmpty("all changes should have been captured from the buffer");
    }

    public void Dispose() { }

    private readonly ChangeTrackingList<string?> _uut;
}
