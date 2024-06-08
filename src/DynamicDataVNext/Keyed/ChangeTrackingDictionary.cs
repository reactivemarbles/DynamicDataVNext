using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DynamicDataVNext;

/// <summary>
/// A collection of items, with distinct keys, which tracks and allows for capturing of changes made to it, as they occur.
/// </summary>
/// <typeparam name="TKey">The type of the item keys in the collection.</typeparam>
/// <typeparam name="TValue">The type of the item values in the collection.</typeparam>
public sealed class ChangeTrackingDictionary<TKey, TValue>
        : IExtendedDictionary<TKey, TValue>,
            IReadOnlyDictionary<TKey, TValue>
    where TKey : notnull
{
    private readonly KeyedChangeSet.Builder<TKey, TValue>   _changeCollector;
    private readonly IEqualityComparer<TValue>              _valueComparer;
    private readonly Dictionary<TKey, TValue>               _valuesByKey;

    private bool _isChangeCollectionEnabled;
    private bool _isDirty;

    /// <summary>
    /// Constructs a new instance of the <see cref="ChangeTrackingDictionary{TKey, TValue}"/> class.
    /// </summary>
    /// <param name="capacity">The initial number of items that the collection should be able to contain, before needing to allocation additional memory.</param>
    /// <param name="keyComparer">The comparer to be used for matching keys against each other, or <see langword="null"/> if <see cref="EqualityComparer{T}.Default"/> should be used.</param>
    /// <param name="valueComparer">The comparer to be used for detecting value changes, within the collection, or <see langword="null"/> if <see cref="EqualityComparer{T}.Default"/> should be used.</param>
    public ChangeTrackingDictionary(
        int?                        capacity        = null,
        IEqualityComparer<TKey>?    keyComparer     = null,
        IEqualityComparer<TValue>?  valueComparer   = null)
    {
        _valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;

        _valuesByKey = (capacity is int givenCapacity)
            ? new(
                capacity:   givenCapacity,
                comparer:   keyComparer)
            : new(
                comparer:   keyComparer);

        _changeCollector = new();
        _isChangeCollectionEnabled = true;
    }

    /// <inheritdoc/>
    public TValue this[TKey key]
    {
        get => _valuesByKey[key];
        set
        {
            var wasKeyFound = _valuesByKey.TryGetValue(key, out var oldValue);

            if (wasKeyFound && _valueComparer.Equals(oldValue, value))
                return;

            _valuesByKey[key] = value;

            if (_isChangeCollectionEnabled)
                _changeCollector.AddChange(wasKeyFound
                    ? KeyedChange.Replacement(
                        key:        key,
                        oldItem:    oldValue!,
                        newItem:    value)
                    : KeyedChange.Addition(key, value));

            _isDirty = true;
        }
    }

    /// <inheritdoc/>
    public int Count
        => _valuesByKey.Count;

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
    /// The comparer to be used for matching key values against each other.
    /// </summary>
    public IEqualityComparer<TKey> KeyComparer
        => _valuesByKey.Comparer;

    /// <inheritdoc/>
    public Dictionary<TKey, TValue>.KeyCollection Keys
        => _valuesByKey.Keys;

    /// <summary>
    /// The comparer to be used for detecting value changes, within the collection.
    /// </summary>
    public IEqualityComparer<TValue> ValueComparer
        => _valueComparer;

    /// <inheritdoc/>
    public Dictionary<TKey, TValue>.ValueCollection Values
        => _valuesByKey.Values;

    /// <inheritdoc/>
    public void Add(
        TKey    key,
        TValue  value)
    {
        _valuesByKey.Add(key, value);

        if (_isChangeCollectionEnabled)
            _changeCollector.AddChange(KeyedChange.Addition(key, value));

        _isDirty = true;
    }

    /// <inheritdoc/>
    public void Add(KeyValuePair<TKey, TValue> item)
        => Add(item.Key, item.Value);

    /// <inheritdoc/>
    public void AddOrReplaceRange(
        IEnumerable<TValue> values,
        Func<TValue, TKey>  keySelector)
    {
        ArgumentNullException.ThrowIfNull(values,       nameof(values));
        ArgumentNullException.ThrowIfNull(keySelector,  nameof(keySelector));

        if (values.TryGetNonEnumeratedCount(out var valueCount))
        {
            _valuesByKey.EnsureCapacity(_valuesByKey.Count + valueCount);

            if (_isChangeCollectionEnabled)
                _changeCollector.EnsureCapacity(_changeCollector.Count + valueCount);
        }

        foreach (var value in values)
        {
            var key = keySelector.Invoke(value);

            this[key] = value;
        }
    }

    /// <inheritdoc/>
    public void AddOrReplaceRange(
        ReadOnlySpan<TValue>    values,
        Func<TValue, TKey>      keySelector)
    {
        ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));

        _valuesByKey.EnsureCapacity(_valuesByKey.Count + values.Length);

        if (_isChangeCollectionEnabled)
            _changeCollector.EnsureCapacity(_changeCollector.Count + values.Length);

        foreach (var value in values)
        {
            var key = keySelector.Invoke(value);

            this[key] = value;
        }
    }

    /// <inheritdoc/>
    public void AddOrReplaceRange(IEnumerable<KeyValuePair<TKey, TValue>> items)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        if (items.TryGetNonEnumeratedCount(out var itemCount))
        {
            _valuesByKey.EnsureCapacity(_valuesByKey.Count + itemCount);

            if (_isChangeCollectionEnabled)
                _changeCollector.EnsureCapacity(_changeCollector.Count + itemCount);
        }

        foreach (var item in items)
            this[item.Key] = item.Value;
    }

    /// <inheritdoc/>
    public void AddOrReplaceRange(ReadOnlySpan<KeyValuePair<TKey, TValue>> items)
    {
        _valuesByKey.EnsureCapacity(_valuesByKey.Count + items.Length);

        if (_isChangeCollectionEnabled)
            _changeCollector.EnsureCapacity(_changeCollector.Count + items.Length);

        foreach (var item in items)
            this[item.Key] = item.Value;
    }

    /// <summary>
    /// Captures any previously-collected changes made to the collection, and resets the collection to a "clean" state (I.E. sets <see cref="IsDirty"/> to <see langword="false"/>).
    /// </summary>
    /// <returns>A <see cref="KeyedChangeSet{TKey, TItem}"/> containing all changes made to the collection since its construction, the last call to <see cref="CaptureChangesAndClean"/>, or since <see cref="IsChangeCollectionEnabled"/> was last changed to <see langword="true"/>.</returns>
    /// <remarks>
    /// Note that this method will always return an empty changeset, when <see cref="IsChangeCollectionEnabled"/> is <see langword="false"/>.
    /// </remarks>
    public KeyedChangeSet<TKey, TValue> CaptureChangesAndClean()
    {
        _isDirty = false;
        return _changeCollector.BuildAndClear();
    }

    /// <inheritdoc/>
    public void Clear()
    {
        if (_valuesByKey.Count is 0)
            return;

        if (_isChangeCollectionEnabled)
        {
            _changeCollector.EnsureCapacity(_valuesByKey.Count);

            foreach (var item in _valuesByKey)
                _changeCollector.AddChange(KeyedChange.Removal(item.Key, item.Value));

            _changeCollector.OnSourceCleared();
        }

        _isDirty = true;
    }

    /// <inheritdoc/>
    public bool Contains(KeyValuePair<TKey, TValue> item)
        => _valuesByKey.TryGetValue(item.Key, out var value)
            && _valueComparer.Equals(item.Value, value);

    /// <inheritdoc/>
    public bool ContainsKey(TKey key)
        => _valuesByKey.ContainsKey(key);

    /// <inheritdoc/>
    public void CopyTo(
            KeyValuePair<TKey, TValue>[]    array,
            int                             arrayIndex)
        => ((ICollection<KeyValuePair<TKey, TValue>>)_valuesByKey).CopyTo(array, arrayIndex);

    /// <inheritdoc cref="Dictionary{TKey, TValue}.EnsureCapacity(int)"/>
    public void EnsureCapacity(int capacity)
        => _valuesByKey.EnsureCapacity(capacity);

    /// <inheritdoc/>
    public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        => _valuesByKey.GetEnumerator();

    /// <inheritdoc/>
    public bool Remove(TKey key)
    {
        if (!_valuesByKey.TryGetValue(key, out var value))
            return false;

        if (_isChangeCollectionEnabled)
        {
            _changeCollector.AddChange(KeyedChange.Removal(key, value));
            if (_valuesByKey.Count is 0)
                _changeCollector.OnSourceCleared();
        }

        _isDirty = true;
        return true;
    }

    /// <inheritdoc/>
    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        if (!_valuesByKey.Contains(item))
            return false;

        if (_isChangeCollectionEnabled)
        {
            _changeCollector.AddChange(KeyedChange.Removal(item.Key, item.Value));
            if (_valuesByKey.Count is 0)
                _changeCollector.OnSourceCleared();
        }

        _isDirty = true;
        return true;
    }

    /// <inheritdoc/>
    public void RemoveRange(IEnumerable<TKey> keys)
    {
        if (_valuesByKey.Count is 0)
            return;

        if (_isChangeCollectionEnabled && keys.TryGetNonEnumeratedCount(out var keyCount))
            _changeCollector.EnsureCapacity(keyCount);

        var wereKeysRemoved = false;
        foreach (var key in keys)
            wereKeysRemoved |= Remove(key);

        if ((_isChangeCollectionEnabled) && wereKeysRemoved && (_valuesByKey.Count is 0))
            _changeCollector.OnSourceCleared();
    }

    /// <inheritdoc/>
    public void RemoveRange(ReadOnlySpan<TKey> keys)
    {
        if (_valuesByKey.Count is 0)
            return;

        _changeCollector.EnsureCapacity(keys.Length);

        var wereKeysRemoved = false;
        foreach (var key in keys)
            wereKeysRemoved |= Remove(key);

        if ((_isChangeCollectionEnabled) && wereKeysRemoved && (_valuesByKey.Count is 0))
            _changeCollector.OnSourceCleared();
    }

    /// <inheritdoc/>
    public void Reset(
        IEnumerable<TValue> values,
        Func<TValue, TKey>  keySelector)
    {
        Clear();
        AddOrReplaceRange(values, keySelector);
    }

    /// <inheritdoc/>
    public void Reset(
        ReadOnlySpan<TValue>    values,
        Func<TValue, TKey>      keySelector)
    {
        Clear();
        AddOrReplaceRange(values, keySelector);
    }

    /// <inheritdoc/>
    public void Reset(IEnumerable<KeyValuePair<TKey, TValue>> items)
    {
        Clear();
        AddOrReplaceRange(items);
    }

    /// <inheritdoc/>
    public void Reset(ReadOnlySpan<KeyValuePair<TKey, TValue>> items)
    {
        Clear();
        AddOrReplaceRange(items);
    }

    /// <inheritdoc/>
    public bool TryGetValue(
                                        TKey    key,
            [MaybeNullWhen(false)]  out TValue  value)
        => _valuesByKey.TryGetValue(key, out value);

    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        => false;

    ICollection<TKey> IDictionary<TKey, TValue>.Keys
        => _valuesByKey.Keys;

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        => _valuesByKey.Keys;

    IReadOnlyCollection<TKey> IExtendedDictionary<TKey, TValue>.Keys
        => _valuesByKey.Keys;

    ICollection<TValue> IDictionary<TKey, TValue>.Values
        => _valuesByKey.Values;

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        => _valuesByKey.Values;

    IReadOnlyCollection<TValue> IExtendedDictionary<TKey, TValue>.Values
        => _valuesByKey.Values;

    IEnumerator IEnumerable.GetEnumerator()
        => _valuesByKey.GetEnumerator();

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        => _valuesByKey.GetEnumerator();
}
