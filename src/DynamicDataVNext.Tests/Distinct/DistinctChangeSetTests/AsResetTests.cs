namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

[TestFixture]
public partial class AsResetTests
{
    [TestCase(0, 1, TestName = "{m}(No removals, Single addition)")]
    [TestCase(1, 1, TestName = "{m}(Single removal, Single addition)")]
    [TestCase(1, 5, TestName = "{m}(Single removal, Multiple additions)")]
    [TestCase(5, 1, TestName = "{m}(Multiple removals, Single addition)")]
    [TestCase(5, 5, TestName = "{m}(Multiple removals, Multiple additions)")]
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

        var uut = DistinctChangeSet.CreateForReset(
            removedItems:   removedItems,
            addedItems:     addedItems);
        
        var result = uut.AsReset();

        result.Removals.Should().BeEquivalentTo(removedItems, static config => config.WithStrictOrdering(), "all removed items should be listed");
        result.Additions.Should().BeEquivalentTo(addedItems, static config => config.WithStrictOrdering(), "all added items should be listed");
    }
}
