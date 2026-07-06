using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

[TestFixture]
public partial class AsClearTests
{
    [TestCase(1, TestName = "{m}(Single item)")]
    [TestCase(5, TestName = "{m}(Multiple items)")]
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
