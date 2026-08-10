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
    
    [TestCase(0, false,  false, TestName = "{m}(No pending changes)")]
    [TestCase(0, true,   false, TestName = "{m}(Source is initially empty)")]
    [TestCase(0, false,  true,  TestName = "{m}(Change empties source)")]
    [TestCase(1, false,  false, TestName = "{m}(Single pending change)")]
    [TestCase(1, true,   false, TestName = "{m}(Source is previously emptied)")]
    [TestCase(5, false,  false, TestName = "{m}(Multiple pending changes)")]
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

    [TestCase(0, TestName = "{m}(No pending changes)")]
    [TestCase(1, TestName = "{m}(Single pending change)")]
    [TestCase(5, TestName = "{m}(Multiple pending changes)")]
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
