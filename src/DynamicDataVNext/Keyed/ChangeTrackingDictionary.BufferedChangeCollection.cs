namespace DynamicDataVNext;

public partial class ChangeTrackingDictionary<TKey, TValue>
{
    /// <summary>
    /// Buffers a sequence of changes that have been made to a <see cref="ChangeTrackingDictionary{TKey, TValue}"/>, and allows them to be captured into a <see cref="KeyedChangeSet{TKey, TItem}"/>.
    /// </summary>
    public class BufferedChangeCollection
        : IReadOnlyList<KeyedChange<TKey, TValue>>
    {
        internal BufferedChangeCollection(bool isSourceEmpty)
            => _bufferedChangeSet = new(isSourceEmpty);
        
        public KeyedChange<TKey, TValue> this[int index]
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
        public IEnumerator<KeyedChange<TKey, TValue>> GetEnumerator()
            => _bufferedChangeSet.Changes.GetEnumerator();

        /// <summary>
        /// Captures the current sequence of changes within the collection into a <see cref="KeyedChangeSet{TKey, TItem}"/>, and removes them from the collection.
        /// </summary>
        /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> containing the changes that were removed from the collection.</returns>
        public KeyedChangeSet<TKey, TValue> CaptureAndClear()
            => _bufferedChangeSet.BuildAndClear();
        
        internal void Add(
                KeyedChange<TKey, TValue>   change,
                bool                        isSourceEmpty = false)
            => _bufferedChangeSet.AddChange(
                change:         change,
                isSourceEmpty:  isSourceEmpty);
                
        internal KeyedChangeSet<TKey, TValue>.Builder.Checkpoint CreateCheckpoint()
            => _bufferedChangeSet.CreateCheckpoint();
        
        internal void EnsureCapacity(int capacity)
            => _bufferedChangeSet.Changes.EnsureCapacity(capacity);
        
        IEnumerator IEnumerable.GetEnumerator()
            => _bufferedChangeSet.Changes.GetEnumerator();
        
        private readonly KeyedChangeSet<TKey, TValue>.Builder _bufferedChangeSet;
    }
}
