using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

public abstract class BuildAndClearTestsBase<TUutAdapter, TChangeSet, TChange, TChangeType>
        : Base
    where TUutAdapter : IUutAdapter<TChangeSet, TChange, TChangeType>, new()
    where TChangeSet : struct, IChangeSet<TChange, TChangeType>
    where TChange : struct, IChange<TChangeType>
    where TChangeType : Enum
{
    public static readonly IReadOnlyList<TestCaseData> Removals_TestCases
        = new[]
        {
            new TestCaseData(1).SetName("{m}(Single removal)"),
            new TestCaseData(5).SetName("{m}(Multiple removals)")
        };

    public static readonly IReadOnlyList<TestCaseData> RemovalsAndAdditions_TestCases
        = new[]
        {
            //                  removalCount,   additionCount
            new TestCaseData(   0,              1)              .SetName("{m}(Single addition, No removals)"),
            new TestCaseData(   1,              1)              .SetName("{m}(Single addition, Single removal)"),
            new TestCaseData(   5,              1)              .SetName("{m}(Multiple additions, Single removal)"),
            new TestCaseData(   1,              5)              .SetName("{m}(Multiple additions, Multiple removals)")
        };

    [TestCaseSource(nameof(RemovalsAndAdditions_TestCases))]
    public void WhenAdditionsDoNotFollowEmptySource_ResultIsReset(
            int removalCount,
            int additionCount)
        => Always_ResultIsExpectedAndBuilderIsReset(
            isSourceEmpty:          false,
            addChangeInvocations:   Enumerable.Concat(
                    Enumerable.Range(1, removalCount)
                        .Select(item => new AddChangeInvocation<TChange, TChangeType>()
                        {
                            Change          = TUutAdapter.CreateRemoval(item),
                            IsSourceEmpty   = false
                        }),
                    Enumerable.Range(removalCount + 1, additionCount)
                        .Select(item => new AddChangeInvocation<TChange, TChangeType>()
                        {
                            Change          = TUutAdapter.CreateAddition(item),
                            IsSourceEmpty   = false
                        }))
                .ToArray(),
            expectedResultType:     ChangeSetType.Update,
            because:                (additionCount is 1)
                ? "an item was added to a non-empty source"
                : "items were added to a non-empty source");

    [TestCaseSource(nameof(RemovalsAndAdditions_TestCases))]
    public void WhenAdditionsFollowEmptySource_ResultIsReset(
            int removalCount,
            int additionCount)
        => Always_ResultIsExpectedAndBuilderIsReset(
            isSourceEmpty:          (removalCount is 0),
            addChangeInvocations:   Enumerable.Concat(
                    Enumerable.Range(1, removalCount)
                        .Select(item => new AddChangeInvocation<TChange, TChangeType>()
                        {
                            Change          = TUutAdapter.CreateRemoval(item),
                            IsSourceEmpty   = (item == removalCount)
                        }),
                    Enumerable.Range(removalCount + 1, additionCount)
                        .Select(item => new AddChangeInvocation<TChange, TChangeType>()
                        {
                            Change          = TUutAdapter.CreateAddition(item),
                            IsSourceEmpty   = false
                        }))
                .ToArray(),
            expectedResultType:     ChangeSetType.Reset,
            because:                (removalCount, additionCount) switch
            {
                (0, 1)  => "an item was added to an empty source",
                (0, >1) => "an item was added to an empty source",
                (>0, 1) => "all items were removed from the source, then a new item as added to it",
                _       => "all items were removed from the source, then new items were added to it" 
            });

    [TestCaseSource(nameof(IsSourceEmpty_TestCases))]
    public void WhenNoPendingChanges_ResultIsEmpty(bool isSourceEmpty)
        => Always_ResultIsExpectedAndBuilderIsReset(
            isSourceEmpty:          isSourceEmpty,
            addChangeInvocations:   Array.Empty<AddChangeInvocation<TChange, TChangeType>>(),
            expectedResultType:     ChangeSetType.Empty,
            because:                "no changes were added");
    
    [TestCaseSource(nameof(RemovalsAndAdditions_TestCases))]
    public void WhenRemovalFollowsReset_ResultIsUpdate(
            int removalCount,
            int additionCount)
        => Always_ResultIsExpectedAndBuilderIsReset(
            isSourceEmpty:          (removalCount is 0),
            addChangeInvocations:   Enumerable.Concat(
                    Enumerable.Range(1, removalCount)
                        .Select(item => new AddChangeInvocation<TChange, TChangeType>()
                        {
                            Change          = TUutAdapter.CreateRemoval(item),
                            IsSourceEmpty   = (item == removalCount) 
                        }),
                    Enumerable.Range(removalCount + 1, additionCount)
                        .Select(item => new AddChangeInvocation<TChange, TChangeType>()
                        {
                            Change          = TUutAdapter.CreateAddition(item),
                            IsSourceEmpty   = false
                        }))
                .Append(new()
                {
                    Change          = TUutAdapter.CreateRemoval(removalCount + additionCount),
                    IsSourceEmpty   = false
                })
                .ToArray(),
            expectedResultType:     ChangeSetType.Update,
            because:                "items were removed from the source, after a reset");

    public static readonly IReadOnlyList<TestCaseData> WhenSourceIsClearedAfterAdditions_TestCases
        = new[]
        {
            new TestCaseData(1, 1).SetName("{m}(Single Removal following Single Addition)"),
            new TestCaseData(1, 5).SetName("{m}(Multiple Removals following Single Addition)"),
            new TestCaseData(5, 1).SetName("{m}(Single Removal following Multiple Additions)"),
            new TestCaseData(5, 5).SetName("{m}(Multiple Removals following Multiple Additions)")
        };
    [TestCaseSource(nameof(WhenSourceIsClearedAfterAdditions_TestCases))]
    public void WhenSourceIsClearedAfterAdditions_ResultIsUpdate(
            int additionCount,
            int removalCount)
        => Always_ResultIsExpectedAndBuilderIsReset(
            isSourceEmpty:          false,
            addChangeInvocations:   Enumerable.Concat(
                    Enumerable.Range(1, additionCount)
                        .Select(item => new AddChangeInvocation<TChange, TChangeType>()
                        {
                            Change          = TUutAdapter.CreateAddition(item),
                            IsSourceEmpty   = false
                        }),
                    Enumerable.Range(additionCount + 1, removalCount)
                        .Select(item => new AddChangeInvocation<TChange, TChangeType>()
                        {
                            Change          = TUutAdapter.CreateRemoval(item),
                            IsSourceEmpty   = (item == (additionCount + removalCount)) 
                        }))
                .ToArray(),
            expectedResultType:     ChangeSetType.Update,
            because:                "items were added to the source before it was cleared");
    
    [TestCaseSource(nameof(Removals_TestCases))]
    public void WhenSourceIsEmptiedByRemovals_ResultIsClear(int removalCount)
        => Always_ResultIsExpectedAndBuilderIsReset(
            isSourceEmpty:          false,
            addChangeInvocations:   Enumerable.Range(1, removalCount)
                .Select(item => new AddChangeInvocation<TChange, TChangeType>()
                {
                    Change          = TUutAdapter.CreateRemoval(item),
                    IsSourceEmpty   = (item == removalCount)
                })
                .ToArray(),
            expectedResultType:     ChangeSetType.Clear,
            because:                "all items were removed from the source");

    [TestCaseSource(nameof(Removals_TestCases))]
    public void WhenSourceIsNotEmptiedByRemovals_ResultIsUpdate(int removalCount)
        => Always_ResultIsExpectedAndBuilderIsReset(
            isSourceEmpty:          false,
            addChangeInvocations:   Enumerable.Range(1, removalCount)
                .Select(item => new AddChangeInvocation<TChange, TChangeType>()
                {
                    Change          = TUutAdapter.CreateRemoval(item),
                    IsSourceEmpty   = (item == removalCount)
                })
                .ToArray(),
            expectedResultType:     ChangeSetType.Clear,
            because:                "not all items were removed from the source");
    
    private static void Always_ResultIsExpectedAndBuilderIsReset(
        bool                                                        isSourceEmpty,
        IReadOnlyList<AddChangeInvocation<TChange, TChangeType>>    addChangeInvocations,
        ChangeSetType                                               expectedResultType,
        string                                                      because)
    {
        var uut = TUutAdapter.CreateUut(isSourceEmpty);
        
        foreach (var invocation in addChangeInvocations)
            uut.AddChange(
                change:         invocation.Change,
                isSourceEmpty:  invocation.IsSourceEmpty);

        uut.Changes.Count.Should().Be(addChangeInvocations.Count, "{0} changes were added", addChangeInvocations);
        uut.CurrentType.Should().Be(expectedResultType, because);
        uut.IsSourceEmpty.Should().Be((addChangeInvocations.Count is 0)
            ? isSourceEmpty
            : addChangeInvocations[^1].IsSourceEmpty);
        
        var wasSourceEmpty = uut.IsSourceEmpty;
        
        var result = uut.BuildAndClear();

        TUutAdapter.AssertShouldBeValid(result);
        result.Changes.Should().BeEquivalentTo(
            expectation:    addChangeInvocations.Select(static invocation => invocation.Change),
            config:         options => options.WithStrictOrdering(),
            because:        "all added changes should be included");
        result.Type.Should().Be(expectedResultType);

        uut.Changes.Count.Should().Be(0, "all pending changes should be consumed");
        uut.CurrentType.Should().Be(ChangeSetType.Empty, "there are no pending changes");
        uut.IsSourceEmpty.Should().Be(wasSourceEmpty, "the state of the source collection should be remembered");
    }
}
