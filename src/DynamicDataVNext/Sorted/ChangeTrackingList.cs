using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DynamicDataVNext;

public sealed class ChangeTrackingList<T>
    : IExtendedList<T>,
        IReadOnlyList<T>
{
    private readonly SortedChangeSet.Builder<T> _changeCollector;
    private readonly IEqualityComparer<T>       _comparer;
    private readonly List<T>                    _items;

    private bool _isChangeCollectionEnabled;
    private bool _isDirty;

    /// <summary>
    /// Constructs a new instance of the <see cref="ChangeTrackingList{T}"/> class.
    /// </summary>
    /// <param name="capacity">The initial number of items that the collection should be able to contain, before needing to allocation additional memory.</param>
    /// <param name="comparer">The comparer to be used for detecting item changes, within the collection, or <see langword="null"/> if <see cref="EqualityComparer{T}.Default"/> should be used.</param>
    public ChangeTrackingList(
        int?                    capacity = null,
        IEqualityComparer<T>?   comparer = null)
    {
        _comparer = comparer ?? EqualityComparer<T>.Default;

        _items = (capacity is int givenCapacity)
            ? new(capacity: givenCapacity)
            : new();

        _changeCollector = new();
        _isChangeCollectionEnabled = true;
    }

    /// <inheritdoc/>
    public T this[int index]
    {
        get => _items[index];
        set
        {
            if (_items.Count > index)
            {
                var oldValue = _items[index];

                if (_comparer.Equals(oldValue, value))
                    return;

                _items[index] = value;

                if (_isChangeCollectionEnabled)
                    _changeCollector.AddChange(SortedChange.Replacement(
                        index:      index,
                        oldItem:    oldValue,
                        newItem:    value));
            }
            else
            {
                _items.Add(value);

                if (_isChangeCollectionEnabled)
                    _changeCollector.AddChange(SortedChange.Insertion(
                        index:  index,
                        item:   value));
            }

            _isDirty = true;
        }
    }

    /// <summary>
    /// The comparer to be used for detecting value changes, within the collection.
    /// </summary>
    public IEqualityComparer<T> Comparer
        => _comparer;

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

    /// <inheritdoc/>
    public void Add(T item)
    {
        _items.Add(item);

        if (_isChangeCollectionEnabled)
            _changeCollector.AddChange(SortedChange.Insertion(
                index:  _items.Count - 1,
                item:   item));

        _isDirty = true;
    }

    /// <inheritdoc/>
    public void AddRange(IEnumerable<T> items)
    {
        if (_isChangeCollectionEnabled)
        {
            if (items.TryGetNonEnumeratedCount(out var itemCount))
            {
                _items.EnsureCapacity(_items.Count + itemCount);

                _changeCollector.EnsureCapacity(itemCount);
            }

            foreach (var item in items)
            {
                _items.Add(item);

                _changeCollector.AddChange(SortedChange.Insertion(
                    index:  _items.Count - 1,
                    item:   item));

                _isDirty = true;
            }
        }
        else
        {
            var oldCount = _items.Count;

            _items.AddRange(items);

            _isDirty = oldCount != _items.Count;
        }
    }

    /// <inheritdoc/>
    public void AddRange(ReadOnlySpan<T> items)
    {
        if (items.Length is 0)
            return;

        if (_isChangeCollectionEnabled)
        {
            _items.EnsureCapacity(_items.Count + items.Length);

            _changeCollector.EnsureCapacity(items.Length);

            foreach (var item in items)
            {
                _items.Add(item);

                _changeCollector.AddChange(SortedChange.Insertion(
                    index:  _items.Count - 1,
                    item:   item));
            }
        }
        else
        {
            _items.AddRange(items);
        }

        _isDirty = true;
    }

    /// <summary>
    /// Captures any previously-collected changes made to the collection, and resets the collection to a "clean" state (I.E. sets <see cref="IsDirty"/> to <see langword="false"/>).
    /// </summary>
    /// <returns>A <see cref="SortedChangeSet{T}"/> containing all changes made to the collection since its construction, the last call to <see cref="CaptureChangesAndClean"/>, or since <see cref="IsChangeCollectionEnabled"/> was last changed to <see langword="true"/>.</returns>
    /// <remarks>
    /// Note that this method will always return an empty changeset, when <see cref="IsChangeCollectionEnabled"/> is <see langword="false"/>.
    /// </remarks>
    public SortedChangeSet<T> CaptureChangesAndClean()
    {
        _isDirty = false;
        return _changeCollector.BuildAndClear();
    }

    /// <inheritdoc/>
    public void Clear()
    {
        if (_items.Count is 0)
            return;

        _changeCollector.EnsureCapacity(_items.Count);

        if (_isChangeCollectionEnabled)
        {
            for (var index = _items.Count - 1; index >= 0; --index)
                _changeCollector.AddChange(SortedChange.Removal(
                    index:  index,
                    item:   _items[index]));

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

    /// <inheritdoc cref="List{T}.EnsureCapacity(int)"/>
    public void EnsureCapacity(int capacity)
        => _items.EnsureCapacity(capacity);

    /// <inheritdoc/>
    public List<T>.Enumerator GetEnumerator()
        => _items.GetEnumerator();

    /// <inheritdoc/>
    public int IndexOf(T item)
        => _items.IndexOf(item);

    /// <inheritdoc/>
    public void Insert(
            int index,
            T   item)
        => _items.Insert(index, item);

    /// <inheritdoc/>
    public void InsertRange(
        int             index,
        IEnumerable<T>  items)
    {
        if (_isChangeCollectionEnabled)
        {
            if (items.TryGetNonEnumeratedCount(out var itemCount))
            {
                _items.EnsureCapacity(_items.Count + itemCount);

                _changeCollector.EnsureCapacity(itemCount);
            }

            var insertionIndex = index;
            foreach (var item in items)
            {
                _items.Insert(
                    index:  insertionIndex,
                    item:   item);

                _changeCollector.AddChange(SortedChange.Insertion(
                    index:  insertionIndex,
                    item:   item));

                ++insertionIndex;
                _isDirty = true;
            }
        }
        else
        {
            var oldCount = _items.Count;

            _items.InsertRange(index, items);

            _isDirty = oldCount != _items.Count;
        }
    }

    /// <inheritdoc/>
    public void InsertRange(
        int             index,
        ReadOnlySpan<T> items)
    {
        if (items.Length is 0)
            return;

        if (_isChangeCollectionEnabled)
        {
            _items.EnsureCapacity(_items.Count + items.Length);

            _changeCollector.EnsureCapacity(items.Length);

            var insertionIndex = index;
            foreach (var item in items)
            {
                _items.Insert(
                    index:  insertionIndex,
                    item:   item);

                _changeCollector.AddChange(SortedChange.Insertion(
                    index:  insertionIndex,
                    item:   item));

                ++insertionIndex;
            }
        }
        else
        {
            _items.InsertRange(index, items);
        }

        _isDirty = true;
    }

    /// <inheritdoc/>
    public void Move(int oldIndex, int newIndex)
    {
        if (oldIndex == newIndex)
            return;

        var item = _items[oldIndex];

        _items.RemoveAt(oldIndex);
        _items.Insert(
            index:  newIndex,
            item:   item);

        if (_isChangeCollectionEnabled)
            _changeCollector.AddChange(SortedChange.Movement(oldIndex, newIndex, item));

        _isDirty = true;
    }

    /// <inheritdoc/>
    public bool Remove(T item)
    {
        if (_isChangeCollectionEnabled)
        {
            var index = _items.IndexOf(item);
            if (index < 0)
                return false;

            _items.RemoveAt(index);

            _isDirty = true;
            return true;
        }
        else
        {
            var result = _items.Remove(item);

            _isDirty |= result;

            return result;
        }
    }

    /// <inheritdoc/>
    public void RemoveAt(int index)
    {
        if (_isChangeCollectionEnabled)
        {
            var item = _items[index];

            _changeCollector.AddChange(SortedChange.Removal(index, item));
        }

        _items.RemoveAt(index);

        _isDirty = true;
    }

    /// <inheritdoc/>
    public void RemoveRange(
        int index,
        int count)
    {
        if (count is 0)
            return;

        if (_isChangeCollectionEnabled)
        {
            for (var i = index + count - 1; i >= index; --i)
            {
                var item = _items[i];

                _changeCollector.AddChange(SortedChange.Removal(
                    index:  i,
                    item:   item));
            }

            if (_items.Count == count)
                _changeCollector.OnSourceCleared();
        }
        
        _items.RemoveRange(index, count);

        _isDirty = true;
    }

    bool ICollection<T>.IsReadOnly
        => false;

    IEnumerator IEnumerable.GetEnumerator()
        => _items.GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
        => _items.GetEnumerator();
}
