using System.Collections.Generic;
using System.Reactive.Concurrency;

namespace DynamicDataVNext.Tests;

public sealed class ValueRecordingObserver<T>
    : RecordingObserverBase<T>
{
    private readonly List<T> _recordedValues;

    public ValueRecordingObserver(IScheduler scheduler)
            : base(scheduler)
        => _recordedValues = new();

    public IReadOnlyList<T> RecordedValues
        => _recordedValues;

    public override void ClearNotifications()
    {
        base.ClearNotifications();
        
        _recordedValues.Clear();
    }

    protected override void OnNext(T value)
    {
        if (!HasFinalized)
            _recordedValues.Add(value);
    }
}
