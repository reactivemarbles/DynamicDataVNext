using DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public static partial class UutFixture
{
    public sealed class WhenChangeStreamSourceHasSubscribers
        : IDictionaryUutFixture<WhenChangeStreamSourceHasSubscribers, ObservableDictionary<string, int>>,
            IReadOnlyDictionaryUutFixture<WhenChangeStreamSourceHasSubscribers, ObservableDictionary<string, int>>
    {
        public static WhenChangeStreamSourceHasSubscribers Create(
                IEqualityComparer<string>?  comparer    = null,
                KeyedItemOptions            options     = default)
            => new(new ObservableDictionary<string, int>(
                comparer:   comparer,
                options:    options));

        public static WhenChangeStreamSourceHasSubscribers Create(
                int                         capacity,
                IEqualityComparer<string>?  comparer    = null,
                KeyedItemOptions            options     = default)
            => new(new ObservableDictionary<string, int>(
                capacity:   capacity,
                comparer:   comparer,
                options:    options));

        public static WhenChangeStreamSourceHasSubscribers Create(
                IEnumerable<KeyValuePair<string, int>>  items,
                IEqualityComparer<string>?              comparer    = null,
                KeyedItemOptions                        options     = default)
            => new(new ObservableDictionary<string, int>(
                items:      items,
                comparer:   comparer,
                options:    options));

        private WhenChangeStreamSourceHasSubscribers(ObservableDictionary<string, int> uut)
        {
            _uut = uut;

            _subscription = uut.ChangeStream
                .ValidateChangeSets()
                .RecordItems(out _results);
            _results.ClearNotifications();       
        }
        
        public ObservableDictionary<string, int> Uut
            => _uut;

        public int UutCapacity
            => _uut.Capacity;

        public IEqualityComparer<string> UutComparer
            => _uut.ChangeStream.KeyComparer;
        
        public KeyedItemOptions UutOptions
            => _uut.ChangeStream.Options;
        
        public void AssertItemWasAdded(
            string  addedKey,
            int     addedValue)
        {
            _results.HasFinalized.Should().BeFalse("the dictionary can still be changed");
            _results.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _results.RecordedChangeSets[0].Changes.Should().ContainSingle("a single change was made");
            _results.RecordedChangeSets[0].Changes[0].Type.Should().Be(KeyedChangeType.Addition, "a single addition was performed");
            _results.RecordedChangeSets[0].Changes[0].AsAddition().Key.Should().Be(addedKey, "the given item should have been added");
            _results.RecordedChangeSets[0].Changes[0].AsAddition().Item.Should().Be(addedValue, "the given item should have been added");
            _results.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Update, "adding an item to a non-empty set should produce an update");
            _results.RecordedItems.Should().BeEquivalentTo(_uut, "collecting published changes should reproduce the source collection");
        }

        public void AssertItemWasRefreshed(
            string  key,
            int     value)
        {
            _results.HasFinalized.Should().BeFalse("the dictionary can still be changed");
            _results.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _results.RecordedChangeSets[0].Changes.Should().ContainSingle("a single change was made");
            _results.RecordedChangeSets[0].Changes[0].Type.Should().Be(KeyedChangeType.Refreshment, "a single refreshment was performed");
            _results.RecordedChangeSets[0].Changes[0].AsRefreshment().Key.Should().Be(key, "the given item should have been refreshed");
            _results.RecordedChangeSets[0].Changes[0].AsRefreshment().Item.Should().Be(value, "the given item should have been refreshed");
            _results.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Update, "refreshing an item should produce an update");
            _results.RecordedItems.Should().BeEquivalentTo(_uut, "collecting published changes should reproduce the source collection");
        }

        public void AssertItemWasRemoved(
            string  removedKey,
            int     removedValue)
        {
            _results.HasFinalized.Should().BeFalse("the dictionary can still be changed");
            _results.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _results.RecordedChangeSets[0].Changes.Should().ContainSingle("a single change was made");
            _results.RecordedChangeSets[0].Changes[0].Type.Should().Be(KeyedChangeType.Removal, "a single removal was performed");
            _results.RecordedChangeSets[0].Changes[0].AsRemoval().Key.Should().Be(removedKey, "the given item should have been removed");
            _results.RecordedChangeSets[0].Changes[0].AsRemoval().Item.Should().Be(removedValue, "the given item should have been removed");
            _results.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Update, "removing an item from a collection of multiple items should produce an update");
            _results.RecordedItems.Should().BeEquivalentTo(_uut, "collecting published changes should reproduce the source collection");
        }

        public void AssertItemWasReplaced(
            string  replacementKey,
            int     replacedValue,
            int     replacementValue)
        {
            _results.HasFinalized.Should().BeFalse("the dictionary can still be changed");
            _results.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _results.RecordedChangeSets[0].Changes.Should().ContainSingle("a single change was made");
            _results.RecordedChangeSets[0].Changes[0].Type.Should().Be(KeyedChangeType.Replacement, "a single replacement was performed");
            _results.RecordedChangeSets[0].Changes[0].AsReplacement().Key.Should().Be(replacementKey, "the replacement should have occurred for the given key");
            _results.RecordedChangeSets[0].Changes[0].AsReplacement().OldItem.Should().Be(replacedValue, "the previous item at the given key should have been recorded");
            _results.RecordedChangeSets[0].Changes[0].AsReplacement().NewItem.Should().Be(replacementValue, "the given item should have replaced the previous one");
            _results.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Update, "replacing an item within a collection should produce an update");
            _results.RecordedItems.Should().BeEquivalentTo(_uut, "collecting published changes should reproduce the source collection");
        }

        public void AssertItemsWereAdded(IReadOnlyList<KeyValuePair<string, int>> addedItems)
        {
            _results.HasFinalized.Should().BeFalse("the dictionary can still be changed");
            _results.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _results.RecordedChangeSets[0].Changes.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "items should only have been added");
            _results.RecordedChangeSets[0].Changes.Select(change => (KeyValuePair<string, int>)change.AsAddition()).Should().BeEquivalentTo(addedItems, options => options.WithoutStrictOrdering(), "items should have been added to the dictionary");
            _results.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Update, "adding items to a non-empty dictionary should produce an update");
            _results.RecordedItems.Should().BeEquivalentTo(_uut, "collecting published changes should reproduce the source collection");
        }

        public void AssertUutDidNothing()
            => _results.RecordedNotifications.Should().BeEmpty("the dictionary should not have been changed");

        public void AssertUutWasCleared(IReadOnlyList<KeyValuePair<string, int>> items)
        {
            _results.HasFinalized.Should().BeFalse("the dictionary can still be changed");
            _results.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _results.RecordedChangeSets[0].Changes.Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "items should only have been removed");
            _results.RecordedChangeSets[0].Changes.Select(change => (KeyValuePair<string, int>)change.AsRemoval()).Should().BeEquivalentTo(items, options => options.WithoutStrictOrdering(), "all items should have been removed");
            _results.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Clear, "removing all items from a dictionary should produce a clear");
            _results.RecordedItems.Should().BeEquivalentTo(_uut, "collecting published changes should reproduce the source collection");
        }

        public void AssertUutWasReset(
            IReadOnlyList<KeyValuePair<string, int>> oldItems,
            IReadOnlyList<KeyValuePair<string, int>> newItems)
        {
            _results.HasFinalized.Should().BeFalse("the dictionary can still be changed");
            _results.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
            _results.RecordedChangeSets[0].Changes.Take(oldItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "all existing items should have been removed");
            _results.RecordedChangeSets[0].Changes.Take(oldItems.Count).Select(change => (KeyValuePair<string, int>)change.AsRemoval()).Should().BeEquivalentTo(oldItems, options => options.WithoutStrictOrdering(), "all existing items should have been removed");
            _results.RecordedChangeSets[0].Changes.Skip(oldItems.Count).Select(change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "all given items should have been added");
            _results.RecordedChangeSets[0].Changes.Skip(oldItems.Count).Select(change => (KeyValuePair<string, int>)change.AsAddition()).Should().BeEquivalentTo(newItems, options => options.WithoutStrictOrdering(), "all given items should have been added");
            _results.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Reset, "removing all items in a set, then adding new items, should produce a reset");
            _results.RecordedItems.Should().BeEquivalentTo(_uut, "collecting published changes should reproduce the source collection");
        }
        
        public void Dispose()
            => _subscription.Dispose();

        private readonly KeyedItemRecordingObserver<string, int>    _results;
        private readonly IDisposable                                _subscription;
        private readonly ObservableDictionary<string, int>          _uut;
    }
}
