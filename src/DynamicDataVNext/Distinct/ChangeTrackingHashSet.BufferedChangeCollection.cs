using System.Collections;
using System.Collections.Generic;

namespace DynamicDataVNext;

public partial class ChangeTrackingHashSet<T>
{
    /// <summary>
    /// Buffers a sequence of changes that have been made to a <see cref="ChangeTrackingHashSet{T}"/>, and allows them to be captured into a <see cref="DistinctChangeSet{T}"/>.
    /// </summary>
    public class BufferedChangeCollection
        : IReadOnlyList<DistinctChange<T>>
    {
        internal BufferedChangeCollection(bool isSourceEmpty)
            => _bufferedChangeSet = new(isSourceEmpty);
        
        public DistinctChange<T> this[int index]
            => _bufferedChangeSet.Changes[index];

        /// <inheritdoc/>
        public int Count
            => _bufferedChangeSet.Changes.Count;
        
        /// <summary>
        /// The type of multi-item change operation that is currently represented by the sequence of changes within the collection. 
        /// </summary>
        public ChangeSetType CurrentSetType
            => _bufferedChangeSet.CurrentType;

        /// <inheritdoc/>
        public IEnumerator<DistinctChange<T>> GetEnumerator()
            => _bufferedChangeSet.Changes.GetEnumerator();

        /// <summary>
        /// Captures the current sequence of changes within the collection into a <see cref="DistinctChangeSet{T}"/>, and removes them from the collection.
        /// </summary>
        /// <returns>A <see cref="DistinctChangeSet{T}"/> containing the changes that were removed from the collection.</returns>
        public DistinctChangeSet<T> CaptureAndClear()
            => _bufferedChangeSet.BuildAndClear();
        
        internal void Add(
                DistinctChange<T>  change,
                bool            isSourceEmpty = false)
            => _bufferedChangeSet.AddChange(
                change:         change,
                isSourceEmpty:  isSourceEmpty);
        
        internal void EnsureCapacity(int capacity)
            => _bufferedChangeSet.Changes.EnsureCapacity(capacity);
        
        IEnumerator IEnumerable.GetEnumerator()
            => _bufferedChangeSet.Changes.GetEnumerator();
        
        private readonly DistinctChangeSet<T>.Builder _bufferedChangeSet;
    }
}
