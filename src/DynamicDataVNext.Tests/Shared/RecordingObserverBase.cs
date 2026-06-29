using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;

namespace DynamicDataVNext.Tests;

// Using a custom implementation of IObserver<> to bypass normal RX safeguards, allowing us to detect and test for invalid behaviors.
public abstract class RecordingObserverBase<T>
    : IObserver<T>
{
    protected RecordingObserverBase(IScheduler scheduler)
    {
        _recordedNotifications  = new();
        _scheduler              = scheduler;
    }

    public Exception? Error
        => _error;

    public bool HasCompleted
        => _hasCompleted;

    public bool HasFinalized
        => _hasCompleted || (_error is not null);

    public IReadOnlyList<Recorded<Notification<T>>> RecordedNotifications
        => _recordedNotifications;

    public virtual void ClearNotifications()
    {
        _recordedNotifications.Clear();
        _error = null;
        _hasCompleted = false;
    }

    protected abstract void OnNext(T value);

    void IObserver<T>.OnCompleted()
    {
        _recordedNotifications.Add(new()
        {
            Time    = _scheduler.Now.Ticks,
            Value   = Notification.CreateOnCompleted<T>()
        });

        _hasCompleted = true;
    }
    
    void IObserver<T>.OnError(Exception error)
    {
        _recordedNotifications.Add(new()
        {
            Time    = _scheduler.Now.Ticks,
            Value   = Notification.CreateOnError<T>(error)
        });

        if (!HasFinalized)
            _error = error;
    }

    void IObserver<T>.OnNext(T value)
    {
        _recordedNotifications.Add(new()
        {
            Time    = _scheduler.Now.Ticks,
            Value   = Notification.CreateOnNext(value)
        });

        OnNext(value);
    }

    private readonly List<Recorded<Notification<T>>>    _recordedNotifications;
    private readonly IScheduler                         _scheduler;

    private Exception?  _error;
    private bool        _hasCompleted;
}
