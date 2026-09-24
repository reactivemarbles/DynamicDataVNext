namespace DynamicDataVNext.Tests.Keyed;

public sealed class KeyedItemRecordingObserver<TKey, TItem>
        : RecordingObserverBase<KeyedChangeSet<TKey, TItem>>
    where TKey : notnull
{
    public KeyedItemRecordingObserver(
            ISequencer                  sequencer,
            IEqualityComparer<TKey>?    keyComparer = null)
        : base(sequencer)
    {
        _recordedChangeSets = new();
        _recordedItems      = new(comparer: keyComparer);
        _refreshedKeys      = new();
    }        

    public IReadOnlyList<KeyedChangeSet<TKey, TItem>> RecordedChangeSets
        => _recordedChangeSets;

    public IReadOnlyDictionary<TKey, TItem> RecordedItems
        => _recordedItems;
        
    public IReadOnlySet<TKey> RefreshedKeys
        => _refreshedKeys;

    public override void ClearNotifications()
    {
        base.ClearNotifications();
        
        _recordedChangeSets.Clear();
    }
    
    public void ClearRefreshedKeys()
        => _refreshedKeys.Clear();

    protected override void OnNext(KeyedChangeSet<TKey, TItem> value)
    {
        if (HasFinalized)
            return;

        _recordedChangeSets.Add(value);

        value.ApplyTo(_recordedItems);
        
        if (value.Type is not ChangeSetType.Update)
            return;

        foreach (var change in value.Changes)
            if (change.Type is KeyedChangeType.Refreshment)
                _refreshedKeys.Add(change.AsRefreshment().Key);
    }

    private readonly List<KeyedChangeSet<TKey, TItem>>  _recordedChangeSets;
    private readonly Dictionary<TKey, TItem>            _recordedItems;
    private readonly HashSet<TKey>                      _refreshedKeys;
}
