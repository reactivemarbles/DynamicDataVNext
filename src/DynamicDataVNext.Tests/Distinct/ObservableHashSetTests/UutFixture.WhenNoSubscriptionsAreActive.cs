using DynamicDataVNext.Tests.Distinct.SetTestBases;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public static partial class UutFixture
{
    public sealed class WhenNoSubscriptionsAreActive
        : ISetUutFixture<WhenNoSubscriptionsAreActive, ObservableHashSet<int>>,
            IReadOnlySetUutFixture<WhenNoSubscriptionsAreActive, ObservableHashSet<int>>
    {
        public static WhenNoSubscriptionsAreActive Create(
                IEqualityComparer<int>? comparer    = null,
                DistinctItemOptions     options     = default)
            => new(new ObservableHashSet<int>(
                comparer:   comparer,
                options:    options));

        public static WhenNoSubscriptionsAreActive Create(
                int                     capacity,
                IEqualityComparer<int>? comparer    = null,
                DistinctItemOptions     options     = default)
            => new(new ObservableHashSet<int>(
                capacity:   capacity,
                comparer:   comparer,
                options:    options));

        public static WhenNoSubscriptionsAreActive Create(
                IEnumerable<int>        items,
                IEqualityComparer<int>? comparer    = null,
                DistinctItemOptions     options     = default)
            => new(new ObservableHashSet<int>(
                items:      items,
                comparer:   comparer,
                options:    options));

        private WhenNoSubscriptionsAreActive(ObservableHashSet<int> uut)
            => _uut = uut;
        
        public ObservableHashSet<int> Uut
            => _uut;

        public int UutCapacity
            => _uut.Capacity;

        public IEqualityComparer<int> UutComparer
            => _uut.ChangeStream.Comparer;
        
        public DistinctItemOptions UutOptions
            => _uut.ChangeStream.Options;
        
        public void Dispose() { }

        public void AssertItemWasAdded(int addedItem)
        { }

        public void AssertItemWasRemoved(int removedItem)
        { }

        public void AssertItemsWereAdded(IReadOnlyList<int> addedItems)
        { }

        public void AssertItemsWereRemoved(
            IReadOnlyList<int>  removedItems,
            string              because)
        { }

        public void AssertUutDidNothing()
        { }

        public void AssertUutWasCleared(IReadOnlyList<int> items)
        { }

        public void AssertUutWasReset(
            IReadOnlyList<int> oldItems,
            IReadOnlyList<int> newItems)
        { }

        public void AssertUutWasUpdated(
            IReadOnlyList<int>  removedItems,
            IReadOnlyList<int>  addedItems,
            string              itemsRemovedBecause)
        { }
        
        public void ResetUut(IEnumerable<int> items)
            => _uut.Reset(items);

        private readonly ObservableHashSet<int> _uut;
    }
}
