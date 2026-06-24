using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DynamicDataVNext;

public partial class ChangeSetBuilderBase<TChange, TChangeType, TChangeSet>
{
    /// <summary>
    /// The collection of buffered changes within a <see cref="ChangeSetBuilderBase{TChange,TChangeType,TChangeSet}"/> object.
    /// </summary>
    public class ChangeCollection
        : IReadOnlyList<TChange>
    {
        internal ChangeCollection()
            => _changes = ImmutableArray.CreateBuilder<TChange>();
        
        internal ChangeCollection(int initialCapacity)
            => _changes = ImmutableArray.CreateBuilder<TChange>(initialCapacity);
        
        /// <inheritdoc/>
        public TChange this[int index]
            => _changes[index];

        /// <inheritdoc cref="List{T}.Capacity"/>
        public int Capacity
        {
            get => _changes.Capacity;
            set => _changes.Capacity = value;
        }

        /// <inheritdoc/>
        public int Count
            => _changes.Count;
        
        /// <inheritdoc cref="List{T}.EnsureCapacity(int)"/>
        public void EnsureCapacity(int capacity)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(capacity, 0);
            
            if (_changes.Capacity < capacity)
                _changes.Capacity = capacity;
        }
    
        /// <inheritdoc/>
        public IEnumerator<TChange> GetEnumerator()
            => _changes.GetEnumerator();

        internal void Add(TChange change)
            => _changes.Add(change);
        
        internal ImmutableArray<TChange> BuildImmutable(bool willBuilderBeReused)
            => willBuilderBeReused
                ? _changes.ToImmutable()
                : _changes.MoveToOrCreateImmutable();
        
        internal void Clear()
            => _changes.Clear();
        
        internal void RemoveRange(
            int index,
            int length)
            => _changes.RemoveRange(
                index:  index,
                length: length);
        
        IEnumerator IEnumerable.GetEnumerator()
            => _changes.GetEnumerator();

        private readonly ImmutableArray<TChange>.Builder _changes;
    }
}
