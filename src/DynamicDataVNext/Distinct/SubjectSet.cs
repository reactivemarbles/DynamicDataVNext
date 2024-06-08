using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace DynamicDataVNext;

/// <summary>
/// The basic implementation of <see cref="ISubjectSet{T}"/>, providing simple collection and change notification functionality, with no concurrency or thread-safety.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
public sealed class SubjectSet<T>
        : ISubjectSet<T>,
            IObservableSet<T>,
            IDisposable
    where T : notnull
{
    private readonly Subject<DistinctChangeSet<T>>  _changeStream;
    private readonly Subject<Unit>                  _collectionChanged;
    private readonly Subject<Unit>                  _notificationsResumed;
    private readonly Action                         _onChangeStreamFinalized;
    private readonly ChangeTrackingSet<T>           _items;

    private int _notificationSuspensionCount;

    /// <summary>
    /// Constructs a new instance of the <see cref="SubjectSet{T}"/> class.
    /// </summary>
    /// <param name="capacity">The initial number of items that the collection should be able to contain, before needing to allocation additional memory.</param>
    /// <param name="comparer">The comparer to be used for detecting item changes, within the collection, or <see langword="null"/> if <see cref="EqualityComparer{T}.Default"/> should be used.</param>
    public SubjectSet(
        int?                    capacity    = null,
        IEqualityComparer<T>?   comparer    = null)
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
    public IObservable<Unit> CollectionChanged
        => _collectionChanged;

    /// <inheritdoc cref="ChangeTrackingSet{T}.Comparer"/>
    public IEqualityComparer<T> Comparer
        => _items.Comparer;

    /// <inheritdoc/>
    public int Count
        => _items.Count;

    /// <inheritdoc/>
    public bool Add(T item)
    {
        var result = _items.Add(item);

        PublishPendingNotifications();

        return result;
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

    /// <inheritdoc cref="ChangeTrackingSet{T}.EnsureCapacity(int)"/>
    public void EnsureCapacity(int capacity)
        => _items.EnsureCapacity(capacity);

    /// <inheritdoc/>
    public void ExceptWith(ReadOnlySpan<T> other)
    {
        _items.ExceptWith(other);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void ExceptWith(IEnumerable<T> other)
    {
        _items.ExceptWith(other);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public HashSet<T>.Enumerator GetEnumerator()
        => _items.GetEnumerator();

    /// <inheritdoc/>
    public void IntersectWith(ReadOnlySpan<T> other)
    {
        _items.IntersectWith(other);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void IntersectWith(IEnumerable<T> other)
    {
        _items.IntersectWith(other);

        PublishPendingNotifications();
    }

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

        PublishPendingNotifications();

        return result;
    }

    /// <inheritdoc/>
    public void Reset(IEnumerable<T> items)
    {
        _items.Reset(items);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void Reset(ReadOnlySpan<T> items)
    {
        _items.Reset(items);

        PublishPendingNotifications();
    }
    /// <inheritdoc/>
    public bool SetEquals(IEnumerable<T> other)
        => _items.SetEquals(other);

    /// <inheritdoc/>
    public IDisposable Subscribe(IObserver<DistinctChangeSet<T>> observer)
    {
        _items.IsChangeCollectionEnabled = true;
        
        return ((_notificationSuspensionCount is 0)
                ? Observable.Empty<Unit>()
                : _notificationsResumed
                    .Take(1))
            .Select(_ => _changeStream
                .Finally(_onChangeStreamFinalized)
                .Prepend(DistinctChangeSet.BulkAddition(_items)))
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
    public void SymmetricExceptWith(ReadOnlySpan<T> other)
    {
        _items.SymmetricExceptWith(other);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void SymmetricExceptWith(IEnumerable<T> other)
    {
        _items.SymmetricExceptWith(other);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void UnionWith(ReadOnlySpan<T> other)
    {
        _items.UnionWith(other);

        PublishPendingNotifications();
    }

    /// <inheritdoc/>
    public void UnionWith(IEnumerable<T> other)
    {
        _items.UnionWith(other);

        PublishPendingNotifications();
    }

    bool ICollection<T>.IsReadOnly
        => false;

    void ICollection<T>.Add(T item)
        => Add(item);

    IEnumerator IEnumerable.GetEnumerator()
        => _items.GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
        => _items.GetEnumerator();

    IDisposable ISubjectSet<T>.SuspendNotifications()
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
    /// A value that controls suspension of notifications, for a <see cref="SubjectSet{T}"/>. Will trigger notifications to be resumed, when disposed.
    /// </summary>
    public struct NotificationSuspension
        : IDisposable
    {
        private SubjectSet<T>? _owner;

        internal NotificationSuspension(SubjectSet<T> owner)
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
