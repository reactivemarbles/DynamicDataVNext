using System;
using System.Collections.Generic;

using ReactiveUI.Primitives.Concurrency;
using ReactiveUI.Primitives.Core;

namespace DynamicDataVNext.Tests;

// Using a custom implementation of IObserver<> to bypass normal RX safeguards, allowing us to detect and test for invalid behaviors.
public abstract class RecordingObserverBase<T>
    : IObserver<T>
{
    protected RecordingObserverBase(ISequencer sequencer)
    {
        _recordedNotifications  = new();
        _sequencer              = sequencer;
    }

    public Exception? Error
        => _error;

    public bool HasCompleted
        => _hasCompleted;

    public bool HasFinalized
        => _hasCompleted || (_error is not null);

    public IReadOnlyList<Recorded<Spark<T>>> RecordedNotifications
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
            Time    = _sequencer.Now.Ticks,
            Value   = Spark.CreateOnCompleted<T>()
        });

        _hasCompleted = true;
    }
    
    void IObserver<T>.OnError(Exception error)
    {
        _recordedNotifications.Add(new()
        {
            Time    = _sequencer.Now.Ticks,
            Value   = Spark.CreateOnError<T>(error)
        });

        if (!HasFinalized)
            _error = error;
    }

    void IObserver<T>.OnNext(T value)
    {
        _recordedNotifications.Add(new()
        {
            Time    = _sequencer.Now.Ticks,
            Value   = Spark.CreateOnNext(value)
        });

        OnNext(value);
    }

    private readonly List<Recorded<Spark<T>>>   _recordedNotifications;
    private readonly ISequencer                 _sequencer;

    private Exception?  _error;
    private bool        _hasCompleted;
}
