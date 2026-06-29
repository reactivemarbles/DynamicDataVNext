using System.Linq;

namespace DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

public abstract partial class CreateCheckpointTestsBase<TUutAdapter, TChangeSet, TChange, TChangeType>
{
    private readonly struct TestContext
    {
        public static TestContext Create(
            bool    isSourceEmpty,
            int     clearingRemovalCount,
            int     resettingAdditionCount,
            int     followupRemovalCount,
            int     followupAdditionCount,
            int     checkpointIndex)
        {
            var uut = TUutAdapter.CreateUut(isSourceEmpty);
            
            var addChangeInvocations = Enumerable.Empty<AddChangeInvocation<TChange, TChangeType>>()
                .Concat(Enumerable.Range(1, clearingRemovalCount)
                    .Select(item => new AddChangeInvocation<TChange, TChangeType>()
                    {
                        Change          = TUutAdapter.CreateRemoval(item),
                        IsSourceEmpty   = (item == clearingRemovalCount) 
                    }))
                .Concat(Enumerable.Range(clearingRemovalCount + 1, resettingAdditionCount)
                    .Select(item => new AddChangeInvocation<TChange, TChangeType>()
                    {
                        Change = TUutAdapter.CreateAddition(item)
                    }))
                .Concat(Enumerable.Range(clearingRemovalCount + resettingAdditionCount + 1, followupRemovalCount)
                    .Select(item => new AddChangeInvocation<TChange, TChangeType>()
                    {
                        Change          = TUutAdapter.CreateRemoval(item),
                        IsSourceEmpty   = (item == clearingRemovalCount) 
                    }))
                .Concat(Enumerable.Range(clearingRemovalCount + resettingAdditionCount + followupRemovalCount + 1, followupAdditionCount)
                    .Select(item => new AddChangeInvocation<TChange, TChangeType>()
                    {
                        Change = TUutAdapter.CreateAddition(item)
                    }));
            
            var checkpoint              = null as ChangeSetBuilderBase<TChangeSet, TChange, TChangeType>.Checkpoint?;
            var checkpointChangeCount   = null as int?;
            var checkpointCurrentType   = null as ChangeSetType?;
            var checkpointIsSourceEmpty = null as bool?;
            
            foreach (var invocation in addChangeInvocations)
            {
                if (uut.Changes.Count == checkpointIndex)
                {
                    checkpoint              = uut.CreateCheckpoint();
                    checkpointChangeCount   = uut.Changes.Count;
                    checkpointCurrentType   = uut.CurrentType;
                    checkpointIsSourceEmpty = uut.IsSourceEmpty;
                }
                
                uut.AddChange(
                    change:         invocation.Change,
                    isSourceEmpty:  invocation.IsSourceEmpty);
            }

            return new()
            {
                Checkpoint              = checkpoint                ?? uut.CreateCheckpoint(),
                CheckpointChangeCount   = checkpointChangeCount     ?? uut.Changes.Count,
                CheckpointCurrentType   = checkpointCurrentType     ?? uut.CurrentType,
                CheckpointIsSourceEmpty = checkpointIsSourceEmpty   ?? uut.IsSourceEmpty,
                Uut                     = uut 
            };
        }
        
        public required ChangeSetBuilderBase<TChangeSet, TChange, TChangeType>.Checkpoint Checkpoint { get; init; }

        public required int CheckpointChangeCount { get; init; }
        
        public required ChangeSetType CheckpointCurrentType { get; init; }

        public required bool CheckpointIsSourceEmpty { get; init; }

        public required ChangeSetBuilderBase<TChangeSet, TChange, TChangeType> Uut { get; init; }
    }
}
