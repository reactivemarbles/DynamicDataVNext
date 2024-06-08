using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace DynamicDataVNext;

/// <summary>
/// The basic implementation of <see cref="ISubjectDictionary{TKey, TValue}"/>, providing simple collection and change notification functionality, with no concurrency or thread-safety.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public sealed class SubjectDictionary<TKey, TValue>
        : ISubjectDictionary<TKey, TValue>,
            IObservableDictionary<TKey, TValue>,
            IDisposable
    where TKey : notnull
{
    private readonly Subject<KeyedChangeSet<TKey, TValue>>  _changeStream;
    private readonly Subject<Unit>                          _collectionChanged;
    private readonly Subject<Unit>                          _notificationsResumed;
    private readonly Action                                 _onChangeStreamFinalized;
    private readonly ChangeTrackingDictionary<TKey, TValue> _valuesByKey;

    private int _notificationSuspensionCount;

    /// <summary>
    /// Constructs a new instance of the <see cref="SubjectDictionary{TKey, TValue}"/> class.
    /// </summary>
    /// <param name="capacity">The initial number of items that the collection should be able to contain, before needing to allocation additional memory.</param>
    /// <param name="keyComparer">The comparer to be used for matching keys against each other, or <see langword="null"/> if <see cref="EqualityComparer{T}.Default"/> should be used.</param>
    /// <param name="valueComparer">The comparer to be used for detecting value changes, within the collection, or <see langword="null"/> if <see cref="EqualityComparer{T}.Default"/> should be used.</param>
    public SubjectDictionary(
        int?                        capacity        = null,
        IEqualityComparer<TKey>?    keyComparer     = null,
        IEqualityComparer<TValue>?  valueComparer   = null)
    {
        _collectionChanged      = new();
        _changeStream           = new();
        _notificationsResumed   = new();
        _valuesByKey            = new(
            capacity:       capacity,
            keyComparer:    keyComparer,
            valueComparer:  valueComparer);

        _onChangeStreamFinalized = () => _valuesByKey.IsChangeCollectionEnabled = _changeStream.HasObservers;
    }

    /// <inheritdoc/>
    public TValue this[TKey key]
    {
        get => _valuesByKey[key];
        set
        {
            _valuesByKey[key] = value;

            PublishPendingNotifications();
        }
    }

    /// <inheritdoc/>
    public IObservable<Unit> CollectionChanged
        => _collectionChanged;

    /// <inheritdoc/>
    public int Count
        => _valuesByKey.Count;

    /// <inheritdoc cref="ChangeTrackingDictionary{TKey, TValue}.KeyComparer"/>
    public IEqualityComparer<TKey> KeyComparer
        => _valuesByKey.KeyComparer;

    /// <inheritdoc/>
    public IReadOnlyCollection<TKey> Keys
        => _valuesByKey.Keys;

    /// <inheritdoc cref="ChangeTrackingDictionary{TKey, TValue}.ValueComparer"/>
    public IEqualityComparer<TValue> ValueComparer
        => _valuesByKey.ValueComparer;

    /// <inheritdoc/>
    public IReadOnlyCollection<TValue> Values
        => _valuesByKey.Values;

    /// <inheritdoc/>
    public void Add(TKey key, TValue value)
    {
        _valuesByKey.Add(key, value);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void Add(KeyValuePair<TKey, TValue> item)
    {
        _valuesByKey.Add(item);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void AddOrReplaceRange(
        IEnumerable<TValue> values,
        Func<TValue, TKey>  keySelector)
    {
        _valuesByKey.AddOrReplaceRange(values, keySelector);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void AddOrReplaceRange(
        ReadOnlySpan<TValue>    values,
        Func<TValue, TKey>      keySelector)
    {
        _valuesByKey.AddOrReplaceRange(values, keySelector);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void AddOrReplaceRange(IEnumerable<KeyValuePair<TKey, TValue>> items)
    {
        _valuesByKey.AddOrReplaceRange(items);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void AddOrReplaceRange(ReadOnlySpan<KeyValuePair<TKey, TValue>> items)
    {
        _valuesByKey.AddOrReplaceRange(items);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void Clear()
    {
        _valuesByKey.Clear();

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public bool Contains(KeyValuePair<TKey, TValue> item)
        => _valuesByKey.Contains(item);

    /// <inheritdoc/>
    public bool ContainsKey(TKey key)
        => _valuesByKey.ContainsKey(key);

    /// <inheritdoc/>
    public void CopyTo(
            KeyValuePair<TKey, TValue>[]    array,
            int                             arrayIndex)
        => _valuesByKey.CopyTo(array, arrayIndex);

    /// <inheritdoc/>
    public void Dispose()
    {
        _changeStream           .OnCompleted();
        _collectionChanged      .OnCompleted();
        _notificationsResumed   .OnCompleted();

        _changeStream           .Dispose();
        _collectionChanged      .Dispose();
        _notificationsResumed   .Dispose();
    }

    /// <inheritdoc cref="ChangeTrackingDictionary{TKey, TValue}.EnsureCapacity(int)"/>
    public void EnsureCapacity(int capacity)
        => _valuesByKey.EnsureCapacity(capacity);

    /// <inheritdoc/>
    public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        => _valuesByKey.GetEnumerator();

    /// <inheritdoc/>
    public IObservable<TValue> ObserveValue(TKey key)
        => ((_notificationSuspensionCount is 0)
                ? Observable.Empty<TValue>()
                : Observable.Never<TValue>()
                    .TakeUntil(_notificationsResumed))
            .Concat(Observable.Create<TValue>(observer =>
            {
                _valuesByKey.IsChangeCollectionEnabled = true;

                if (!_valuesByKey.TryGetValue(key, out var initialValue))
                {
                    observer.OnCompleted();
                    return Disposable.Empty;
                }

                observer.OnNext(initialValue);
                return _changeStream
                    .Finally(_onChangeStreamFinalized)
                    .SubscribeSafe(Observer.Create<KeyedChangeSet<TKey, TValue>>(
                        onNext:         changeSet =>
                        {
                            switch (changeSet.Type)
                            {
                                case ChangeSetType.Clear:
                                    observer.OnCompleted();
                                    break;

                                case ChangeSetType.Reset:
                                    if (_valuesByKey.TryGetValue(key, out var value))
                                        observer.OnNext(value);
                                    else
                                        observer.OnCompleted();
                                    break;

                                default:
                                    foreach (var change in changeSet.Changes)
                                    {
                                        switch (change.Type)
                                        {
                                            case KeyedChangeType.Removal:
                                                if (_valuesByKey.KeyComparer.Equals(key, change.AsRemoval().Key))
                                                    observer.OnCompleted();
                                                break;

                                            case KeyedChangeType.Replacement:
                                                var replacement = change.AsReplacement();
                                                if (_valuesByKey.KeyComparer.Equals(key, replacement.Key))
                                                    observer.OnNext(replacement.NewItem);
                                                break;
                                        }
                                    }
                                    break;
                            }
                        },
                        onError:        observer.OnError,
                        onCompleted:    observer.OnCompleted));
            }));

    /// <inheritdoc/>
    public bool Remove(TKey key)
    {
        var result = _valuesByKey.Remove(key);

        PublishPendingNotifications();

        return result;
    }

    /// <inheritdoc/>
    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        var result = _valuesByKey.Remove(item);

        PublishPendingNotifications();

        return result;
    }

    /// <inheritdoc/>
    public void RemoveRange(IEnumerable<TKey> keys)
    {
        _valuesByKey.RemoveRange(keys);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void RemoveRange(ReadOnlySpan<TKey> keys)
    {
        _valuesByKey.RemoveRange(keys);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void Reset(
        IEnumerable<TValue> values,
        Func<TValue, TKey>  keySelector)
    {
        _valuesByKey.Reset(values, keySelector);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void Reset(
        ReadOnlySpan<TValue>    values,
        Func<TValue, TKey>      keySelector)
    {
        _valuesByKey.Reset(values, keySelector);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void Reset(IEnumerable<KeyValuePair<TKey, TValue>> items)
    {
        _valuesByKey.Reset(items);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void Reset(ReadOnlySpan<KeyValuePair<TKey, TValue>> items)
    {
        _valuesByKey.Reset(items);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public IDisposable Subscribe(IObserver<KeyedChangeSet<TKey, TValue>> observer)
    {
        _valuesByKey.IsChangeCollectionEnabled = true;
        
        return ((_notificationSuspensionCount is 0)
                ? Observable.Empty<Unit>()
                : _notificationsResumed
                    .Take(1))
            .Select(_ => _changeStream
                .Finally(_onChangeStreamFinalized)
                .Prepend(KeyedChangeSet.BulkAddition(_valuesByKey)))
            .Switch()
            .Subscribe(observer);
    }

    /// <inheritdoc/>
    public NotificationSuspension SuspendNotifications()
    {
        ++_notificationSuspensionCount;
        return new(this);
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

    ICollection<TValue> IDictionary<TKey, TValue>.Values
        => _valuesByKey.Values;

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        => _valuesByKey.Values;

    IEnumerator IEnumerable.GetEnumerator()
        => _valuesByKey.GetEnumerator();

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        => _valuesByKey.GetEnumerator();

    IDisposable ISubjectDictionary<TKey, TValue>.SuspendNotifications()
        => SuspendNotifications();

    private void OnNotificationSuspensionDisposed()
    {
        --_notificationSuspensionCount;
        if (_notificationSuspensionCount is 0)
        {
            PublishPendingNotifications();

            _notificationsResumed.OnNext(Unit.Default);
        }
    }

    private void PublishPendingNotifications()
    {
        if ((_notificationSuspensionCount is not 0) || !_valuesByKey.IsDirty)
            return;

        _collectionChanged.OnNext(Unit.Default);

        _changeStream.OnNext(_valuesByKey.CaptureChangesAndClean());
    }

    /// <summary>
    /// A value that controls suspension of notifications, for a <see cref="SubjectDictionary{TKey, TValue}"/>. Will trigger notifications to be resumed, when disposed.
    /// </summary>
    public struct NotificationSuspension
        : IDisposable
    {
        private SubjectDictionary<TKey, TValue>? _owner;

        internal NotificationSuspension(SubjectDictionary<TKey, TValue> owner)
            => _owner = owner;

        /// <inheritdoc/>
        public void Dispose()
        {
            if (_owner is not null)
            {
                _owner.OnNotificationSuspensionDisposed();
                _owner = null;
            }
        }
    }
}
