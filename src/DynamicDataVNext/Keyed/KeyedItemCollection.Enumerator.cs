namespace DynamicDataVNext;

public partial struct KeyedItemCollection<TKey, TItem>
{
    public struct Enumerator
        : IEnumerator<KeyedItem<TKey, TItem>>
    {
        internal Enumerator(Dictionary<TKey, TItem>.Enumerator enumerator)
            => _enumerator = enumerator;

        /// <inheritdoc/>
        public KeyedItem<TKey, TItem> Current
            => _enumerator.Current;
    
        /// <inheritdoc/>
        public bool MoveNext()
            => _enumerator.MoveNext();
    
        /// <inheritdoc/>
        public void Reset()
            => ((IEnumerator)_enumerator).Reset();
    
        object? IEnumerator.Current
            => ((IEnumerator)_enumerator).Current;
    
        void IDisposable.Dispose() { }

        private Dictionary<TKey, TItem>.Enumerator _enumerator;
    }
}
