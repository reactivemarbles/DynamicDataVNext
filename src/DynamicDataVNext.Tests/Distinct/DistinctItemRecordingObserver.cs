using System.Collections.Generic;

using ReactiveUI.Primitives.Concurrency;

namespace DynamicDataVNext.Tests.Distinct;

public sealed class DistinctItemRecordingObserver<T>
    : RecordingObserverBase<DistinctChangeSet<T>>
{
    public DistinctItemRecordingObserver(
            ISequencer              sequencer,
            IEqualityComparer<T>?   comparer    = null)
        : base(sequencer)
    {
        _recordedChangeSets = new();
        _recordedItems      = new(comparer: comparer);
        _refreshedItems     = new();
    }        

    public IReadOnlyList<DistinctChangeSet<T>> RecordedChangeSets
        => _recordedChangeSets;

    public IReadOnlySet<T> RecordedItems
        => _recordedItems;
        
    public IReadOnlySet<T> RefreshedItems
        => _refreshedItems;

    public override void ClearNotifications()
    {
        base.ClearNotifications();
        
        _recordedChangeSets.Clear();
    }
    
    public void ClearRefreshedItems()
        => _refreshedItems.Clear();

    protected override void OnNext(DistinctChangeSet<T> value)
    {
        if (HasFinalized)
            return;

        _recordedChangeSets.Add(value);

        value.ApplyTo(_recordedItems);
        
        if (value.Type is not ChangeSetType.Update)
            return;

        foreach (var change in value.Changes)
            if (change.Type is DistinctChangeType.Refreshment)
                _refreshedItems.Add(change.Item);
    }

    private readonly List<DistinctChangeSet<T>> _recordedChangeSets;
    private readonly HashSet<T>                 _recordedItems;
    private readonly HashSet<T>                 _refreshedItems;
}
