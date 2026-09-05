using DynamicDataVNext.Tests.Distinct.SetTestBases;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public static partial class UutFixture
{
    public sealed class WhenChangeStreamSourceHasSubscribers
        : ISetUutFixture<WhenChangeStreamSourceHasSubscribers, ObservableHashSet<int>>,
            IReadOnlySetUutFixture<WhenChangeStreamSourceHasSubscribers, ObservableHashSet<int>>
    {
        public static WhenChangeStreamSourceHasSubscribers Create(
                IEqualityComparer<int>? comparer    = null,
                DistinctItemOptions     options     = default)
            => new(new ObservableHashSet<int>(
                comparer:   comparer,
                options:    options));

        public static WhenChangeStreamSourceHasSubscribers Create(
                int                     capacity,
                IEqualityComparer<int>? comparer    = null,
                DistinctItemOptions     options     = default)
            => new(new ObservableHashSet<int>(
                capacity:   capacity,
                comparer:   comparer,
                options:    options));

        public static WhenChangeStreamSourceHasSubscribers Create(
                IEnumerable<int>        items,
                IEqualityComparer<int>? comparer    = null,
                DistinctItemOptions     options     = default)
            => new(new ObservableHashSet<int>(
                items:      items,
                comparer:   comparer,
                options:    options));

        private WhenChangeStreamSourceHasSubscribers(ObservableHashSet<int> uut)
        {
            _uut = uut;

            _subscription = uut.ChangeStream
                .ValidateChangeSets()
                .RecordItems(out _results);
            _results.ClearNotifications();       
        }
        
        public ObservableHashSet<int> Uut
            => _uut;

        public int UutCapacity
            => _uut.Capacity;

        public IEqualityComparer<int> UutComparer
            => _uut.ChangeStream.Comparer;
        
        public DistinctItemOptions UutOptions
            => _uut.ChangeStream.Options;
        
        public void Dispose()
            => _subscription.Dispose();

        public void AssertItemWasAdded(int addedItem)
        {
            _results.HasFinalized.Should().BeFalse("the set can still be changed");
            _results.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _results.RecordedChangeSets[0].Changes.Should().ContainSingle("a single item should have been added");
            _results.RecordedChangeSets[0].Changes[0].Type.Should().Be(DistinctChangeType.Addition, "a single item should have been added");
            _results.RecordedChangeSets[0].Changes[0].Item.Should().Be(addedItem, "the given item should have been added to the set");
            _results.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Update, "adding an item to a non-empty set should produce an update");
            _results.RecordedItems.Should().BeEquivalentTo(_uut, options => options.WithoutStrictOrdering(), "collecting published changes should reproduce the source collection");
        }

        public void AssertItemWasRefreshed(int refreshedItem)
        {
            _results.HasFinalized.Should().BeFalse("the set can still be changed");
            _results.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _results.RecordedChangeSets[0].Changes.Should().ContainSingle("a single item should have been refreshed");
            _results.RecordedChangeSets[0].Changes[0].Type.Should().Be(DistinctChangeType.Refreshment, "a single item should have been refreshed");
            _results.RecordedChangeSets[0].Changes[0].Item.Should().Be(refreshedItem, "the given item should have been refreshed");
            _results.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Update, "refreshing an item should produce an update");
            _results.RecordedItems.Should().BeEquivalentTo(_uut, options => options.WithoutStrictOrdering(), "collecting published changes should reproduce the source collection");
        }

        public void AssertItemWasRemoved(int removedItem)
        {
            _results.HasFinalized.Should().BeFalse("the set can still be changed");
            _results.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _results.RecordedChangeSets[0].Changes.Should().ContainSingle("a single item should have been removed");
            _results.RecordedChangeSets[0].Changes[0].Type.Should().Be(DistinctChangeType.Removal, "a single item should have been removed");
            _results.RecordedChangeSets[0].Changes[0].Item.Should().Be(removedItem, "the given item should have been removed from the set");
            _results.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Update, "removing an item from a set with multiple items should produce an update");
            _results.RecordedItems.Should().BeEquivalentTo(_uut, options => options.WithoutStrictOrdering(), "collecting published changes should reproduce the source collection");
        }

        public void AssertItemsWereAdded(IReadOnlyList<int> addedItems)
        {
            _results.HasFinalized.Should().BeFalse("the set can still be changed");
            _results.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _results.RecordedChangeSets[0].Changes.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "items should only have been added");
            _results.RecordedChangeSets[0].Changes.Select(change => change.Item).Should().BeEquivalentTo(addedItems, options => options.WithoutStrictOrdering(), "items not already in the set should have been added");
            _results.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Update, "adding items to a non-empty set should produce an update");
            _results.RecordedItems.Should().BeEquivalentTo(_uut, options => options.WithoutStrictOrdering(), "collecting published changes should reproduce the source collection");
        }

        public void AssertItemsWereRemoved(
            IReadOnlyList<int>  removedItems,
            string              because)
        {
            _results.HasFinalized.Should().BeFalse("the set can still be changed");
            _results.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _results.RecordedChangeSets[0].Changes.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "items should only have been added");
            _results.RecordedChangeSets[0].Changes.Select(change => change.Item).Should().BeEquivalentTo(removedItems, options => options.WithoutStrictOrdering(), because);
            _results.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Update, "removing some items, but not all, from a set should produce a update");
            _results.RecordedItems.Should().BeEquivalentTo(_uut, options => options.WithoutStrictOrdering(), "collecting published changes should reproduce the source collection");
        }

        public void AssertUutDidNothing()
            => _results.RecordedNotifications.Should().BeEmpty("the set should not have been changed");

        public void AssertUutWasCleared(IReadOnlyList<int> items)
        {
            _results.HasFinalized.Should().BeFalse("the set can still be changed");
            _results.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _results.RecordedChangeSets[0].Changes.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "items should only have been removed");
            _results.RecordedChangeSets[0].Changes.Select(change => change.Item).Should().BeEquivalentTo(items, options => options.WithoutStrictOrdering(), "all items in the set should have been removed");
            _results.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Clear, "removing all items in a set should produce a clear");
            _results.RecordedItems.Should().BeEmpty("all items in the set should have been removed");
        }

        public void AssertUutWasReset(
            IReadOnlyList<int> oldItems,
            IReadOnlyList<int> newItems)
        {
            _results.HasFinalized.Should().BeFalse("the set can still be changed");
            _results.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _results.RecordedChangeSets[0].Changes.Take(oldItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "all removals should have occurred before any additions");
            _results.RecordedChangeSets[0].Changes.Take(oldItems.Count).Select(change => change.Item).Should().BeEquivalentTo(oldItems, options => options.WithoutStrictOrdering(), "all existing items in the set should have been removed");
            _results.RecordedChangeSets[0].Changes.Skip(oldItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "all additions should have occurred before any removals");
            _results.RecordedChangeSets[0].Changes.Skip(oldItems.Count).Select(change => change.Item).Should().BeEquivalentTo(newItems, options => options.WithoutStrictOrdering(), "all given items should have been added to the set");
            _results.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Reset, (oldItems.Count is 0)
                ? "adding items to an empty set should produce a reset"
                : "removing all items from a set, and then adding new ones, should produce a reset");
            _results.RecordedItems.Should().BeEquivalentTo(_uut, options => options.WithoutStrictOrdering(), "collecting published changes should reproduce the source collection");
        }

        public void AssertUutWasUpdated(
            IReadOnlyList<int>  removedItems,
            IReadOnlyList<int>  addedItems,
            string              itemsRemovedBecause)
        {
            _results.HasFinalized.Should().BeFalse("the set can still be changed");
            _results.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _results.RecordedChangeSets[0].Changes.Take(removedItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "all removals should have occurred before any additions");
            _results.RecordedChangeSets[0].Changes.Take(removedItems.Count).Select(change => change.Item).Should().BeEquivalentTo(removedItems, options => options.WithoutStrictOrdering(), itemsRemovedBecause);
            _results.RecordedChangeSets[0].Changes.Skip(removedItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "all additions should have occurred before any removals");
            _results.RecordedChangeSets[0].Changes.Skip(removedItems.Count).Select(change => change.Item).Should().BeEquivalentTo(addedItems, options => options.WithoutStrictOrdering(), "items not already in the set should have been added");
            _results.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Update, "removing items from a set, without clearing it, and then adding items to it, should produce an update");
            _results.RecordedItems.Should().BeEquivalentTo(_uut, options => options.WithoutStrictOrdering(), "collecting published changes should reproduce the source collection");
        }
        
        private readonly DistinctItemRecordingObserver<int> _results;
        private readonly IDisposable                        _subscription;
        private readonly ObservableHashSet<int>             _uut;
    }
}
