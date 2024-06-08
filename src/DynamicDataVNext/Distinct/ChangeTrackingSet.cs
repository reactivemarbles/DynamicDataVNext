using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DynamicDataVNext;

/// <summary>
/// A collection of distinct items, which tracks and allows for capturing of changes made to it, as they occur.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public sealed class ChangeTrackingSet<T>
    : IExtendedSet<T>,
        IReadOnlySet<T>
{
    private readonly DistinctChangeSet.Builder<T>   _changeCollector;
    private readonly IEqualityComparer<T>           _comparer;

    private bool        _isChangeCollectionEnabled;
    private bool        _isDirty;
    private HashSet<T>  _items;

    /// <summary>
    /// Constructs a new instance of the <see cref="ChangeTrackingSet{T}"/> class.
    /// </summary>
    /// <param name="capacity">The initial number of items that the collection should be able to contain, before needing to allocation additional memory.</param>
    /// <param name="comparer">The comparer to be used for detecting item changes, within the collection, or <see langword="null"/> if <see cref="EqualityComparer{T}.Default"/> should be used.</param>
    public ChangeTrackingSet(
        int?                    capacity = null,
        IEqualityComparer<T>?   comparer = null)
    {
        _comparer = comparer ?? EqualityComparer<T>.Default;

        _items = (capacity is int givenCapacity)
            ? new(
                capacity: givenCapacity,
                comparer: _comparer)
            : new(
                comparer: _comparer);

        _changeCollector = new();
        _isChangeCollectionEnabled = true;
    }

    /// <summary>
    /// The comparer to be used for matching items against each other.
    /// </summary>
    public IEqualityComparer<T> Comparer
        => _items.Comparer;

    /// <inheritdoc/>
    public int Count
        => _items.Count;

    /// <summary>
    /// A flag indicating whether the collection should actually collect changes, to be retrieved by calls to <see cref="CaptureChangesAndClean"/>.
    /// Defaults to <see langword="true"/>.
    /// </summary>
    /// <remarks>
    /// Note that any changes previously collected, but not captured, when this property is set to <see langword="false"/> will be discarded. Otherwise, it would be possible for <see cref="CaptureChangesAndClean"/> to generate a corrupt changes, when next called, after setting this property back to <see langword="true"/>.
    /// </remarks>
    public bool IsChangeCollectionEnabled
    { 
        get => _isChangeCollectionEnabled;
        set
        {
            if ((value is false) && (_changeCollector.Count is not 0))
                _changeCollector.Clear();

            _isChangeCollectionEnabled = value;
        }
    }

    /// <summary>
    /// A flag indicating whether changes have been made to the collection since its creation, or since the last call to `<see cref="CaptureChangesAndClean"/>.
    /// </summary>
    public bool IsDirty
        => _isDirty;

    /// <summary>
    /// Captures any previously-collected changes made to the collection, and resets the collection to a "clean" state (I.E. sets <see cref="IsDirty"/> to <see langword="false"/>).
    /// </summary>
    /// <returns>A <see cref="DistinctChangeSet{T}"/> containing all changes made to the collection since its construction, the last call to <see cref="CaptureChangesAndClean"/>, or since <see cref="IsChangeCollectionEnabled"/> was last changed to <see langword="true"/>.</returns>
    /// <remarks>
    /// Note that this method will always return an empty changeset, when <see cref="IsChangeCollectionEnabled"/> is <see langword="false"/>.
    /// </remarks>
    public DistinctChangeSet<T> CaptureChangesAndClean()
    {
        _isDirty = false;
        return _changeCollector.BuildAndClear();
    }

    /// <inheritdoc/>
    public bool Add(T item)
    {
        var result = _items.Add(item);

        if (result && _isChangeCollectionEnabled)
            _changeCollector.AddChange(DistinctChange.Addition(item));

        _isDirty |= result;

        return result;
    }

    /// <inheritdoc/>
    public void Clear()
    {
        if (_items.Count is 0)
            return;

        if (_isChangeCollectionEnabled)
        {
            foreach (var item in _items)
                _changeCollector.AddChange(DistinctChange.Removal(item));

            _changeCollector.OnSourceCleared();
        }

        _items.Clear();

        _isDirty = true;
    }

    /// <inheritdoc/>
    public bool Contains(T item)
        => _items.Contains(item);

    /// <inheritdoc/>
    public void CopyTo(
            T[] array,
            int arrayIndex)
        => _items.CopyTo(array, arrayIndex);

    /// <inheritdoc cref="HashSet{T}.EnsureCapacity(int)"/>
    public void EnsureCapacity(int capacity)
        => _items.EnsureCapacity(capacity);

    /// <inheritdoc/>
    public void ExceptWith(IEnumerable<T> other)
    {
        if (_items.Count is 0)
            return;

        if (_isChangeCollectionEnabled)
        {
            if(other.TryGetNonEnumeratedCount(out var otherCount))
            {
                if (otherCount is 0)
                    return;

                _changeCollector.EnsureCapacity(Math.Min(otherCount, _items.Count));
            }

            foreach (var item in other)
                if (_items.Remove(item))
                {
                    _changeCollector.AddChange(DistinctChange.Removal(item));
                    _isDirty = true;
                }

            if (_items.Count is 0)
                _changeCollector.OnSourceCleared();
        }
        else
        {
            var oldCount = _items.Count;

            _items.ExceptWith(other);

            _isDirty |= _items.Count != oldCount;
        }
    }

    /// <inheritdoc/>
    public void ExceptWith(ReadOnlySpan<T> other)
    {
        if ((other.Length is 0) || (_items.Count is 0))
            return;

        if (_isChangeCollectionEnabled)
        {
            _changeCollector.EnsureCapacity(Math.Min(other.Length, _items.Count));

            foreach (var item in other)
                if (_items.Remove(item))
                {
                    _changeCollector.AddChange(DistinctChange.Removal(item));
                    _isDirty = true;
                }

            if (_items.Count is 0)
                _changeCollector.OnSourceCleared();
        }
        else
        {
            foreach (var item in other)
                _isDirty |= _items.Remove(item);
        }
    }

    /// <inheritdoc/>
    public void IntersectWith(IEnumerable<T> other)
    {
        if (_items.Count is 0)
            return;

        if (_isChangeCollectionEnabled)
        {
            if(other.TryGetNonEnumeratedCount(out var otherCount))
                _changeCollector.EnsureCapacity(Math.Min(0, _items.Count - otherCount));

            var newItems = new HashSet<T>(capacity: Math.Min(otherCount, _items.Count));
            var hasChanges = false;
            foreach (var item in other)
            {
                if (_items.Remove(item))
                    newItems.Add(item);
                else
                {
                    _changeCollector.AddChange(DistinctChange.Removal(item));
                    hasChanges = true;
                }
            }

            if (hasChanges)
            {
                _items = newItems;
                _isDirty = true;

                if (_items.Count is 0)
                    _changeCollector.OnSourceCleared();
            }
        }
        else
        {
            var oldCount = _items.Count;
            
            _items.IntersectWith(other);

            _isDirty = _items.Count != oldCount;
        }
    }

    /// <inheritdoc/>
    public void IntersectWith(ReadOnlySpan<T> other)
    {
        if (_items.Count is 0)
            return;

        var newItems = new HashSet<T>(capacity: Math.Min(other.Length, _items.Count));
        var hasChanges = false;

        if (_isChangeCollectionEnabled)
        {
            _changeCollector.EnsureCapacity(Math.Min(0, _items.Count - other.Length));

            foreach (var item in other)
            {
                if (_items.Remove(item))
                    newItems.Add(item);
                else
                {
                    _changeCollector.AddChange(DistinctChange.Removal(item));
                    hasChanges = true;
                }
            }
        }
        else
        {
            foreach (var item in other)
            {
                if (_items.Remove(item))
                    newItems.Add(item);
                else
                    hasChanges = true;
            }
        }

        if (hasChanges)
        {
            _items = newItems;
            _isDirty = true;

            if (_isChangeCollectionEnabled && (_items.Count is 0))
                _changeCollector.OnSourceCleared();
        }
    }

    /// <inheritdoc/>
    public HashSet<T>.Enumerator GetEnumerator()
        => _items.GetEnumerator();

    /// <inheritdoc/>
    public bool IsProperSubsetOf(IEnumerable<T> other)
        => _items.IsProperSubsetOf(other);

    /// <inheritdoc/>
    public bool IsProperSupersetOf(IEnumerable<T> other)
        => _items.IsProperSupersetOf(other);

    /// <inheritdoc/>
    public bool IsSubsetOf(IEnumerable<T> other)
        => _items.IsSubsetOf(other);

    /// <inheritdoc/>
    public bool IsSupersetOf(IEnumerable<T> other)
        => _items.IsSupersetOf(other);

    /// <inheritdoc/>
    public bool Overlaps(IEnumerable<T> other)
        => _items.Overlaps(other);

    /// <inheritdoc/>
    public bool Remove(T item)
    {
        var result = _items.Remove(item);

        if (result && _isChangeCollectionEnabled)
            _changeCollector.AddChange(DistinctChange.Removal(item));

        _isDirty |= result;

        return result;
    }

    /// <inheritdoc/>
    public void Reset(IEnumerable<T> items)
    {
        Clear();
        UnionWith(items);
    }

    /// <inheritdoc/>
    public void Reset(ReadOnlySpan<T> items)
    {
        Clear();
        UnionWith(items);
    }

    /// <inheritdoc/>
    public bool SetEquals(IEnumerable<T> other)
        => _items.SetEquals(other);

    /// <inheritdoc/>
    public void SymmetricExceptWith(IEnumerable<T> other)
    {
        if (_isChangeCollectionEnabled)
        {
            if(other.TryGetNonEnumeratedCount(out var otherCount))
            { 
                if (otherCount is 0)
                    return;

                _items.EnsureCapacity(_items.Count + otherCount);
                _changeCollector.EnsureCapacity(_items.Count + otherCount);
            }

            foreach (var item in other)
            {
                if (_items.Add(item))
                    _changeCollector.AddChange(DistinctChange.Addition(item));
                else
                {
                    _items.Remove(item);
                    _changeCollector.AddChange(DistinctChange.Removal(item));
                }

                _isDirty = true;
            }

            if (_items.Count is 0)
                _changeCollector.OnSourceCleared();
        }
        else
        {
            foreach (var item in other)
            {
                if (!_items.Add(item))
                    _items.Remove(item);

                _isDirty = true;
            }
        }
    }

    /// <inheritdoc/>
    public void SymmetricExceptWith(ReadOnlySpan<T> other)
    {
        if (other.Length is 0)
            return;

        if (_isChangeCollectionEnabled)
        {
            _items.EnsureCapacity(_items.Count + other.Length);
            _changeCollector.EnsureCapacity(_items.Count + other.Length);

            foreach (var item in other)
            {
                if (_items.Add(item))
                    _changeCollector.AddChange(DistinctChange.Addition(item));
                else
                {
                    _items.Remove(item);
                    _changeCollector.AddChange(DistinctChange.Removal(item));
                }

                _isDirty = true;
            }

            if (_items.Count is 0)
                _changeCollector.OnSourceCleared();
        }
        else
        {
            foreach (var item in other)
            {
                if (!_items.Add(item))
                    _items.Remove(item);

                _isDirty = true;
            }
        }
    }

    /// <inheritdoc/>
    public void UnionWith(IEnumerable<T> other)
    {
        if (_isChangeCollectionEnabled)
        {
            if (other.TryGetNonEnumeratedCount(out var otherCount))
            {
                if (otherCount is 0)
                    return;

                _items.EnsureCapacity(_items.Count + otherCount);

                _changeCollector.EnsureCapacity(otherCount);
            }

            foreach(var item in other)
                if (_items.Add(item))
                {
                    _changeCollector.AddChange(DistinctChange.Addition(item));
                    _isDirty = true;
                }
        }
        else
        {
            var oldCount = _items.Count;

            _items.UnionWith(other);

            _isDirty |= oldCount != _items.Count;
        }
    }

    /// <inheritdoc/>
    public void UnionWith(ReadOnlySpan<T> other)
    {
        if (other.Length is 0)
            return;

        _items.EnsureCapacity(_items.Count + other.Length);

        if (_isChangeCollectionEnabled)
        {
            _changeCollector.EnsureCapacity(other.Length);

            foreach(var item in other)
                if (_items.Add(item))
                {
                    _changeCollector.AddChange(DistinctChange.Addition(item));
                    _isDirty = true;
                }
        }
        else
        {
            foreach(var item in other)
                _isDirty |= _items.Add(item);
        }
    }

    bool ICollection<T>.IsReadOnly
        => false;

    void ICollection<T>.Add(T item)
        => Add(item);

    IEnumerator IEnumerable.GetEnumerator()
        => _items.GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
        => _items.GetEnumerator();
}
