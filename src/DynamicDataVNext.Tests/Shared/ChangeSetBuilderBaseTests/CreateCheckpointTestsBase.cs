namespace DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

public abstract partial class CreateCheckpointTestsBase<TUutAdapter, TChangeSet, TChange, TChangeType>
    where TUutAdapter : IUutAdapter<TChangeSet, TChange, TChangeType>, new()
    where TChangeSet : struct, IChangeSet<TChange, TChangeType>
    where TChange : struct, IChange<TChangeType>
    where TChangeType : Enum
{
    [Test]
    public void WhenAlreadyRestored_RestoreThrowsException()
    {
        var uut = TUutAdapter.CreateUut(sourceCount: 0);
        
        var checkpoint = uut.CreateCheckpoint();

        uut.AddChange(TUutAdapter.CreateAddition(1));
        uut.AddChange(TUutAdapter.CreateAddition(2));
        uut.AddChange(TUutAdapter.CreateAddition(3));

        checkpoint.Restore();

        var priorChanges        = uut.Changes.ToArray();
        var priorCurrentType    = uut.CurrentType;
        var priorSourceCount    = uut.SourceCount;

        var result = checkpoint.Invoking(static checkpoint => checkpoint.Restore())
            .Should().Throw<InvalidOperationException>("a checkpoint can only be restored once")
            .Which;
        
        Console.WriteLine(result);

        uut.Changes.Should().BeEquivalentTo(priorChanges, static options => options.WithStrictOrdering(), "a rejected restoration should not affect the builder state");
        uut.CurrentType.Should().Be(priorCurrentType, "a rejected restoration should not affect the builder state");
        uut.SourceCount.Should().Be(priorSourceCount, "a rejected restoration should not affect the builder state");
    }

    [Test]
    public void WhenAnotherCheckpointHasBeenRestored_RestoreThrowsException()
    {
        var uut = TUutAdapter.CreateUut(sourceCount: 0);
        
        var checkpoint = uut.CreateCheckpoint();

        uut.AddChange(TUutAdapter.CreateAddition(1));
        uut.AddChange(TUutAdapter.CreateAddition(2));
        uut.AddChange(TUutAdapter.CreateAddition(3));
        var otherCheckpoint = uut.CreateCheckpoint();
        uut.AddChange(TUutAdapter.CreateAddition(4));
        uut.AddChange(TUutAdapter.CreateAddition(5));
        uut.AddChange(TUutAdapter.CreateAddition(6));
        otherCheckpoint.Restore();

        var priorChanges        = uut.Changes.ToArray();
        var priorCurrentType    = uut.CurrentType;
        var priorSourceCount    = uut.SourceCount;

        var result = checkpoint.Invoking(static checkpoint => checkpoint.Restore())
            .Should().Throw<InvalidOperationException>("restoring a checkpoint invalidates all other checkpoints")
            .Which;

        Console.WriteLine(result);

        uut.Changes.Should().BeEquivalentTo(priorChanges, static options => options.WithStrictOrdering(), "a rejected restoration should not affect the builder state");
        uut.CurrentType.Should().Be(priorCurrentType, "a rejected restoration should not affect the builder state");
        uut.SourceCount.Should().Be(priorSourceCount, "a rejected restoration should not affect the builder state");
    }
    
    [TestCase(0, 0, 1, 1, 0, 0, TestName = "{m}(Checkpoint before changes, Source is empty)")]
    [TestCase(1, 0, 0, 1, 1, 0, TestName = "{m}(Checkpoint before changes, Source is not empty)")]
    [TestCase(2, 2, 0, 0, 0, 1, TestName = "{m}(Checkpoint during pending Clear)")]
    [TestCase(0, 0, 2, 0, 0, 1, TestName = "{m}(Checkpoint during pending Reset)")]
    [TestCase(3, 0, 0, 2, 2, 2, TestName = "{m}(Checkpoint during pending Update)")]
    public void WhenBuilderHasBeenCleared_RestoreThrowsException(
        int initialSourceCount,
        int clearingRemovalCount,
        int resettingAdditionCount,
        int followupRemovalCount,
        int followupAdditionCount,
        int checkpointIndex)
    {
        var testContext = TestContext.Create(
            initialSourceCount:     initialSourceCount,
            clearingRemovalCount:   clearingRemovalCount,
            resettingAdditionCount: resettingAdditionCount,
            followupRemovalCount:   followupRemovalCount,
            followupAdditionCount:  followupAdditionCount,
            checkpointIndex:        clearingRemovalCount + resettingAdditionCount + followupRemovalCount + followupAdditionCount);
        
        testContext.Uut.Clear(initialSourceCount);
        
        var result = testContext.Checkpoint.Invoking(static checkpoint => checkpoint.Restore())
            .Should().Throw<InvalidOperationException>("a checkpoint cannot be used to restore removed changes")
            .Which;

        testContext.Uut.Changes.Count.Should().Be(0, "a rejected restoration should not affect the builder state");
        testContext.Uut.CurrentType.Should().Be(ChangeSetType.Empty, "a rejected restoration should not affect the builder state");
        testContext.Uut.SourceCount.Should().Be(initialSourceCount, "a rejected restoration should not affect the builder state");

        Console.WriteLine(result);
    }
    
    [TestCase(0, 0, 0, 0, 0, TestName=  "{m}(No pending changes, Source is empty)")]
    [TestCase(1, 0, 0, 0, 0, TestName=  "{m}(No pending changes, Source is not empty)")]
    [TestCase(2, 1, 0, 0, 0, TestName=  "{m}(Pending Clear)")]
    [TestCase(0, 0, 1, 0, 0, TestName=  "{m}(Pending Reset)")]
    [TestCase(3, 0, 0, 1, 1, TestName=  "{m}(Pending Update)")]
    public void WhenNoChangesHaveBeenAdded_RestoreDoesNothing(
        int initialSourceCount,
        int clearingRemovalCount,
        int resettingAdditionCount,
        int followupRemovalCount,
        int followupAdditionCount)
    {
        var testContext = TestContext.Create(
            initialSourceCount:     initialSourceCount,
            clearingRemovalCount:   clearingRemovalCount,
            resettingAdditionCount: resettingAdditionCount,
            followupRemovalCount:   followupRemovalCount,
            followupAdditionCount:  followupAdditionCount,
            checkpointIndex:        clearingRemovalCount + resettingAdditionCount + followupRemovalCount + followupAdditionCount);
        
        testContext.Checkpoint.Restore();
        
        testContext.Uut.Changes.Count.Should().Be(testContext.CheckpointChangeCount, "no changes were added after the checkpoint");
        testContext.Uut.CurrentType.Should().Be(testContext.CheckpointCurrentType, "no changes were added after the checkpoint");
        testContext.Uut.SourceCount.Should().Be(testContext.CheckpointSourceCount, "no changes were added after the checkpoint");

        testContext.Checkpoint.Invoking(static checkpoint => checkpoint.Restore())
            .Should().NotThrow("no changes were added after the checkpoint");
    }
    
    [TestCase(0, 0, 1, 1, 0, 0, TestName = "{m}(Checkpoint before changes, Source is empty)")]
    [TestCase(1, 0, 0, 1, 1, 0, TestName = "{m}(Checkpoint before changes, Source is not empty)")]
    [TestCase(2, 2, 0, 0, 0, 1, TestName = "{m}(Checkpoint during pending Clear)")]
    [TestCase(0, 0, 2, 0, 0, 1, TestName = "{m}(Checkpoint during pending Reset)")]
    [TestCase(3, 0, 0, 2, 2, 2, TestName = "{m}(Checkpoint during pending Update)")]
    public void Otherwise_RestoreRestoresBuilder(
        int initialSourceCount,
        int clearingRemovalCount,
        int resettingAdditionCount,
        int followupRemovalCount,
        int followupAdditionCount,
        int checkpointIndex)
    {
        var testContext = TestContext.Create(
            initialSourceCount:     initialSourceCount,
            clearingRemovalCount:   clearingRemovalCount,
            resettingAdditionCount: resettingAdditionCount,
            followupRemovalCount:   followupRemovalCount,
            followupAdditionCount:  followupAdditionCount,
            checkpointIndex:        clearingRemovalCount + resettingAdditionCount + followupRemovalCount + followupAdditionCount);
        
        testContext.Checkpoint.Restore();
        
        testContext.Uut.Changes.Count.Should().Be(testContext.CheckpointChangeCount, "a checkpoint was restored");
        testContext.Uut.CurrentType.Should().Be(testContext.CheckpointCurrentType, "a checkpoint was restored");
        testContext.Uut.SourceCount.Should().Be(testContext.CheckpointSourceCount, "a checkpoint was restored");
    }
}
