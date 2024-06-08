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
/// The basic implementation of <see cref="ISubjectCache{TKey, TItem}"/>, providing simple collection and change notification functionality, with no concurrency or thread-safety.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public sealed class SubjectCache<TKey, TItem>
        : ISubjectCache<TKey, TItem>,
            IObservableCache<TKey, TItem>,
            IDisposable
    where TKey : notnull
{
    private readonly Subject<KeyedChangeSet<TKey, TItem>>   _changeStream;
    private readonly Subject<Unit>                          _collectionChanged;
    private readonly Subject<Unit>                          _notificationsResumed;
    private readonly Action                                 _onChangeStreamFinalized;
    private readonly ChangeTrackingCache<TKey, TItem>       _items;

    private int _notificationSuspensionCount;

    /// <summary>
    /// Constructs a new instance of the <see cref="SubjectCache{TKey, TItem}"/> class.
    /// </summary>
    /// <param name="capacity">The initial number of items that the collection should be able to contain, before needing to allocation additional memory.</param>
    /// <param name="keyComparer">The comparer to be used for matching keys against each other, or <see langword="null"/> if <see cref="EqualityComparer{T}.Default"/> should be used.</param>
    /// <param name="itemComparer">The comparer to be used for detecting item changes, within the collection, or <see langword="null"/> if <see cref="EqualityComparer{T}.Default"/> should be used.</param>
    public SubjectCache(
        Func<TItem, TKey>           keySelector,
        int?                        capacity        = null,
        IEqualityComparer<TKey>?    keyComparer     = null,
        IEqualityComparer<TItem>?   itemComparer    = null)
    {
        _collectionChanged      = new();
        _changeStream           = new();
        _notificationsResumed   = new();
        _items                  = new(
            keySelector:    keySelector,
            capacity:       capacity,
            keyComparer:    keyComparer,
            itemComparer:   itemComparer);

        _onChangeStreamFinalized = () => _items.IsChangeCollectionEnabled = _changeStream.HasObservers;
    }

    /// <inheritdoc/>
    public TItem this[TKey key]
        => _items[key];

    /// <inheritdoc/>
    public IObservable<Unit> CollectionChanged
        => _collectionChanged;

    /// <inheritdoc/>
    public int Count
        => _items.Count;

    /// <inheritdoc cref="ChangeTrackingDictionary{TKey, TItem}.ItemComparer"/>
    public IEqualityComparer<TItem> ItemComparer
        => _items.ItemComparer;

    /// <inheritdoc cref="ChangeTrackingDictionary{TKey, TItem}.KeyComparer"/>
    public IEqualityComparer<TKey> KeyComparer
        => _items.KeyComparer;

    /// <inheritdoc/>
    public IReadOnlyCollection<TKey> Keys
        => _items.Keys;

    /// <inheritdoc/>
    public void Add(TItem value)
    {
        _items.Add(value);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void AddOrReplace(TItem value)
    {
        _items.AddOrReplace(value);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void AddOrReplaceRange(IEnumerable<TItem> values)
    {
        _items.AddOrReplaceRange(values);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void AddOrReplaceRange(ReadOnlySpan<TItem> values)
    {
        _items.AddOrReplaceRange(values);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void Clear()
    {
        _items.Clear();

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public bool Contains(TItem item)
        => _items.Contains(item);

    /// <inheritdoc/>
    public bool ContainsKey(TKey key)
        => _items.ContainsKey(key);

    /// <inheritdoc/>
    public void CopyTo(
            TItem[] array,
            int     arrayIndex)
        => _items.CopyTo(array, arrayIndex);

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

    /// <inheritdoc cref="ChangeTrackingCache{TKey, TItem}.EnsureCapacity(int)"/>
    public void EnsureCapacity(int capacity)
        => _items.EnsureCapacity(capacity);

    /// <inheritdoc/>
    public Dictionary<TKey, TItem>.ValueCollection.Enumerator GetEnumerator()
        => _items.GetEnumerator();

    /// <inheritdoc/>
    public IObservable<TItem> ObserveValue(TKey key)
        => ((_notificationSuspensionCount is 0)
                ? Observable.Empty<TItem>()
                : Observable.Never<TItem>()
                    .TakeUntil(_notificationsResumed))
            .Concat(Observable.Create<TItem>(observer =>
            {
                _items.IsChangeCollectionEnabled = true;

                if (!_items.TryGetItem(key, out var initialItem))
                {
                    observer.OnCompleted();
                    return Disposable.Empty;
                }

                observer.OnNext(initialItem);
                return _changeStream
                    .Finally(_onChangeStreamFinalized)
                    .SubscribeSafe(Observer.Create<KeyedChangeSet<TKey, TItem>>(
                        onNext:         changeSet =>
                        {
                            switch (changeSet.Type)
                            {
                                case ChangeSetType.Clear:
                                    observer.OnCompleted();
                                    break;

                                case ChangeSetType.Reset:
                                    if (_items.TryGetItem(key, out var item))
                                        observer.OnNext(item);
                                    else
                                        observer.OnCompleted();
                                    break;

                                default:
                                    foreach (var change in changeSet.Changes)
                                    {
                                        switch (change.Type)
                                        {
                                            case KeyedChangeType.Removal:
                                                if (_items.KeyComparer.Equals(key, change.AsRemoval().Key))
                                                    observer.OnCompleted();
                                                break;

                                            case KeyedChangeType.Replacement:
                                                var replacement = change.AsReplacement();
                                                if (_items.KeyComparer.Equals(key, replacement.Key))
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
    public bool Remove(TItem item)
    {
        var result = _items.Remove(item);

        PublishPendingNotifications();

        return result;
    }

    /// <inheritdoc/>
    public bool Remove(TKey key)
    {
        var result = _items.Remove(key);

        PublishPendingNotifications();

        return result;
    }

    /// <inheritdoc/>
    public void RemoveRange(IEnumerable<TItem> items)
    {
        _items.RemoveRange(items);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void RemoveRange(ReadOnlySpan<TItem> items)
    {
        _items.RemoveRange(items);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void RemoveRange(IEnumerable<TKey> keys)
    {
        _items.RemoveRange(keys);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void RemoveRange(ReadOnlySpan<TKey> keys)
    {
        _items.RemoveRange(keys);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void Reset(IEnumerable<TItem> items)
    {
        _items.Reset(items);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void Reset(ReadOnlySpan<TItem> items)
    {
        _items.Reset(items);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public IDisposable Subscribe(IObserver<KeyedChangeSet<TKey, TItem>> observer)
    {
        _items.IsChangeCollectionEnabled = true;
        
        return ((_notificationSuspensionCount is 0)
                ? Observable.Empty<Unit>()
                : _notificationsResumed
                    .Take(1))
            .Select(_ => _changeStream
                .Finally(_onChangeStreamFinalized)
                .Prepend(KeyedChangeSet.BulkAddition(_items, _items.KeySelector)))
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
    public bool TryGetItem(
                                        TKey    key,
            [MaybeNullWhen(false)]  out TItem   item)
        => _items.TryGetItem(key, out item);

    bool ICollection<TItem>.IsReadOnly
        => false;

    IReadOnlyCollection<TKey> ICache<TKey, TItem>.Keys
        => _items.Keys;

    IReadOnlyCollection<TKey> IReadOnlyCache<TKey, TItem>.Keys
        => _items.Keys;

    IEnumerator IEnumerable.GetEnumerator()
        => _items.GetEnumerator();

    IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
        => _items.GetEnumerator();

    IDisposable ISubjectCache<TKey, TItem>.SuspendNotifications()
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
        if ((_notificationSuspensionCount is not 0) || !_items.IsDirty)
            return;

        _collectionChanged.OnNext(Unit.Default);

        _changeStream.OnNext(_items.CaptureChangesAndClean());
    }

    /// <summary>
    /// A value that controls suspension of notifications, for a <see cref="SubjectDictionary{TKey, TItem}"/>. Will trigger notifications to be resumed, when disposed.
    /// </summary>
    public struct NotificationSuspension
        : IDisposable
    {
        private SubjectCache<TKey, TItem>? _owner;

        internal NotificationSuspension(SubjectCache<TKey, TItem> owner)
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
