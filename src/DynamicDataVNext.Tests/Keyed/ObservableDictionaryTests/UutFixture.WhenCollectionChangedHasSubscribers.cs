using DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public static partial class UutFixture
{
    public sealed class WhenCollectionChangedHasSubscribers
        : IDictionaryUutFixture<WhenCollectionChangedHasSubscribers, ObservableDictionary<string, int>>,
            IReadOnlyDictionaryUutFixture<WhenCollectionChangedHasSubscribers, ObservableDictionary<string, int>>
    {
        public static WhenCollectionChangedHasSubscribers Create(
                IEqualityComparer<string>?  comparer    = null,
                KeyedItemOptions            options     = default)
            => new(new ObservableDictionary<string, int>(
                comparer:   comparer,
                options:    options));

        public static WhenCollectionChangedHasSubscribers Create(
                int                         capacity,
                IEqualityComparer<string>?  comparer    = null,
                KeyedItemOptions            options     = default)
            => new(new ObservableDictionary<string, int>(
                capacity:   capacity,
                comparer:   comparer,
                options:    options));

        public static WhenCollectionChangedHasSubscribers Create(
                IEnumerable<KeyValuePair<string, int>>  items,
                IEqualityComparer<string>?              comparer    = null,
                KeyedItemOptions                        options     = default)
            => new(new ObservableDictionary<string, int>(
                items:      items,
                comparer:   comparer,
                options:    options));

        private WhenCollectionChangedHasSubscribers(ObservableDictionary<string, int> uut)
        {
            _uut = uut;
            
            _subscription = uut.CollectionChanged.RecordValues(out _results);
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
            => AssertChangeWasPerformed();

        public void AssertItemWasRefreshed(
                string  key,
                int     value)
            => AssertChangeWasPerformed();

        public void AssertItemWasRemoved(
                string  removedKey,
                int     removedValue)
            => AssertChangeWasPerformed();

        public void AssertItemWasReplaced(
                string  replacementKey,
                int     replacedValue,
                int     replacementValue)
            => AssertChangeWasPerformed();

        public void AssertItemsWereAdded(IReadOnlyList<KeyValuePair<string, int>> addedItems)
            => AssertChangeWasPerformed();

        public void AssertUutDidNothing()
            => _results.RecordedNotifications.Should().BeEmpty("the dictionary should not have been changed");

        public void AssertUutWasCleared(IReadOnlyList<KeyValuePair<string, int>> items)
            => AssertChangeWasPerformed();

        public void AssertUutWasReset(
                IReadOnlyList<KeyValuePair<string, int>> oldItems,
                IReadOnlyList<KeyValuePair<string, int>> newItems)
            => AssertChangeWasPerformed();
        
        public void Dispose()
            => _subscription.Dispose();

        private void AssertChangeWasPerformed()
        {
            _results.HasFinalized.Should().BeFalse("the set can still be changed");
            _results.RecordedValues.Should().ContainSingle("a single change operation was performed");
        }
        
        private readonly ValueRecordingObserver<RxVoid>     _results;
        private readonly IDisposable                        _subscription;
        private readonly ObservableDictionary<string, int>  _uut;
    }
}
