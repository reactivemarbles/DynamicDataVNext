using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public static partial class UutFixture
{
    public sealed class WhenCollectionChangedHasSubscribers
        : ICacheUutFixture<WhenCollectionChangedHasSubscribers, ObservableCache<string, TestItem>>,
            IReadOnlyCacheUutFixture<WhenCollectionChangedHasSubscribers, ObservableCache<string, TestItem>>
    {
        public static WhenCollectionChangedHasSubscribers Create(
                Func<TestItem, string>      keySelector,
                IEqualityComparer<string>?  comparer    = null,
                KeyedItemOptions            options     = default)
            => new(new(
                keySelector:    keySelector,
                comparer:       comparer,
                options:        options));

        public static WhenCollectionChangedHasSubscribers Create(
                int                         capacity,
                Func<TestItem, string>      keySelector,
                IEqualityComparer<string>?  comparer    = null,
                KeyedItemOptions            options     = default)
            => new(new(
                capacity:       capacity,
                keySelector:    keySelector,
                comparer:       comparer,
                options:        options));

        public static WhenCollectionChangedHasSubscribers Create(
                IEnumerable<TestItem>       items,
                Func<TestItem, string>      keySelector,
                IEqualityComparer<string>?  comparer    = null,
                KeyedItemOptions            options     = default)
            => new(new(
                items:          items,
                keySelector:    keySelector,
                comparer:       comparer,
                options:        options));

        private WhenCollectionChangedHasSubscribers(ObservableCache<string, TestItem> uut)
        {
            _uut = uut;
            
            _subscription = uut.CollectionChanged.RecordValues(out _results);
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
            => AssertChangeWasPerformed();

        public void AssertItemWasRefreshed(TestItem refreshedItem)
            => AssertChangeWasPerformed();

        public void AssertItemWasRemoved(TestItem removedItem)
            => AssertChangeWasPerformed();

        public void AssertItemWasReplaced(
                TestItem oldItem,
                TestItem newItem)
            => AssertChangeWasPerformed();

        public void AssertItemsWereAdded(IReadOnlyList<TestItem> addedItems)
            => AssertChangeWasPerformed();

        public void AssertItemsWereMerged(
                IReadOnlyList<TestItem>                             addedItems,
                IReadOnlyList<KeyedReplacement<string, TestItem>>   replacements)
            => AssertChangeWasPerformed();

        public void AssertItemsWereRemoved(IReadOnlyList<TestItem> removedItems)
            => AssertChangeWasPerformed();

        public void AssertKeyWasRefreshed(string key)
            => AssertChangeWasPerformed();

        public void AssertUutDidNothing()
            => _results.RecordedNotifications.Should().BeEmpty("the dictionary should not have been changed");

        public void AssertUutWasCleared(IReadOnlyList<TestItem> removedItems)
            => AssertChangeWasPerformed();

        public void AssertUutWasReset(
                IReadOnlyList<TestItem> removedItems,
                IReadOnlyList<TestItem> addedItems)
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
        private readonly ObservableCache<string, TestItem>  _uut;
    }
}
