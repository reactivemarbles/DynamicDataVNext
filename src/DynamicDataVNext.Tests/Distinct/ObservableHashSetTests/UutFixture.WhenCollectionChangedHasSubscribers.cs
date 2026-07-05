using System;
using System.Collections.Generic;
using System.Reactive;

using AwesomeAssertions;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public static partial class UutFixture
{
    public sealed class WhenSetChangedHasSubscribers
        : ISetUutFixture<WhenSetChangedHasSubscribers, ObservableHashSet<int>>,
            IReadOnlySetUutFixture<WhenSetChangedHasSubscribers, ObservableHashSet<int>>
    {
        public static WhenSetChangedHasSubscribers Create(
                IEqualityComparer<int>? comparer    = null,
                DistinctItemOptions     options     = default)
            => new(new ObservableHashSet<int>(
                comparer:   comparer,
                options:    options));

        public static WhenSetChangedHasSubscribers Create(
                int                     capacity,
                IEqualityComparer<int>? comparer    = null,
                DistinctItemOptions     options     = default)
            => new(new ObservableHashSet<int>(
                capacity:   capacity,
                comparer:   comparer,
                options:    options));

        public static WhenSetChangedHasSubscribers Create(
                IEnumerable<int>        items,
                IEqualityComparer<int>? comparer    = null,
                DistinctItemOptions     options     = default)
            => new(new ObservableHashSet<int>(
                items:      items,
                comparer:   comparer,
                options:    options));

        private WhenSetChangedHasSubscribers(ObservableHashSet<int> uut)
        {
            _uut = uut;
            
            _subscription = uut.CollectionChanged.RecordValues(out _results);
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
            => AssertChangePerformed();

        public void AssertItemWasRemoved(int removedItem)
            => AssertChangePerformed();

        public void AssertItemsWereAdded(IReadOnlyList<int> addedItems)
            => AssertChangePerformed();

        public void AssertItemsWereRemoved(
                IReadOnlyList<int>  removedItems,
                string              because)
            => AssertChangePerformed();

        public void AssertUutDidNothing()
            => _results.RecordedNotifications.Should().BeEmpty("the set should not have been changed");

        public void AssertUutWasCleared(IReadOnlyList<int> items)
            => AssertChangePerformed();

        public void AssertUutWasReset(
                IReadOnlyList<int> oldItems,
                IReadOnlyList<int> newItems)
            => AssertChangePerformed();

        public void AssertUutWasUpdated(
                IReadOnlyList<int>  removedItems,
                IReadOnlyList<int>  addedItems,
                string              itemsRemovedBecause)
            => AssertChangePerformed();

        public void ResetUut(IEnumerable<int> items)
            => _uut.Reset(items);

        private void AssertChangePerformed()
        {
            _results.HasFinalized.Should().BeFalse("the set can still be changed");
            _results.RecordedValues.Should().ContainSingle("a single change operation was performed");
        }
        
        private readonly ValueRecordingObserver<Unit>   _results;
        private readonly IDisposable                    _subscription;
        private readonly ObservableHashSet<int>         _uut;
    }
}
