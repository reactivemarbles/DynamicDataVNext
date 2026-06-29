using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

[TestFixture]
public class AsResetTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenTypeIsNotReset_TestCasea
        = new[]
        {
            new TestCaseData(OrderedChangeSet.CreateForClear(new[] { 1 }))
                .SetName("{m}(Clear Changeset)"),
            new TestCaseData(OrderedChangeSet.Empty<int>())
                .SetName("{m}(Empty Changeset)"),
            new TestCaseData(OrderedChangeSet.CreateForUpdate(new[]
                {
                    OrderedChange.CreateInsertion(
                        index:  0,
                        item:   1)
                }))
                .SetName("{m}(Update Changeset)"),
        };
    [TestCaseSource(nameof(WhenTypeIsNotReset_TestCasea))]
    public void WhenTypeIsNotReset_ThrowsException(OrderedChangeSet<int> uut)
    {
        var result = FluentActions.Invoking(() =>
            {
                _ = uut.AsReset();
            })
            .Should().Throw<InvalidOperationException>()
            .Which;
        
        result.Message.Should().Contain(nameof(ChangeSetType.Reset));
        
        Console.WriteLine(result);
    }
    
    public static readonly IReadOnlyList<TestCaseData> WhenTypeIsReset_TestCasea
        = new[]
        {
            new TestCaseData(0, 1).SetName("{m}(No removals, Single addition)"),
            new TestCaseData(1, 1).SetName("{m}(Single removal, Single addition)"),
            new TestCaseData(1, 5).SetName("{m}(Single removal, Multiple additions)"),
            new TestCaseData(5, 1).SetName("{m}(Multiple removals, Single addition)"),
            new TestCaseData(5, 5).SetName("{m}(Multiple removals, Multiple additions)")
        };
    [TestCaseSource(nameof(WhenTypeIsReset_TestCasea))]
    public void WhenTypeIsReset_ResultMatchesChanges(
        int removedItemCount,
        int addedItemCount)
    {
        var removedItems = Enumerable
            .Range(1, removedItemCount)
            .ToArray();
        
        var addedItems = Enumerable
            .Range(1 + removedItemCount, addedItemCount)
            .ToArray();

        var uut = OrderedChangeSet.CreateForReset(
            removedItems:   removedItems,
            addedItems:     addedItems);
        
        var result = uut.AsReset();

        result.ReversedRemovals.Should().BeEquivalentTo(removedItems.Reverse(), static config => config.WithStrictOrdering(), "all removed items should be listed");
        result.Additions.Should().BeEquivalentTo(addedItems, static config => config.WithStrictOrdering(), "all added items should be listed");
    }
}
