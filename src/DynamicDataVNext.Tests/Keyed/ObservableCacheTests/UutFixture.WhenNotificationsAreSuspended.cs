using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public static partial class UutFixture
{
    public sealed class WhenNotificationsAreSuspended
        : ICacheUutFixture<WhenNotificationsAreSuspended, ObservableCache<string, TestItem>>,
            IReadOnlyCacheUutFixture<WhenNotificationsAreSuspended, ObservableCache<string, TestItem>>
    {
        public static WhenNotificationsAreSuspended Create(
                Func<TestItem, string>      keySelector,
                IEqualityComparer<string>?  comparer    = null,
                KeyedItemOptions            options     = default)
            => new(
                uut:            new(
                    keySelector:    keySelector,
                    comparer:       comparer,
                    options:        options),
                keySelector:    keySelector);

        public static WhenNotificationsAreSuspended Create(
                int                         capacity,
                Func<TestItem, string>      keySelector,
                IEqualityComparer<string>?  comparer    = null,
                KeyedItemOptions            options     = default)
            => new(
                uut:            new(
                    capacity:       capacity,
                    keySelector:    keySelector,
                    comparer:       comparer,
                    options:        options),
                keySelector:    keySelector);

        public static WhenNotificationsAreSuspended Create(
                IEnumerable<TestItem>       items,
                Func<TestItem, string>      keySelector,
                IEqualityComparer<string>?  comparer    = null,
                KeyedItemOptions            options     = default)
            => new(
                uut:            new(
                    items:          items,
                    keySelector:    keySelector,
                    comparer:       comparer,
                    options:        options),
                keySelector:    keySelector);

        private WhenNotificationsAreSuspended(
            ObservableCache<string, TestItem>   uut,
            Func<TestItem, string>              keySelector)
        {
            _uut            = uut;
            _keySelector    = keySelector;

            _collectionChangedSubscription = uut.CollectionChanged
                .RecordValues(out _collectionChangedResults);

            _changeStreamSourceSubscription = uut.ChangeStream
                .ValidateChangeSets()
                .RecordItems(out _changeStreamSourceResults);
            _changeStreamSourceResults.ClearNotifications();
            
            _suspension = uut.SuspendNotifications();
        }
        
        public ObservableCache<string, TestItem> Uut
            => _uut;

        public int UutCapacity
            => _uut.Capacity;

        public IEqualityComparer<string> UutComparer
            => _uut.ChangeStream.KeyComparer;
        
        public KeyedItemOptions UutOptions
            => _uut.ChangeStream.Options;
        
        public void AssertItemWasAdded(TestItem addedItem)
        {
            AssertNotificationsSuspendedAndResumed();

            var addedKey = _keySelector.Invoke(addedItem);

            _changeStreamSourceResults.HasFinalized.Should().BeFalse("the dictionary can still be changed");
            _changeStreamSourceResults.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes.Should().ContainSingle("a single change was made");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes[0].Type.Should().Be(KeyedChangeType.Addition, "a single addition was performed");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes[0].AsAddition().Key.Should().Be(addedKey, "the given item should have been added");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes[0].AsAddition().Item.Should().Be(addedItem, "the given item should have been added");
            _changeStreamSourceResults.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Update, "adding an item to a non-empty set should produce an update");
            _changeStreamSourceResults.RecordedItems.Values.Should().BeEquivalentTo(_uut, "collecting published changes should reproduce the source collection");
        }

        public void AssertItemWasRefreshed(TestItem refreshedItem)
        {
            AssertNotificationsSuspendedAndResumed();

            var refreshedKey = _keySelector.Invoke(refreshedItem);

            _changeStreamSourceResults.HasFinalized.Should().BeFalse("the dictionary can still be changed");
            _changeStreamSourceResults.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes.Should().ContainSingle("a single change was made");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes[0].Type.Should().Be(KeyedChangeType.Refreshment, "a single refreshment was performed");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes[0].AsRefreshment().Key.Should().Be(refreshedKey, "the given item should have been refreshed");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes[0].AsRefreshment().Item.Should().Be(refreshedItem, "the given item should have been refreshed");
            _changeStreamSourceResults.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Update, "refreshing an item should produce an update");
            _changeStreamSourceResults.RecordedItems.Values.Should().BeEquivalentTo(_uut, "collecting published changes should reproduce the source collection");
        }

        public void AssertItemWasRemoved(TestItem removedItem)
        {
            AssertNotificationsSuspendedAndResumed();

            var removedKey = _keySelector.Invoke(removedItem);

            _changeStreamSourceResults.HasFinalized.Should().BeFalse("the dictionary can still be changed");
            _changeStreamSourceResults.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes.Should().ContainSingle("a single change was made");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes[0].Type.Should().Be(KeyedChangeType.Removal, "a single removal was performed");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes[0].AsRemoval().Key.Should().Be(removedKey, "the given item should have been removed");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes[0].AsRemoval().Item.Should().Be(removedItem, "the given item should have been removed");
            _changeStreamSourceResults.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Update, "removing an item from a collection of multiple items should produce an update");
            _changeStreamSourceResults.RecordedItems.Values.Should().BeEquivalentTo(_uut, "collecting published changes should reproduce the source collection");
        }

        public void AssertItemWasReplaced(
            TestItem oldItem,
            TestItem newItem)
        {
            AssertNotificationsSuspendedAndResumed();

            var replacementKey = _keySelector.Invoke(oldItem);

            _changeStreamSourceResults.HasFinalized.Should().BeFalse("the dictionary can still be changed");
            _changeStreamSourceResults.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes.Should().ContainSingle("a single change was made");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes[0].Type.Should().Be(KeyedChangeType.Replacement, "a single replacement was performed");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes[0].AsReplacement().Key.Should().Be(replacementKey, "the replacement should have occurred for the given key");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes[0].AsReplacement().OldItem.Should().Be(oldItem, "the previous item at the given key should have been recorded");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes[0].AsReplacement().NewItem.Should().Be(newItem, "the given item should have replaced the previous one");
            _changeStreamSourceResults.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Update, "replacing an item within a collection should produce an update");
            _changeStreamSourceResults.RecordedItems.Values.Should().BeEquivalentTo(_uut, "collecting published changes should reproduce the source collection");
        }

        public void AssertItemsWereAdded(IReadOnlyList<TestItem> addedItems)
        {
            AssertNotificationsSuspendedAndResumed();

            var additions = addedItems
                .Select(addedItem => new KeyedItem<string, TestItem>()
                {
                    Key     = _keySelector.Invoke(addedItem),
                    Item    = addedItem
                })
                .ToArray();

            _changeStreamSourceResults.HasFinalized.Should().BeFalse("the dictionary can still be changed");
            _changeStreamSourceResults.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "items should only have been added");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes.Select(change => change.AsAddition()).Should().BeEquivalentTo(additions, "items should have been added to the dictionary");
            _changeStreamSourceResults.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Update, "adding items to a non-empty dictionary should produce an update");
            _changeStreamSourceResults.RecordedItems.Values.Should().BeEquivalentTo(_uut, "collecting published changes should reproduce the source collection");
        }

        public void AssertItemsWereMerged(
            IReadOnlyList<TestItem>                             addedItems,
            IReadOnlyList<KeyedReplacement<string, TestItem>>   replacements)
        {
            AssertNotificationsSuspendedAndResumed();

            var additions = addedItems
                .Select(addedItem => new KeyedItem<string, TestItem>()
                {
                    Key     = _keySelector.Invoke(addedItem),
                    Item    = addedItem
                })
                .ToArray();

            _changeStreamSourceResults.HasFinalized.Should().BeFalse("the dictionary can still be changed");
            _changeStreamSourceResults.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes.Length.Should().Be(additions.Length + replacements.Count, "all given items not present in the collection should have been merged into it");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes.Where(change => change.Type is KeyedChangeType.Addition).Select(change => change.AsAddition()).Should().BeEquivalentTo(additions, options => options.WithoutStrictOrdering(), "all given items whose keys were not present within the collection should have been added");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes.Where(change => change.Type is KeyedChangeType.Replacement).Select(change => change.AsReplacement()).Should().BeEquivalentTo(replacements, options => options.WithoutStrictOrdering(), "all given items not present in the collection, but whose keys were, should have been replaced");
            _changeStreamSourceResults.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Update, "a combination of addition and replacement operations, should produce an update");
            _changeStreamSourceResults.RecordedItems.Values.Should().BeEquivalentTo(_uut, "collecting published changes should reproduce the source collection");
        }

        public void AssertItemsWereRemoved(IReadOnlyList<TestItem> removedItems)
        {
            AssertNotificationsSuspendedAndResumed();

            var removals = removedItems
                .Select(removedItem => new KeyedItem<string, TestItem>()
                {
                    Key     = _keySelector.Invoke(removedItem),
                    Item    = removedItem
                })
                .ToArray();
    
            _changeStreamSourceResults.HasFinalized.Should().BeFalse("the dictionary can still be changed");
            _changeStreamSourceResults.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "items should only have been removed");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes.Select(change => change.AsRemoval()).Should().BeEquivalentTo(removals, options => options.WithoutStrictOrdering(), "the given items should have been removed from the collection");
            _changeStreamSourceResults.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Update, "removing items from a collection, without emptying it, should produce an update");
            _changeStreamSourceResults.RecordedItems.Values.Should().BeEquivalentTo(_uut, "collecting published changes should reproduce the source collection");
        }

        public void AssertKeyWasRefreshed(string key)
        {
            AssertNotificationsSuspendedAndResumed();

            _changeStreamSourceResults.HasFinalized.Should().BeFalse("the dictionary can still be changed");
            _changeStreamSourceResults.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes.Should().ContainSingle("a single change was made");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes[0].Type.Should().Be(KeyedChangeType.Refreshment, "a single refreshment was performed");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes[0].AsRefreshment().Key.Should().Be(key, "the given key should have been retrieved");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes[0].AsRefreshment().Item.Key.Should().Be(key, "the given key's item should have been retrieved");
            _changeStreamSourceResults.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Update, "refreshing an item should produce an update");
            _changeStreamSourceResults.RecordedItems.Values.Should().BeEquivalentTo(_uut, "collecting published changes should reproduce the source collection");
        }

        public void AssertUutDidNothing()
        {
            _collectionChangedResults.RecordedNotifications.Should().BeEmpty("notifications should have been suspended");
            _changeStreamSourceResults.RecordedNotifications.Should().BeEmpty("notifications should have been suspended");

            _suspension.Dispose();

            _collectionChangedResults.RecordedNotifications.Should().BeEmpty("the dictionary should not have been changed");
            _changeStreamSourceResults.RecordedNotifications.Should().BeEmpty("the dictionary should not have been changed");
        }

        public void AssertUutWasCleared(IReadOnlyList<TestItem> removedItems)
        {
            AssertNotificationsSuspendedAndResumed();

            var removals = removedItems
                .Select(removedItem => new KeyedItem<string, TestItem>()
                {
                    Key     = _keySelector.Invoke(removedItem),
                    Item    = removedItem
                })
                .ToArray();

            _changeStreamSourceResults.HasFinalized.Should().BeFalse("the dictionary can still be changed");
            _changeStreamSourceResults.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "items should only have been removed");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes.Select(change => change.AsRemoval()).Should().BeEquivalentTo(removals, "all items should have been removed");
            _changeStreamSourceResults.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Clear, "removing all items from a dictionary should produce a clear");
            _changeStreamSourceResults.RecordedItems.Values.Should().BeEquivalentTo(_uut, "collecting published changes should reproduce the source collection");
        }

        public void AssertUutWasReset(
            IReadOnlyList<TestItem> removedItems,
            IReadOnlyList<TestItem> addedItems)
        {
            AssertNotificationsSuspendedAndResumed();

            var additions = addedItems
                .Select(addedItem => new KeyedItem<string, TestItem>()
                {
                    Key     = _keySelector.Invoke(addedItem),
                    Item    = addedItem
                })
                .ToArray();

            var removals = removedItems
                .Select(removedItem => new KeyedItem<string, TestItem>()
                {
                    Key     = _keySelector.Invoke(removedItem),
                    Item    = removedItem
                })
                .ToArray();

            _changeStreamSourceResults.HasFinalized.Should().BeFalse("the dictionary can still be changed");
            _changeStreamSourceResults.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes.Take(removedItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "all existing items should have been removed");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes.Take(removedItems.Count).Select(change => change.AsRemoval()).Should().BeEquivalentTo(removals, "all existing items should have been removed");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes.Skip(removedItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "all given items should have been added");
            _changeStreamSourceResults.RecordedChangeSets[0].Changes.Skip(removedItems.Count).Select(change => change.AsAddition()).Should().BeEquivalentTo(additions, "all given items should have been added");
            _changeStreamSourceResults.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Reset, "removing all items in a set, then adding new items, should produce a reset");
            _changeStreamSourceResults.RecordedItems.Values.Should().BeEquivalentTo(_uut, "collecting published changes should reproduce the source collection");
        }

        public void Dispose()
        {
            _suspension.Dispose();
            _collectionChangedSubscription.Dispose();
            _changeStreamSourceSubscription.Dispose();
        }
        
        private void AssertNotificationsSuspendedAndResumed()
        {
            _collectionChangedResults.RecordedNotifications.Should().BeEmpty("notifications should have been suspended");
            _changeStreamSourceResults.RecordedNotifications.Should().BeEmpty("notifications should have been suspended");

            _suspension.Dispose();

            _collectionChangedResults.HasFinalized.Should().BeFalse("the set can still be changed");
            _collectionChangedResults.RecordedValues.Should().ContainSingle("a single change operation was performed");
        }
        
        private readonly KeyedItemRecordingObserver<string, TestItem>   _changeStreamSourceResults;
        private readonly IDisposable                                    _changeStreamSourceSubscription;
        private readonly ValueRecordingObserver<RxVoid>                 _collectionChangedResults;
        private readonly IDisposable                                    _collectionChangedSubscription;
        private readonly Func<TestItem, string>                         _keySelector;
        private readonly ObservableCache<string, TestItem>              _uut;
    
        private ObservableCache<string, TestItem>.Suspension _suspension;
    }
}
