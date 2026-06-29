using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

public abstract class AddChangeTestsBase<TUutAdapter, TChangeSet, TChange, TChangeType>
    where TUutAdapter : IUutAdapter<TChangeSet, TChange, TChangeType>, new()
    where TChangeSet : struct, IChangeSet<TChange, TChangeType>
    where TChange : struct, IChange<TChangeType>
    where TChangeType : Enum
{
    [Test]
    public void WhenChangeIsAdditionAndEmptiesSource_ThrowsException()
        => WhenChangeIsInvalidOrIncoherent_ThrowsException(
            isSourceEmpty:      false,
            priorInvocations:   Array.Empty<AddChangeInvocation<TChange, TChangeType>>(),
            invocation:         new()
            {
                Change          = TUutAdapter.CreateAddition(1),
                IsSourceEmpty   = true
            },
            because:            "a collection cannot be emptied by adding an item");
    
    public static readonly IReadOnlyList<TestCaseData> WhenChangeIsNone_TestCases
        = new[]
        {
            new TestCaseData(0, false,  false)  .SetName("{m}(No pending changes)"),
            new TestCaseData(0, true,   false)  .SetName("{m}(Source is initially empty)"),
            new TestCaseData(0, false,  true)   .SetName("{m}(Change empties source)"),
            new TestCaseData(1, false,  false)  .SetName("{m}(Single pending change)"),
            new TestCaseData(1, true,   false)  .SetName("{m}(Source is previously emptied)"),
            new TestCaseData(5, false,  false)  .SetName("{m}(Multiple pending changes)")
        };
    [TestCaseSource(nameof(WhenChangeIsNone_TestCases))]
    public void WhenChangeIsNone_ThrowsException(
            int     priorChangeCount,
            bool    isSourcePreviouslyEmpty,
            bool    isSourceEmpty)
        => WhenChangeIsInvalidOrIncoherent_ThrowsException(
            isSourceEmpty:      (priorChangeCount is 0) && isSourcePreviouslyEmpty,
            priorInvocations:   Enumerable.Range(1, priorChangeCount)
                .Select(item => new AddChangeInvocation<TChange, TChangeType>()
                {
                    Change          = TUutAdapter.CreateRemoval(item),
                    IsSourceEmpty   = (item == priorChangeCount) && isSourcePreviouslyEmpty
                })
                .ToArray(),
            invocation:         new()
            {
                Change          = TUutAdapter.CreateNone(),
                IsSourceEmpty   = isSourceEmpty
            },
            because:            "changes of type None are not supported");

    public static readonly IReadOnlyList<TestCaseData> WhenChangeIsRemovalAndSourceIsEmpty_TestCases
        = new[]
        {
            new TestCaseData(0).SetName("{m}(No pending changes)"),
            new TestCaseData(1).SetName("{m}(Single pending change)"),
            new TestCaseData(5).SetName("{m}(Multiple pending changes)")
        };
    [TestCaseSource(nameof(WhenChangeIsRemovalAndSourceIsEmpty_TestCases))]
    public void WhenChangeIsRemovalAndSourceIsEmpty_ThrowsException(int priorChangeCount)
        => WhenChangeIsInvalidOrIncoherent_ThrowsException(
            isSourceEmpty:      (priorChangeCount is 0),
            priorInvocations:   Enumerable.Range(1, priorChangeCount)
                .Select(item => new AddChangeInvocation<TChange, TChangeType>()
                {
                    Change          = TUutAdapter.CreateRemoval(item),
                    IsSourceEmpty   = (item == priorChangeCount)
                })
                .ToArray(),
            invocation:         new()
            {
                Change          = TUutAdapter.CreateAddition(1),
                IsSourceEmpty   = true
            },
            because:            "an item cannot be removed from an empty collection");

    // Remaining scenarios are covered by BuildAndClear() tests
    
    private static void WhenChangeIsInvalidOrIncoherent_ThrowsException(
        bool                                                        isSourceEmpty,
        IReadOnlyList<AddChangeInvocation<TChange, TChangeType>>    priorInvocations,
        AddChangeInvocation<TChange, TChangeType>                   invocation,
        string                                                      because)
    {
        var uut = TUutAdapter.CreateUut(isSourceEmpty);
        
        foreach (var priorInvocation in priorInvocations)
            uut.AddChange(
                change:         priorInvocation.Change,
                isSourceEmpty:  priorInvocation.IsSourceEmpty);
        
        var priorChangeCount    = uut.Changes.Count;
        var priorType           = uut.CurrentType;
        var wasSourceEmpty      = uut.IsSourceEmpty;
        
        var result = uut.Invoking(uut => uut.AddChange(
                change:         invocation.Change,
                isSourceEmpty:  invocation.IsSourceEmpty))
            .Should().Throw<ArgumentException>(because)
            .Which;
        
        uut.Changes.Count.Should().Be(priorChangeCount, "a rejected change should restore the builder's prior state");
        uut.CurrentType.Should().Be(priorType,          "a rejected change should restore the builder's prior state");
        uut.IsSourceEmpty.Should().Be(wasSourceEmpty,   "a rejected change should restore the builder's prior state");

        Console.WriteLine(result);
    }
}
