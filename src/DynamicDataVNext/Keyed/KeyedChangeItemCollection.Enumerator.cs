using System;
using System.Collections;
using System.Collections.Generic;

namespace DynamicDataVNext;

public readonly partial struct KeyedChangeItemCollection<TKey, TItem>
{
    /// <inheritdoc/>
    public struct Enumerator
        : IEnumerator<KeyedItem<TKey, TItem>>
    {
        internal Enumerator(KeyedChangeItemCollection<TKey, TItem> owner)
        {
            _owner = owner;
            
            _changeIndex = -1;
        }   
        
        /// <inheritdoc/>
        public KeyedItem<TKey, TItem> Current
        {
            get
            {
                var change = _owner.Changes[_changeIndex];
                
                return new()
                {
                    Item    = change.PrimaryItem,
                    Key     = change.Key
                };
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

        private readonly KeyedChangeItemCollection<TKey, TItem> _owner;

        private int _changeIndex;
    }
}
