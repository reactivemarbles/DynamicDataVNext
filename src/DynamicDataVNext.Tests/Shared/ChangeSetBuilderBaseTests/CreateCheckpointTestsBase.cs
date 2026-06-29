using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

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
        var uut = TUutAdapter.CreateUut(isSourceEmpty: true);
        
        var checkpoint = uut.CreateCheckpoint();
        
        uut.AddChange(TUutAdapter.CreateAddition(1));
        uut.AddChange(TUutAdapter.CreateAddition(2));
        uut.AddChange(TUutAdapter.CreateAddition(3));

        checkpoint.Restore();
        
        var priorChangeCount    = uut.Changes.Count;
        var priorCurrentType    = uut.CurrentType;
        var priorIsSourceEmpty  = uut.IsSourceEmpty;

        var result = checkpoint.Invoking(static checkpoint => checkpoint.Restore())
            .Should().Throw<InvalidOperationException>("a checkpoint can only be restored once")
            .Which;
        
        uut.Changes.Count.Should().Be(priorChangeCount, "a rejected restoration should not affect the builder state");
        uut.CurrentType.Should().Be(priorCurrentType, "a rejected restoration should not affect the builder state");
        uut.IsSourceEmpty.Should().Be(priorIsSourceEmpty, "a rejected restoration should not affect the builder state");

        Console.WriteLine(result);
    }

    [Test]
    public void WhenAnotherCheckpointHasBeenRestored_RestoreThrowsException()
    {
        var uut = TUutAdapter.CreateUut(isSourceEmpty: true);
        
        var checkpoint = uut.CreateCheckpoint();
        
        uut.AddChange(TUutAdapter.CreateAddition(1));
        uut.AddChange(TUutAdapter.CreateAddition(2));
        uut.AddChange(TUutAdapter.CreateAddition(3));

        var otherCheckpoint = uut.CreateCheckpoint();
        
        uut.AddChange(TUutAdapter.CreateAddition(4));
        uut.AddChange(TUutAdapter.CreateAddition(5));
        uut.AddChange(TUutAdapter.CreateAddition(6));

        otherCheckpoint.Restore();

        var priorChangeCount    = uut.Changes.Count;
        var priorCurrentType    = uut.CurrentType;
        var priorIsSourceEmpty  = uut.IsSourceEmpty;

        var result = checkpoint.Invoking(static checkpoint => checkpoint.Restore())
            .Should().Throw<InvalidOperationException>("restoring a checkpoint invalidates all other checkpoints")
            .Which;

        uut.Changes.Count.Should().Be(priorChangeCount, "a rejected restoration should not affect the builder state");
        uut.CurrentType.Should().Be(priorCurrentType, "a rejected restoration should not affect the builder state");
        uut.IsSourceEmpty.Should().Be(priorIsSourceEmpty, "a rejected restoration should not affect the builder state");

        Console.WriteLine(result);
    }
    
    public static readonly IReadOnlyList<TestCaseData> WhenBuilderHasBeenCleared_TestCases
        = new[]
        {
            new TestCaseData(true,  0,  1,  1,  0,  0).SetName("{m}(Checkpoint before changes, Source is empty)"),
            new TestCaseData(false, 0,  0,  1,  1,  0).SetName("{m}(Checkpoint before changes, Source is not empty)"),
            new TestCaseData(false, 2,  0,  0,  0,  1).SetName("{m}(Checkpoint during pending Clear)"),
            new TestCaseData(true,  0,  2,  0,  0,  1).SetName("{m}(Checkpoint during pending Reset)"),
            new TestCaseData(false, 0,  0,  2,  2,  2).SetName("{m}(Checkpoint during pending Update)")
        };
    [TestCaseSource(nameof(WhenBuilderHasBeenCleared_TestCases))]
    public void WhenBuilderHasBeenCleared_RestoreThrowsException(
        bool    isSourceEmpty,
        int     clearingRemovalCount,
        int     resettingAdditionCount,
        int     followupRemovalCount,
        int     followupAdditionCount,
        int     checkpointIndex)
    {
        var testContext = TestContext.Create(
            isSourceEmpty:          isSourceEmpty,
            clearingRemovalCount:   clearingRemovalCount,
            resettingAdditionCount: resettingAdditionCount,
            followupRemovalCount:   followupRemovalCount,
            followupAdditionCount:  followupAdditionCount,
            checkpointIndex:        clearingRemovalCount + resettingAdditionCount + followupRemovalCount + followupAdditionCount);
        
        testContext.Uut.Clear(isSourceEmpty: isSourceEmpty);
        
        var result = testContext.Checkpoint.Invoking(static checkpoint => checkpoint.Restore())
            .Should().Throw<InvalidOperationException>("a checkpoint cannot be used to restore removed changes")
            .Which;

        testContext.Uut.Changes.Count.Should().Be(0, "a rejected restoration should not affect the builder state");
        testContext.Uut.CurrentType.Should().Be(ChangeSetType.Empty, "a rejected restoration should not affect the builder state");
        testContext.Uut.IsSourceEmpty.Should().Be(isSourceEmpty, "a rejected restoration should not affect the builder state");

        Console.WriteLine(result);
    }
    
    public static readonly IReadOnlyList<TestCaseData> WhenNoChangesHaveBeenAdded_TestCases
        = new[]
        {
            new TestCaseData(true,  0,  0,  0,  0).SetName("{m}(No pending changes, Source is empty)"),
            new TestCaseData(false, 0,  0,  0,  0).SetName("{m}(No pending changes, Source is not empty)"),
            new TestCaseData(false, 1,  0,  0,  0).SetName("{m}(Pending Clear)"),
            new TestCaseData(true,  0,  1,  0,  0).SetName("{m}(Pending Reset)"),
            new TestCaseData(false, 0,  0,  1,  1).SetName("{m}(Pending Update)")
        };
    [TestCaseSource(nameof(WhenNoChangesHaveBeenAdded_TestCases))]
    public void WhenNoChangesHaveBeenAdded_RestoreDoesNothing(
        bool    isSourceEmpty,
        int     clearingRemovalCount,
        int     resettingAdditionCount,
        int     followupRemovalCount,
        int     followupAdditionCount)
    {
        var testContext = TestContext.Create(
            isSourceEmpty:          isSourceEmpty,
            clearingRemovalCount:   clearingRemovalCount,
            resettingAdditionCount: resettingAdditionCount,
            followupRemovalCount:   followupRemovalCount,
            followupAdditionCount:  followupAdditionCount,
            checkpointIndex:        clearingRemovalCount + resettingAdditionCount + followupRemovalCount + followupAdditionCount);
        
        testContext.Checkpoint.Restore();
        
        testContext.Uut.Changes.Count.Should().Be(testContext.CheckpointChangeCount, "no changes were added after the checkpoint");
        testContext.Uut.CurrentType.Should().Be(testContext.CheckpointCurrentType, "no changes were added after the checkpoint");
        testContext.Uut.IsSourceEmpty.Should().Be(testContext.CheckpointIsSourceEmpty, "no changes were added after the checkpoint");

        testContext.Checkpoint.Invoking(static checkpoint => checkpoint.Restore())
            .Should().NotThrow("no changes were added after the checkpoint");
    }
    
    public static readonly IReadOnlyList<TestCaseData> Otherwise_TestCases
        = new[]
        {
            new TestCaseData(true,  0,  1,  1,  0,  0).SetName("{m}(Checkpoint before changes, Source is empty)"),
            new TestCaseData(false, 0,  0,  1,  1,  0).SetName("{m}(Checkpoint before changes, Source is not empty)"),
            new TestCaseData(false, 2,  0,  0,  0,  1).SetName("{m}(Checkpoint during pending Clear)"),
            new TestCaseData(true,  0,  2,  0,  0,  1).SetName("{m}(Checkpoint during pending Reset)"),
            new TestCaseData(false, 0,  0,  2,  2,  2).SetName("{m}(Checkpoint during pending Update)")
        };
    [TestCaseSource(nameof(Otherwise_TestCases))]
    public void Otherwise_RestoreRestoresBuilder(
        bool    isSourceEmpty,
        int     clearingRemovalCount,
        int     resettingAdditionCount,
        int     followupRemovalCount,
        int     followupAdditionCount,
        int     checkpointIndex)
    {
        var testContext = TestContext.Create(
            isSourceEmpty:          isSourceEmpty,
            clearingRemovalCount:   clearingRemovalCount,
            resettingAdditionCount: resettingAdditionCount,
            followupRemovalCount:   followupRemovalCount,
            followupAdditionCount:  followupAdditionCount,
            checkpointIndex:        clearingRemovalCount + resettingAdditionCount + followupRemovalCount + followupAdditionCount);
        
        testContext.Checkpoint.Restore();
        
        testContext.Uut.Changes.Count.Should().Be(testContext.CheckpointChangeCount, "a checkpoint was restored");
        testContext.Uut.CurrentType.Should().Be(testContext.CheckpointCurrentType, "a checkpoint was restored");
        testContext.Uut.IsSourceEmpty.Should().Be(testContext.CheckpointIsSourceEmpty, "a checkpoint was restored");
    }
}
