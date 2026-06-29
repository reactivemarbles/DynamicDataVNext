using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

[TestFixture]
public class AsClearTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenTypeIsNotClear_TestCasea
        = new[]
        {
            new TestCaseData(OrderedChangeSet.Empty<int>())                     .SetName("{m}(Empty Changeset)"),
            new TestCaseData(OrderedChangeSet.CreateForReset(
                removedItems:   new[] { 2 },
                addedItems:     new[] { 3 }))                                   .SetName("{m}(Reset Changeset)"),
            new TestCaseData(OrderedChangeSet.CreateForUpdate(changes: new[]
            {
                OrderedChange.CreateInsertion(
                    index:  0,
                    item:   1)
            }))                                                                 .SetName("{m}(Update Changeset)"),
        };
    [TestCaseSource(nameof(WhenTypeIsNotClear_TestCasea))]
    public void WhenTypeIsNotClear_ThrowsException(OrderedChangeSet<int> uut)
    {
        var result = FluentActions.Invoking(() =>
            {
                _ = uut.AsClear();
            })
            .Should().Throw<InvalidOperationException>()
            .Which;
        
        result.Message.Should().Contain(nameof(ChangeSetType.Clear));
        
        Console.WriteLine(result);
    }
    
    public static readonly IReadOnlyList<TestCaseData> WhenTypeIsClear_TestCases
        = new[]
        {
            new TestCaseData(1).SetName("{m}(Single item)"),
            new TestCaseData(5).SetName("{m}(Multiple items)")
        };
    [TestCaseSource(nameof(WhenTypeIsClear_TestCases))]
    public void WhenTypeIsClear_ResultMatchesChanges(int itemCount)
    {
        var removals = Enumerable
            .Range(0, itemCount)
            .Select(index => new OrderedItem<int>()
            {
                Index   = index,
                Item    = index + 10
            })
            .ToArray();

        var uut = OrderedChangeSet.CreateForClear(removals);
        
        var result = uut.AsClear();

        result.ReversedItems.Should().BeEquivalentTo(removals.Reverse(), static config => config.WithStrictOrdering(), "all removed items should be listed");
    }
}
