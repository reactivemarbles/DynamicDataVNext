using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

[TestFixture]
public class ClearTestsBase<TUutAdapter, TChangeSet, TChange, TChangeType>
        : Base
    where TUutAdapter : IUutAdapter<TChangeSet, TChange, TChangeType>, new()
    where TChangeSet : struct, IChangeSet<TChange, TChangeType>
    where TChange : struct, IChange<TChangeType>
    where TChangeType : Enum
{
    [TestCaseSource(nameof(Base.IsSourceEmpty_TestCases))]
    public void WhenNoPendingChanges_BuilderIsReset(bool isSourceEmpty)
        => Always_BuilderIsReset(
            isSourceInitiallyEmpty: !isSourceEmpty,
            addChangeInvocations:   Array.Empty<AddChangeInvocation<TChange, TChangeType>>(),
            isSourceEmpty:          isSourceEmpty);
    
    [TestCaseSource(nameof(Base.IsSourceEmpty_TestCases))]
    public void WhenPendingChangesAreClear_BuilderIsReset(bool isSourceEmpty)
        => Always_BuilderIsReset(
            isSourceInitiallyEmpty: false,
            addChangeInvocations:   new AddChangeInvocation<TChange, TChangeType>[]
            {
                new() { Change = TUutAdapter.CreateRemoval(1), IsSourceEmpty = true }
            },
            isSourceEmpty:          isSourceEmpty);
    
    [TestCaseSource(nameof(Base.IsSourceEmpty_TestCases))]
    public void WhenPendingChangesAreReset_BuilderIsReset(bool isSourceEmpty)
        => Always_BuilderIsReset(
            isSourceInitiallyEmpty: true,
            addChangeInvocations:   new AddChangeInvocation<TChange, TChangeType>[]
            {
                new() { Change = TUutAdapter.CreateAddition(1), IsSourceEmpty = false }
            },
            isSourceEmpty:          isSourceEmpty);
    
    [TestCaseSource(nameof(Base.IsSourceEmpty_TestCases))]
    public void WhenPendingChangesAreUpdate_BuilderIsReset(bool isSourceEmpty)
        => Always_BuilderIsReset(
            isSourceInitiallyEmpty: false,
            addChangeInvocations:   new AddChangeInvocation<TChange, TChangeType>[]
            {
                new() { Change = TUutAdapter.CreateAddition(1), IsSourceEmpty = false},
                new() { Change = TUutAdapter.CreateRemoval(2),  IsSourceEmpty = false}
            },
            isSourceEmpty:          isSourceEmpty);

    private static void Always_BuilderIsReset(
        bool                                                        isSourceInitiallyEmpty,
        IReadOnlyList<AddChangeInvocation<TChange, TChangeType>>    addChangeInvocations,
        bool                                                        isSourceEmpty)
    {
        var uut = TUutAdapter.CreateUut(isSourceEmpty: isSourceInitiallyEmpty);
        
        foreach (var invocation in addChangeInvocations)
            uut.AddChange(
                change:         invocation.Change,
                isSourceEmpty:  invocation.IsSourceEmpty);

        uut.Clear(isSourceEmpty: isSourceEmpty);

        uut.Changes.Count.Should().Be(0, "all pending changes should be removed");
        uut.CurrentType.Should().Be(ChangeSetType.Empty, "there are no pending changes");
        uut.IsSourceEmpty.Should().Be(isSourceEmpty, "the state of the source collection should be reset");
    }
}
