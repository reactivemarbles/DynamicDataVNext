using System;
using System.Collections;
using System.Collections.Generic;

namespace DynamicDataVNext;

public readonly partial struct DistinctChangeItemCollection<T>
{
    /// <inheritdoc/>
    public struct Enumerator
        : IEnumerator<T>
    {
        internal Enumerator(DistinctChangeItemCollection<T> owner)
        {
            _owner = owner;
            
            _changeIndex = -1;
        }   
        
        /// <inheritdoc/>
        public T Current
            => _owner.Changes[_changeIndex].Item;
        
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

        private readonly DistinctChangeItemCollection<T> _owner;

        private int _changeIndex;
    }
}
