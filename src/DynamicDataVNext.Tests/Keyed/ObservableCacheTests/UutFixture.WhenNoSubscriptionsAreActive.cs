using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public static partial class UutFixture
{
    public sealed class WhenNoSubscriptionsAreActive
        : ICacheUutFixture<WhenNoSubscriptionsAreActive, ObservableCache<string, TestItem>>,
            IReadOnlyCacheUutFixture<WhenNoSubscriptionsAreActive, ObservableCache<string, TestItem>>
    {
        public static WhenNoSubscriptionsAreActive Create(
                Func<TestItem, string>      keySelector,
                IEqualityComparer<string>?  comparer    = null,
                KeyedItemOptions            options     = default)
            => new(new(
                keySelector:    keySelector,
                comparer:       comparer,
                options:        options));

        public static WhenNoSubscriptionsAreActive Create(
                int                         capacity,
                Func<TestItem, string>      keySelector,
                IEqualityComparer<string>?  comparer    = null,
                KeyedItemOptions            options     = default)
            => new(new(
                capacity:       capacity,
                keySelector:    keySelector,
                comparer:       comparer,
                options:        options));

        public static WhenNoSubscriptionsAreActive Create(
                IEnumerable<TestItem>       items,
                Func<TestItem, string>      keySelector,
                IEqualityComparer<string>?  comparer    = null,
                KeyedItemOptions            options     = default)
            => new(new(
                items:          items,
                keySelector:    keySelector,
                comparer:       comparer,
                options:        options));

        private WhenNoSubscriptionsAreActive(ObservableCache<string, TestItem> uut)
            => _uut = uut;
        
        public ObservableCache<string, TestItem> Uut
            => _uut;

        public int UutCapacity
            => _uut.Capacity;

        public IEqualityComparer<string> UutComparer
            => _uut.ChangeStream.KeyComparer;
        
        public KeyedItemOptions UutOptions
            => _uut.ChangeStream.Options;
        
        public void AssertItemWasAdded(TestItem addedItem)
        { }

        public void AssertItemWasRefreshed(TestItem refreshedItem)
        { }

        public void AssertItemWasRemoved(TestItem removedItem)
        { }

        public void AssertItemWasReplaced(
            TestItem oldItem,
            TestItem newItem)
        { }

        public void AssertItemsWereAdded(IReadOnlyList<TestItem> addedItems)
        { }

        public void AssertItemsWereMerged(
            IReadOnlyList<TestItem>                             addedItems,
            IReadOnlyList<KeyedReplacement<string, TestItem>>   replacements)
        { }

        public void AssertItemsWereRemoved(IReadOnlyList<TestItem> removedItems)
        { }

        public void AssertKeyWasRefreshed(string key)
        { }

        public void AssertUutDidNothing()
        { }

        public void AssertUutWasCleared(IReadOnlyList<TestItem> removedItems)
        { }

        public void AssertUutWasReset(
            IReadOnlyList<TestItem> removedItems,
            IReadOnlyList<TestItem> addedItems)
        { }

        public void Dispose() { }

        private readonly ObservableCache<string, TestItem> _uut;
    }
}
