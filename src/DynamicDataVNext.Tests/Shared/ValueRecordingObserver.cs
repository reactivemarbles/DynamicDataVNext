using System.Collections.Generic;

using ReactiveUI.Primitives.Concurrency;

namespace DynamicDataVNext.Tests;

public sealed class ValueRecordingObserver<T>
    : RecordingObserverBase<T>
{
    private readonly List<T> _recordedValues;

    public ValueRecordingObserver(ISequencer sequencer)
            : base(sequencer)
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
