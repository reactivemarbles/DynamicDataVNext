namespace DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

public abstract partial class CreateCheckpointTestsBase<TUutAdapter, TChangeSet, TChange, TChangeType>
{
    private readonly struct TestContext
    {
        public static TestContext Create(
            int initialSourceCount,
            int clearingRemovalCount,
            int resettingAdditionCount,
            int followupRemovalCount,
            int followupAdditionCount,
            int checkpointIndex)
        {
            var uut = TUutAdapter.CreateUut(initialSourceCount);
            
            var changes = Enumerable.Empty<TChange>()
                .Concat(Enumerable.Range(1, clearingRemovalCount)
                    .Select(TUutAdapter.CreateRemoval))
                .Concat(Enumerable.Range(clearingRemovalCount + 1, resettingAdditionCount)
                    .Select(TUutAdapter.CreateAddition))
                .Concat(Enumerable.Range(clearingRemovalCount + resettingAdditionCount + 1, followupRemovalCount)
                    .Select(TUutAdapter.CreateRemoval))
                .Concat(Enumerable.Range(clearingRemovalCount + resettingAdditionCount + followupRemovalCount + 1, followupAdditionCount)
                    .Select(TUutAdapter.CreateAddition));
            
            var checkpoint              = null as ChangeSetBuilderBase<TChangeSet, TChange, TChangeType>.Checkpoint?;
            var checkpointChangeCount   = null as int?;
            var checkpointCurrentType   = null as ChangeSetType?;
            var checkpointSourceCount   = null as int?;
            
            foreach (var change in changes)
            {
                if (uut.Changes.Count == checkpointIndex)
                {
                    checkpoint              = uut.CreateCheckpoint();
                    checkpointChangeCount   = uut.Changes.Count;
                    checkpointCurrentType   = uut.CurrentType;
                    checkpointSourceCount   = uut.SourceCount;
                }
                
                uut.AddChange(change);
            }

            return new()
            {
                Checkpoint              = checkpoint            ?? uut.CreateCheckpoint(),
                CheckpointChangeCount   = checkpointChangeCount ?? uut.Changes.Count,
                CheckpointCurrentType   = checkpointCurrentType ?? uut.CurrentType,
                CheckpointSourceCount   = checkpointSourceCount ?? uut.SourceCount,
                Uut                     = uut 
            };
        }
        
        public required ChangeSetBuilderBase<TChangeSet, TChange, TChangeType>.Checkpoint Checkpoint { get; init; }

        public required int CheckpointChangeCount { get; init; }
        
        public required ChangeSetType CheckpointCurrentType { get; init; }

        public required int CheckpointSourceCount { get; init; }

        public required ChangeSetBuilderBase<TChangeSet, TChange, TChangeType> Uut { get; init; }
    }
}
