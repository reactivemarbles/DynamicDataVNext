using DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public static partial class UutFixture
{
    public sealed class WhenNoSubscriptionsAreActive
        : IDictionaryUutFixture<WhenNoSubscriptionsAreActive, ObservableDictionary<string, int>>,
            IReadOnlyDictionaryUutFixture<WhenNoSubscriptionsAreActive, ObservableDictionary<string, int>>
    {
        public static WhenNoSubscriptionsAreActive Create(
                IEqualityComparer<string>?  comparer    = null,
                KeyedItemOptions            options     = default)
            => new(new ObservableDictionary<string, int>(
                comparer:   comparer,
                options:    options));

        public static WhenNoSubscriptionsAreActive Create(
                int                         capacity,
                IEqualityComparer<string>?  comparer    = null,
                KeyedItemOptions            options     = default)
            => new(new ObservableDictionary<string, int>(
                capacity:   capacity,
                comparer:   comparer,
                options:    options));

        public static WhenNoSubscriptionsAreActive Create(
                IEnumerable<KeyValuePair<string, int>>  items,
                IEqualityComparer<string>?              comparer    = null,
                KeyedItemOptions                        options     = default)
            => new(new ObservableDictionary<string, int>(
                items:      items,
                comparer:   comparer,
                options:    options));

        private WhenNoSubscriptionsAreActive(ObservableDictionary<string, int> uut)
            => _uut = uut;
        
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
        { }

        public void AssertItemWasRefreshed(
            string  key,
            int     value)
        { }

        public void AssertItemWasRemoved(
            string  removedKey,
            int     removedValue)
        { }

        public void AssertItemWasReplaced(
            string  replacementKey,
            int     replacedValue,
            int     replacementValue)
        { }

        public void AssertItemsWereAdded(IReadOnlyList<KeyValuePair<string, int>> addedItems)
        { }

        public void AssertUutDidNothing()
        { }

        public void AssertUutWasCleared(IReadOnlyList<KeyValuePair<string, int>> items)
        { }

        public void AssertUutWasReset(
            IReadOnlyList<KeyValuePair<string, int>> oldItems,
            IReadOnlyList<KeyValuePair<string, int>> newItems)
        { }

        public void Dispose() { }

        private readonly ObservableDictionary<string, int> _uut;
    }
}
