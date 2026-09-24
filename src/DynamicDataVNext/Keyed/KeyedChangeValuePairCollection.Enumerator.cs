namespace DynamicDataVNext;

public readonly partial struct KeyedChangeValuePairCollection<TKey, TItem>
{
    /// <inheritdoc/>
    public struct Enumerator
        : IEnumerator<KeyValuePair<TKey, TItem>>
    {
        internal Enumerator(KeyedChangeValuePairCollection<TKey, TItem> owner)
        {
            _owner = owner;
            
            _changeIndex = -1;
        }   
        
        /// <inheritdoc/>
        public KeyValuePair<TKey, TItem> Current
        {
            get
            {
                var change = _owner.Changes[_changeIndex];
                
                return new(
                    key:    change.Key,
                    value:  change.PrimaryItem);
            }
        }
        
        /// <inheritdoc/>
        public bool MoveNext()
        {
            if (_changeIndex >= _owner.LastIndex)
                return false;
            
            _changeIndex = (_changeIndex is -1)
                ? _owner.FirstIndex
                : _changeIndex + 1;
            
            return true;
        }
        
        /// <inheritdoc/>
        public void Reset()
            => _changeIndex = 0;
        
        object? IEnumerator.Current
            => Current;
        
        void IDisposable.Dispose() { }

        private readonly KeyedChangeValuePairCollection<TKey, TItem> _owner;

        private int _changeIndex;
    }
}
