using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;

using AwesomeAssertions;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public static partial class UutFixture
{
    public sealed class WhenNotificationsAreSuspended
        : ISetUutFixture<WhenNotificationsAreSuspended, ObservableHashSet<int>>,
            IReadOnlySetUutFixture<WhenNotificationsAreSuspended, ObservableHashSet<int>>
    {
        public static WhenNotificationsAreSuspended Create(
                IEqualityComparer<int>? comparer    = null,
                DistinctItemOptions     options     = default)
            => new(new ObservableHashSet<int>(
                comparer:   comparer,
                options:    options));

        public static WhenNotificationsAreSuspended Create(
                int                     capacity,
                IEqualityComparer<int>? comparer    = null,
                DistinctItemOptions     options     = default)
            => new(new ObservableHashSet<int>(
                capacity:   capacity,
                comparer:   comparer,
                options:    options));

        public static WhenNotificationsAreSuspended Create(
                IEnumerable<int>        items,
                IEqualityComparer<int>? comparer    = null,
                DistinctItemOptions     options     = default)
            => new(new ObservableHashSet<int>(
                items:      items,
                comparer:   comparer,
                options:    options));

        private WhenNotificationsAreSuspended(ObservableHashSet<int> uut)
        {
            _uut = uut;

            _collectionChangedSubscription = uut.CollectionChanged
                .RecordValues(out _collectionChangedResults);

            _uutSubscription = uut.ChangeStream
                .ValidateChangeSets()
                .RecordItems(out _uutResults);
            _uutResults.ClearNotifications();
            
            _suspension = uut.SuspendNotifications();
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
        {
            _suspension.Dispose();
            _collectionChangedSubscription.Dispose();
            _uutSubscription.Dispose();
        }

        public void AssertItemWasAdded(int addedItem)
        {
            AssertNotificationsSuspendedAndResumed();

            _uutResults.HasFinalized.Should().BeFalse("the set can still be changed");
            _uutResults.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _uutResults.RecordedChangeSets[0].Changes.Should().ContainSingle("a single item should have been added");
            _uutResults.RecordedChangeSets[0].Changes[0].Type.Should().Be(DistinctChangeType.Addition, "a single item should have been added");
            _uutResults.RecordedChangeSets[0].Changes[0].Item.Should().Be(addedItem, "the given item should have been added to the set");
            _uutResults.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Update, "adding an item to a non-empty set should produce an update");
            _uutResults.RecordedItems.Should().BeEquivalentTo(_uut, options => options.WithoutStrictOrdering(), "collecting published changes should reproduce the source collection");
        }

        public void AssertItemWasRemoved(int removedItem)
        {
            AssertNotificationsSuspendedAndResumed();

            _uutResults.HasFinalized.Should().BeFalse("the set can still be changed");
            _uutResults.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _uutResults.RecordedChangeSets[0].Changes.Should().ContainSingle("a single item should have been removed");
            _uutResults.RecordedChangeSets[0].Changes[0].Type.Should().Be(DistinctChangeType.Removal, "a single item should have been removed");
            _uutResults.RecordedChangeSets[0].Changes[0].Item.Should().Be(removedItem, "the given item should have been removed from the set");
            _uutResults.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Update, "removing an item from a set with multiple items should produce an update");
            _uutResults.RecordedItems.Should().BeEquivalentTo(_uut, options => options.WithoutStrictOrdering(), "collecting published changes should reproduce the source collection");
        }

        public void AssertItemsWereAdded(IReadOnlyList<int> addedItems)
        {
            AssertNotificationsSuspendedAndResumed();

            _uutResults.HasFinalized.Should().BeFalse("the set can still be changed");
            _uutResults.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _uutResults.RecordedChangeSets[0].Changes.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "items should only have been added");
            _uutResults.RecordedChangeSets[0].Changes.Select(change => change.Item).Should().BeEquivalentTo(addedItems, options => options.WithoutStrictOrdering(), "items not already in the set should have been added");
            _uutResults.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Update, "adding items to a non-empty set should produce an update");
            _uutResults.RecordedItems.Should().BeEquivalentTo(_uut, options => options.WithoutStrictOrdering(), "collecting published changes should reproduce the source collection");
        }

        public void AssertItemsWereRemoved(
            IReadOnlyList<int>  removedItems,
            string              because)
        {
            AssertNotificationsSuspendedAndResumed();

            _uutResults.HasFinalized.Should().BeFalse("the set can still be changed");
            _uutResults.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _uutResults.RecordedChangeSets[0].Changes.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "items should only have been added");
            _uutResults.RecordedChangeSets[0].Changes.Select(change => change.Item).Should().BeEquivalentTo(removedItems, options => options.WithoutStrictOrdering(), because);
            _uutResults.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Update, "removing some items, but not all, from a set should produce a update");
            _uutResults.RecordedItems.Should().BeEquivalentTo(_uut, options => options.WithoutStrictOrdering(), "collecting published changes should reproduce the source collection");
        }

        public void AssertUutDidNothing()
        {
            _collectionChangedResults.RecordedNotifications.Should().BeEmpty("notifications should have been suspended");
            _uutResults.RecordedNotifications.Should().BeEmpty("notifications should have been suspended");

            _suspension.Dispose();

            _collectionChangedResults.RecordedNotifications.Should().BeEmpty("the set should not have been changed");
            _uutResults.RecordedNotifications.Should().BeEmpty("the set should not have been changed");
        }

        public void AssertUutWasCleared(IReadOnlyList<int> items)
        {
            AssertNotificationsSuspendedAndResumed();

            _uutResults.HasFinalized.Should().BeFalse("the set can still be changed");
            _uutResults.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _uutResults.RecordedChangeSets[0].Changes.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "items should only have been removed");
            _uutResults.RecordedChangeSets[0].Changes.Select(change => change.Item).Should().BeEquivalentTo(items, options => options.WithoutStrictOrdering(), "all items in the set should have been removed");
            _uutResults.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Clear, "removing all items in a set should produce a clear");
            _uutResults.RecordedItems.Should().BeEmpty("all items in the set should have been removed");
        }

        public void AssertUutWasReset(
            IReadOnlyList<int> oldItems,
            IReadOnlyList<int> newItems)
        {
            AssertNotificationsSuspendedAndResumed();
            
            _uutResults.HasFinalized.Should().BeFalse("the set can still be changed");
            _uutResults.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _uutResults.RecordedChangeSets[0].Changes.Take(oldItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "all removals should have occurred before any additions");
            _uutResults.RecordedChangeSets[0].Changes.Take(oldItems.Count).Select(change => change.Item).Should().BeEquivalentTo(oldItems, options => options.WithoutStrictOrdering(), "all existing items in the set should have been removed");
            _uutResults.RecordedChangeSets[0].Changes.Skip(oldItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "all additions should have occurred before any removals");
            _uutResults.RecordedChangeSets[0].Changes.Skip(oldItems.Count).Select(change => change.Item).Should().BeEquivalentTo(newItems, options => options.WithoutStrictOrdering(), "all given items should have been added to the set");
            _uutResults.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Reset, (oldItems.Count is 0)
                ? "adding items to an empty set should produce a reset"
                : "removing all items from a set, and then adding new ones, should produce a reset");
            _uutResults.RecordedItems.Should().BeEquivalentTo(_uut, options => options.WithoutStrictOrdering(), "collecting published changes should reproduce the source collection");
        }

        public void AssertUutWasUpdated(
            IReadOnlyList<int>  removedItems,
            IReadOnlyList<int>  addedItems,
            string              itemsRemovedBecause)
        {
            AssertNotificationsSuspendedAndResumed();

            _uutResults.HasFinalized.Should().BeFalse("the set can still be changed");
            _uutResults.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _uutResults.RecordedChangeSets[0].Changes.Take(removedItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "all removals should have occurred before any additions");
            _uutResults.RecordedChangeSets[0].Changes.Take(removedItems.Count).Select(change => change.Item).Should().BeEquivalentTo(removedItems, options => options.WithoutStrictOrdering(), itemsRemovedBecause);
            _uutResults.RecordedChangeSets[0].Changes.Skip(removedItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "all additions should have occurred before any removals");
            _uutResults.RecordedChangeSets[0].Changes.Skip(removedItems.Count).Select(change => change.Item).Should().BeEquivalentTo(addedItems, options => options.WithoutStrictOrdering(), "items not already in the set should have been added");
            _uutResults.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Update, "removing items from a set, without clearing it, and then adding items to it, should produce an update");
            _uutResults.RecordedItems.Should().BeEquivalentTo(_uut, options => options.WithoutStrictOrdering(), "collecting published changes should reproduce the source collection");
        }
        
        public void ResetUut(IEnumerable<int> items)
            => _uut.Reset(items);

        private void AssertNotificationsSuspendedAndResumed()
        {
            _collectionChangedResults.RecordedNotifications.Should().BeEmpty("notifications should have been suspended");
            _uutResults.RecordedNotifications.Should().BeEmpty("notifications should have been suspended");

            _suspension.Dispose();

            _collectionChangedResults.HasFinalized.Should().BeFalse("the set can still be changed");
            _collectionChangedResults.RecordedValues.Should().ContainSingle("a single change operation was performed");
        }
        
        private readonly ValueRecordingObserver<Unit>       _collectionChangedResults;
        private readonly IDisposable                        _collectionChangedSubscription;
        private readonly ObservableHashSet<int>             _uut;
        private readonly DistinctItemRecordingObserver<int> _uutResults;
        private readonly IDisposable                        _uutSubscription;
    
        private ObservableHashSet<int>.Suspension _suspension;
    }
}
