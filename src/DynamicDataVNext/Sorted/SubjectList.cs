using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace DynamicDataVNext;

/// <summary>
/// The basic implementation of <see cref="ISubjectList{T}"/>, providing simple collection and change notification functionality, with no concurrency or thread-safety.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public sealed class SubjectList<T>
        : ISubjectList<T>,
            IObservableList<T>,
            IDisposable
    where T : notnull
{
    private readonly Subject<SortedChangeSet<T>>    _changeStream;
    private readonly Subject<Unit>                  _collectionChanged;
    private readonly Subject<Unit>                  _notificationsResumed;
    private readonly Action                         _onChangeStreamFinalized;
    private readonly ChangeTrackingList<T>          _items;

    private int _notificationSuspensionCount;

    /// <summary>
    /// Constructs a new instance of the <see cref="SubjectSet{T}"/> class.
    /// </summary>
    /// <param name="capacity">The initial number of items that the collection should be able to contain, before needing to allocation additional memory.</param>
    /// <param name="comparer">The comparer to be used for detecting item changes, within the collection, or <see langword="null"/> if <see cref="EqualityComparer{T}.Default"/> should be used.</param>
    public SubjectList(
        int?                    capacity = null,
        IEqualityComparer<T>?   comparer = null)
    {
        _collectionChanged      = new();
        _changeStream           = new();
        _notificationsResumed   = new();
        _items                  = new(
            capacity:   capacity,
            comparer:   comparer);

        _onChangeStreamFinalized = () => _items.IsChangeCollectionEnabled = _changeStream.HasObservers;
    }

    /// <inheritdoc/>
    public T this[int index]
    {
        get => _items[index];
        set
        {
            _items[index] = value;

            PublishPendingNotifications();
        }
    }

    /// <inheritdoc/>
    public IObservable<Unit> CollectionChanged
        => _collectionChanged;

    /// <inheritdoc/>
    public int Count
        => _items.Count;

    /// <inheritdoc/>
    public void Add(T item)
    {
        _items.Add(item);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void AddRange(IEnumerable<T> items)
    {
        _items.AddRange(items);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void AddRange(ReadOnlySpan<T> items)
    {
        _items.AddRange(items);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void Clear()
    {
        _items.Clear();

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public bool Contains(T item)
        => _items.Contains(item);

    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex)
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

    /// <inheritdoc cref="ChangeTrackingList{T}.EnsureCapacity(int)"/>
    public void EnsureCapacity(int capacity)
        => _items.EnsureCapacity(capacity);

    /// <inheritdoc/>
    public List<T>.Enumerator GetEnumerator()
        => _items.GetEnumerator();

    /// <inheritdoc/>
    public int IndexOf(T item)
        => _items.IndexOf(item);

    /// <inheritdoc/>
    public void Insert(int index, T item)
    {
        _items.Insert(index, item);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void InsertRange(int index, IEnumerable<T> items)
    {
        _items.InsertRange(index, items);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void InsertRange(int index, ReadOnlySpan<T> items)
    {
        _items.InsertRange(index, items);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void Move(int oldIndex, int newIndex)
    {
        _items.Move(oldIndex, newIndex);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public IObservable<T> ObserveValue(int index)
        => ((_notificationSuspensionCount is 0)
                ? Observable.Empty<T>()
                : Observable.Never<T>()
                    .TakeUntil(_notificationsResumed))
            .Concat(Observable.Create<T>(observer =>
            {
                _items.IsChangeCollectionEnabled = true;

                if (index >= _items.Count)
                {
                    observer.OnCompleted();
                    return Disposable.Empty;
                }

                var oldItem = _items[index];
                observer.OnNext(oldItem);
                return _changeStream
                    .Finally(_onChangeStreamFinalized)
                    .SubscribeSafe(Observer.Create<SortedChangeSet<T>>(
                        onNext:         changeSet =>
                        {
                            switch (changeSet.Type)
                            {
                                case ChangeSetType.Clear:
                                    observer.OnCompleted();
                                    break;

                                case ChangeSetType.Reset:
                                    if (index < _items.Count)
                                    {
                                        oldItem = _items[index];
                                        observer.OnNext(oldItem);
                                    }
                                    else
                                        observer.OnCompleted();
                                    break;

                                default:
                                    if (index < _items.Count)
                                    {
                                        var newItem = _items[index];
                                        if (_items.Comparer.Equals(oldItem, newItem))
                                            break;
                                        oldItem = newItem;
                                        observer.OnNext(newItem);
                                    }
                                    else
                                        observer.OnCompleted();
                                    break;
                            }
                        },
                        onError:        observer.OnError,
                        onCompleted:    observer.OnCompleted));
            }));

    /// <inheritdoc/>
    public bool Remove(T item)
    {
        var result = _items.Remove(item);

        PublishPendingNotifications();

        return result;
    }

    /// <inheritdoc/>
    public void RemoveAt(int index)
    {
        _items.RemoveAt(index);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void RemoveRange(int index, int count)
    {
        _items.RemoveRange(index, count);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public IDisposable Subscribe(IObserver<SortedChangeSet<T>> observer)
    {
        _items.IsChangeCollectionEnabled = true;
        
        return ((_notificationSuspensionCount is 0)
                ? Observable.Empty<Unit>()
                : _notificationsResumed
                    .Take(1))
            .Select(_ => _changeStream
                .Finally(_onChangeStreamFinalized)
                .Prepend(SortedChangeSet.RangeInsertion(
                    index:  0,
                    items:  _items)))
            .Switch()
            .Subscribe(observer);
    }

    /// <inheritdoc/>
    public NotificationSuspension SuspendNotifications()
    {
        ++_notificationSuspensionCount;
        return new(this);
    }

    bool ICollection<T>.IsReadOnly
        => false;

    IEnumerator IEnumerable.GetEnumerator()
        => _items.GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
        => _items.GetEnumerator();

    IDisposable ISubjectList<T>.SuspendNotifications()
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
    /// A value that controls suspension of notifications, for a <see cref="SubjectList{T}"/>. Will trigger notifications to be resumed, when disposed.
    /// </summary>
    public struct NotificationSuspension
        : IDisposable
    {
        private SubjectList<T>? _owner;

        internal NotificationSuspension(SubjectList<T> owner)
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
