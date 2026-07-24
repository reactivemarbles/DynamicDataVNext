using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace DynamicDataVNext;


/// <summary>
/// Describes a collection of distinct items, which publishes notifications about mutations made to itself or its items.
/// </summary>
/// <typeparam name="T">The type of the items in the collection.</typeparam>
[DebuggerDisplay("Count = {Count}")]
public sealed partial class ObservableHashSet<T>
    : IDisposable,
        IObservableSet<T>,
        IObservableReadOnlySet<T>,
        IExpandableCollection
{
    /// <summary>
    /// Initializes a new empty instance of the <see cref="ObservableHashSet{T}"/> class. 
    /// </summary>
    /// <inheritdoc cref="ChangeTrackingHashSet{T}(IEqualityComparer{T}, DistinctItemOptions)"/>
    public ObservableHashSet(
            IEqualityComparer<T>?   comparer    = null,
            DistinctItemOptions     options     = default) 
        : this(new(
            comparer:   comparer,
            options:    options))
    { }

    /// <inheritdoc cref="ObservableHashSet{T}(System.Collections.Generic.IEqualityComparer{T}, DistinctItemOptions)"/>
    /// <param name="capacity">The initial value to use for <see cref="Capacity"/>.</param>
    public ObservableHashSet(
            int                     capacity,
            IEqualityComparer<T>?   comparer    = null,
            DistinctItemOptions     options     = default)
        : this(new(
            capacity:   capacity,
            comparer:   comparer,
            options:    options))
    { }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ObservableHashSet{T}"/> class, containing the given items. 
    /// </summary>
    /// <inheritdoc cref="ObservableHashSet{T}(System.Collections.Generic.IEqualityComparer{T}, DistinctItemOptions)"/>
    /// <param name="items">The initial set of items to be loaded into the collection. Duplicate items are ignored.</param>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="items"/>.</exception>
    public ObservableHashSet(
            IEnumerable<T>          items,
            IEqualityComparer<T>?   comparer    = null,
            DistinctItemOptions     options     = default)
        : this(new(
            items:      items ?? throw new ArgumentNullException(nameof(items)),
            comparer:   comparer,
            options:    options))
    { }
    
    private ObservableHashSet(ChangeTrackingHashSet<T> items)
    {
        _areNotificationsSuspended  = new(false);
        _collectionChanged          = new();
        _collectionChangesCaptured  = new();
        _items                      = items;

        _changeStream = new()
        {
            Comparer    = items.Comparer,
            Options     = items.Options,
            Source      = _areNotificationsSuspended
                .SkipWhile(areNotificationsSuspended => areNotificationsSuspended)
                .Take(1)
                .Select(_ => (_items.Count is not 0)
                    ? _collectionChangesCaptured
                        .Prepend(DistinctChangeSet.CreateForReset(addedItems: _items))
                    : _collectionChangesCaptured)
                .Switch()
        };
    }            

    /// <inheritdoc cref="IObservableCollection{T}.CollectionChanged"/>
    public IObservable<Unit> CollectionChanged
        => _collectionChanged;

    /// <inheritdoc/>
    public int Capacity
        => _items.Capacity;
    
    /// <inheritdoc cref="IObservableSet{T}.ChangeStream"/>
    public DistinctChangeStream<T> ChangeStream
        => _changeStream;

    /// <inheritdoc cref="ICollection{T}.Count"/>
    public int Count
        => _items.Count;

    /// <summary>
    /// A flag indicating whether the collection can currently be mutated.
    /// </summary>
    /// <remarks>
    /// <see langword="true"/> after <see cref="Dispose"/> has been called. <see langword="false"/> otherwise.
    /// </remarks>
    public bool IsReadOnly
        => _hasDisposed;

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"></exception>
    public bool Add(T item)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        var result = _items.Add(item);

        PublishNotificationsIfNeeded();

        return result;
    }

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"></exception>
    public void Clear()
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        _items.Clear();

        PublishNotificationsIfNeeded();
    }

    /// <inheritdoc cref="ICollection{T}.Contains"/>
    public bool Contains(T item)
        => _items.Contains(item);

    /// <inheritdoc cref="ICollection{T}.CopyTo"/>
    public void CopyTo(T[] array, int arrayIndex)
        => _items.CopyTo(array, arrayIndex);

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_hasDisposed)
            return;
        _hasDisposed = true;

        _areNotificationsSuspended  .OnCompleted();
        _collectionChanged          .OnCompleted();
        _collectionChangesCaptured  .OnCompleted();

        _areNotificationsSuspended  .Dispose();
        _collectionChangesCaptured  .Dispose();
        _collectionChanged          .Dispose();
    }

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"></exception>
    public void EnsureCapacity(int capacity)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        _items.EnsureCapacity(capacity);
    }
    
    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"></exception>
    public void ExceptWith(IEnumerable<T> other)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        _items.ExceptWith(other);

        PublishNotificationsIfNeeded();
    }

    /// <inheritdoc cref="ChangeTrackingHashSet{T}.GetEnumerator()"/>
    public HashSet<T>.Enumerator GetEnumerator()
        => _items.GetEnumerator();

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"></exception>
    public void IntersectWith(IEnumerable<T> other)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        _items.IntersectWith(other);

        PublishNotificationsIfNeeded();
    }

    /// <inheritdoc cref="ISet{T}.IsProperSubsetOf"/>
    public bool IsProperSubsetOf(IEnumerable<T> other)
        => _items.IsProperSubsetOf(other);

    /// <inheritdoc cref="ISet{T}.IsProperSupersetOf"/>
    public bool IsProperSupersetOf(IEnumerable<T> other)
        => _items.IsProperSupersetOf(other);

    /// <inheritdoc cref="ISet{T}.IsSubsetOf"/>
    public bool IsSubsetOf(IEnumerable<T> other)
        => _items.IsSubsetOf(other);

    /// <inheritdoc cref="ISet{T}.IsSupersetOf"/>
    public bool IsSupersetOf(IEnumerable<T> other)
        => _items.IsSupersetOf(other);

    /// <inheritdoc cref="ISet{T}.Overlaps"/>
    public bool Overlaps(IEnumerable<T> other)
        => _items.Overlaps(other);

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"></exception>
    public bool Refresh(T item)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        var result = _items.Refresh(item);

        PublishNotificationsIfNeeded();

        return result;
    }

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"></exception>
    public bool Remove(T item)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        var result = _items.Remove(item);

        PublishNotificationsIfNeeded();

        return result;
    }

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"></exception>
    public void Reset(IEnumerable<T> items)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        _items.Reset(items);

        PublishNotificationsIfNeeded();
    }

    /// <inheritdoc cref="ISet{T}.SetEquals"/>
    public bool SetEquals(IEnumerable<T> other)
        => _items.SetEquals(other);

    /// <inheritdoc cref="IObservableCollection{T}.SuspendNotifications"/>
    /// <exception cref="ObjectDisposedException"></exception>
    public Suspension SuspendNotifications()
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        if (_areNotificationsSuspended.Value)
            throw new InvalidOperationException("Notifications are already suspended");
        _areNotificationsSuspended.OnNext(true);

        return new(this);
    }

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"></exception>
    public void SymmetricExceptWith(IEnumerable<T> other)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        _items.SymmetricExceptWith(other);

        PublishNotificationsIfNeeded();
    }

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"></exception>
    public void UnionWith(IEnumerable<T> other)
    {
        ObjectDisposedException.ThrowIf(_hasDisposed, GetType());

        _items.UnionWith(other);
        
        PublishNotificationsIfNeeded();
    }

    void ICollection<T>.Add(T item)
        => Add(item);

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
        => _items.GetEnumerator();

    IDisposable IObservableCollection<T>.SuspendNotifications()
        => SuspendNotifications();

    private void PublishNotificationsIfNeeded()
    {
        if (_areNotificationsSuspended.Value)
            return;

        var changes = _items.BufferedChanges.CaptureAndClear();
        if (changes.Type is ChangeSetType.Empty)
            return;

        _collectionChangesCaptured.OnNext(changes);
        _collectionChanged.OnNext(Unit.Default);
    }

    private readonly BehaviorSubject<bool>          _areNotificationsSuspended;
    private readonly DistinctChangeStream<T>        _changeStream;
    private readonly Subject<Unit>                  _collectionChanged;
    private readonly Subject<DistinctChangeSet<T>>  _collectionChangesCaptured;
    private readonly ChangeTrackingHashSet<T>       _items;

    private bool _hasDisposed;
}
