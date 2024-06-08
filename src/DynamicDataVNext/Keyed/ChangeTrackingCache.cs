using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Headers;

namespace DynamicDataVNext;

/// <summary>
/// A collection of distinctly-keyed items, which tracks and allows for capturing of changes made to it, as they occur.
/// </summary>
/// <typeparam name="TKey">The type of the key values of items in the collection.</typeparam>
/// <typeparam name="TItem">The type of the items in the collection.</typeparam>
public sealed class ChangeTrackingCache<TKey, TItem>
        : ICache<TKey, TItem>,
            IReadOnlyCache<TKey, TItem>
    where TKey : notnull
{
    private readonly KeyedChangeSet.Builder<TKey, TItem>    _changeCollector;
    private readonly IEqualityComparer<TItem>               _itemComparer;
    private readonly Dictionary<TKey, TItem>                _itemsByKey;
    private readonly Func<TItem, TKey>                      _keySelector;

    private bool _isChangeCollectionEnabled;
    private bool _isDirty;

    /// <summary>
    /// Constructs a new instance of the <see cref="ChangeTrackingCache{TKey, TItem}"/> class.
    /// </summary>
    /// <param name="capacity">The initial number of items that the collection should be able to contain, before needing to allocation additional memory.</param>
    /// <param name="keyComparer">The comparer to be used for matching keys against each other, or <see langword="null"/> if <see cref="EqualityComparer{T}.Default"/> should be used.</param>
    /// <param name="itemComparer">The comparer to be used for detecting item changes, within the collection, or <see langword="null"/> if <see cref="EqualityComparer{T}.Default"/> should be used.</param>
    public ChangeTrackingCache(
        Func<TItem, TKey>           keySelector,
        int?                        capacity        = null,
        IEqualityComparer<TKey>?    keyComparer     = null,
        IEqualityComparer<TItem>?   itemComparer    = null)
    {
        _keySelector = keySelector;

        _itemComparer = itemComparer ?? EqualityComparer<TItem>.Default;

        _itemsByKey = (capacity is int givenCapacity)
            ? new(
                capacity:   givenCapacity,
                comparer:   keyComparer)
            : new(
                comparer:   keyComparer);

        _changeCollector = new();
        _isChangeCollectionEnabled = true;
    }

    /// <inheritdoc/>
    public TItem this[TKey key]
        => _itemsByKey[key];

    /// <inheritdoc/>
    public int Count
        => _itemsByKey.Count;

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
    /// The comparer to be used for detecting value changes, within the collection.
    /// </summary>
    public IEqualityComparer<TItem> ItemComparer
        => _itemComparer;

    /// <summary>
    /// The comparer to be used for matching key values against each other.
    /// </summary>
    public IEqualityComparer<TKey> KeyComparer
        => _itemsByKey.Comparer;

    /// <summary>
    /// The function used by the collection to identify the key values of items.
    /// </summary>
    public Func<TItem, TKey> KeySelector
        => _keySelector;

    /// <inheritdoc/>
    public Dictionary<TKey, TItem>.KeyCollection Keys
        => _itemsByKey.Keys;

    /// <inheritdoc/>
    public void Add(TItem item)
    {
        var key = _keySelector.Invoke(item);

        _itemsByKey.Add(key, item);

        if (_isChangeCollectionEnabled)
            _changeCollector.AddChange(KeyedChange.Addition(key, item));

        _isDirty = true;
    }

    /// <inheritdoc/>
    public void AddOrReplace(TItem item)
    {
        var key = _keySelector.Invoke(item);

        var wasKeyFound = _itemsByKey.TryGetValue(key, out var oldItem);

        if (wasKeyFound && _itemComparer.Equals(oldItem, item))
            return;

        _itemsByKey[key] = item;

        if (_isChangeCollectionEnabled)
            _changeCollector.AddChange(wasKeyFound
                ? KeyedChange.Replacement(
                    key:        key,
                    oldItem:    oldItem!,
                    newItem:    item)
                : KeyedChange.Addition(key, item));

        _isDirty = true;
    }

    /// <inheritdoc/>
    public void AddOrReplaceRange(IEnumerable<TItem> values)
    {
        ArgumentNullException.ThrowIfNull(values, nameof(values));

        if (values.TryGetNonEnumeratedCount(out var valueCount))
        {
            _itemsByKey.EnsureCapacity(_itemsByKey.Count + valueCount);

            if (_isChangeCollectionEnabled)
                _changeCollector.EnsureCapacity(_changeCollector.Count + valueCount);
        }

        foreach (var value in values)
            AddOrReplace(value);
    }

    /// <inheritdoc/>
    public void AddOrReplaceRange(ReadOnlySpan<TItem> values)
    {
        _itemsByKey.EnsureCapacity(_itemsByKey.Count + values.Length);

        if (_isChangeCollectionEnabled)
            _changeCollector.EnsureCapacity(_changeCollector.Count + values.Length);

        foreach (var value in values)
            AddOrReplace(value);
    }

    /// <summary>
    /// Captures any previously-collected changes made to the collection, and resets the collection to a "clean" state (I.E. sets <see cref="IsDirty"/> to <see langword="false"/>).
    /// </summary>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> containing all changes made to the collection since its construction, the last call to <see cref="CaptureChangesAndClean"/>, or since <see cref="IsChangeCollectionEnabled"/> was last changed to <see langword="true"/>.</returns>
    /// <remarks>
    /// Note that this method will always return an empty changeset, when <see cref="IsChangeCollectionEnabled"/> is <see langword="false"/>.
    /// </remarks>
    public KeyedChangeSet<TKey, TItem> CaptureChangesAndClean()
    {
        _isDirty = false;
        return _changeCollector.BuildAndClear();
    }

    /// <inheritdoc/>
    public void Clear()
    {
        if (_itemsByKey.Count is 0)
            return;

        if (_isChangeCollectionEnabled)
        {
            _changeCollector.EnsureCapacity(_itemsByKey.Count);

            foreach (var item in _itemsByKey)
                _changeCollector.AddChange(KeyedChange.Removal(item.Key, item.Value));

            _changeCollector.OnSourceCleared();
        }

        _isDirty = true;
    }

    /// <inheritdoc/>
    public bool Contains(TItem item)
        => _itemsByKey.TryGetValue(_keySelector.Invoke(item), out var existingItem)
            && _itemComparer.Equals(item, existingItem);

    /// <inheritdoc/>
    public bool ContainsKey(TKey key)
        => _itemsByKey.ContainsKey(key);

    /// <inheritdoc/>
    public void CopyTo(
            TItem[] array,
            int     arrayIndex)
        => _itemsByKey.Values.CopyTo(array, arrayIndex);

    /// <inheritdoc cref="Dictionary{TKey, TItem}.EnsureCapacity(int)"/>
    public void EnsureCapacity(int capacity)
        => _itemsByKey.EnsureCapacity(capacity);

    /// <inheritdoc/>
    public Dictionary<TKey, TItem>.ValueCollection.Enumerator GetEnumerator()
        => _itemsByKey.Values.GetEnumerator();

    /// <inheritdoc/>
    public bool Remove(TItem item)
    {
        var key = _keySelector.Invoke(item);

        if (!_itemsByKey.TryGetValue(key, out var existingItem)
                || !_itemComparer.Equals(item, existingItem))
            return false;

        if (_isChangeCollectionEnabled)
        {
            _changeCollector.AddChange(KeyedChange.Removal(key, item));
            if (_itemsByKey.Count is 0)
                _changeCollector.OnSourceCleared();
        }

        _isDirty = true;
        return true;
    }

    /// <inheritdoc/>
    public bool Remove(TKey key)
    {
        if (!_itemsByKey.TryGetValue(key, out var value))
            return false;

        if (_isChangeCollectionEnabled)
        {
            _changeCollector.AddChange(KeyedChange.Removal(key, value));
            if (_itemsByKey.Count is 0)
                _changeCollector.OnSourceCleared();
        }

        _isDirty = true;
        return true;
    }

    /// <inheritdoc/>
    public void RemoveRange(IEnumerable<TItem> items)
    {
        if (_itemsByKey.Count is 0)
            return;

        if (_isChangeCollectionEnabled && items.TryGetNonEnumeratedCount(out var itemCount))
            _changeCollector.EnsureCapacity(itemCount);

        var wereItemsRemoved = false;
        foreach (var item in items)
            wereItemsRemoved |= Remove(item);

        if ((_isChangeCollectionEnabled) && wereItemsRemoved && (_itemsByKey.Count is 0))
            _changeCollector.OnSourceCleared();
    }

    /// <inheritdoc/>
    public void RemoveRange(ReadOnlySpan<TItem> items)
    {
        if (_itemsByKey.Count is 0)
            return;

        _changeCollector.EnsureCapacity(items.Length);

        var wereItemsRemoved = false;
        foreach (var item in items)
            wereItemsRemoved |= Remove(item);

        if ((_isChangeCollectionEnabled) && wereItemsRemoved && (_itemsByKey.Count is 0))
            _changeCollector.OnSourceCleared();
    }

    /// <inheritdoc/>
    public void RemoveRange(IEnumerable<TKey> keys)
    {
        if (_itemsByKey.Count is 0)
            return;

        if (_isChangeCollectionEnabled && keys.TryGetNonEnumeratedCount(out var keyCount))
            _changeCollector.EnsureCapacity(keyCount);

        var wereKeysRemoved = false;
        foreach (var key in keys)
            wereKeysRemoved |= Remove(key);

        if ((_isChangeCollectionEnabled) && wereKeysRemoved && (_itemsByKey.Count is 0))
            _changeCollector.OnSourceCleared();
    }

    /// <inheritdoc/>
    public void RemoveRange(ReadOnlySpan<TKey> keys)
    {
        if (_itemsByKey.Count is 0)
            return;

        _changeCollector.EnsureCapacity(keys.Length);

        var wereKeysRemoved = false;
        foreach (var key in keys)
            wereKeysRemoved |= Remove(key);

        if ((_isChangeCollectionEnabled) && wereKeysRemoved && (_itemsByKey.Count is 0))
            _changeCollector.OnSourceCleared();
    }

    /// <inheritdoc/>
    public void Reset(IEnumerable<TItem> items)
    {
        Clear();
        AddOrReplaceRange(items);
    }

    /// <inheritdoc/>
    public void Reset(ReadOnlySpan<TItem> values)
    {
        Clear();
        AddOrReplaceRange(values);
    }

    /// <inheritdoc/>
    public bool TryGetItem(
                                        TKey    key,
            [MaybeNullWhen(false)]  out TItem   item)
        => _itemsByKey.TryGetValue(key, out item);

    IReadOnlyCollection<TKey> ICache<TKey, TItem>.Keys
        => _itemsByKey.Keys;

    IReadOnlyCollection<TKey> IReadOnlyCache<TKey, TItem>.Keys
        => _itemsByKey.Keys;

    bool ICollection<TItem>.IsReadOnly
        => false;

    IEnumerator IEnumerable.GetEnumerator()
        => _itemsByKey.Values.GetEnumerator();

    IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
        => _itemsByKey.Values.GetEnumerator();
}
