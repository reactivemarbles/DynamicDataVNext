namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

[TestFixture]
public partial class AsClearTests
{
    [TestCase(1, TestName ="{m}(Single item)")]
    [TestCase(5, TestName ="{m}(Multiple items)")]
    public void WhenTypeIsClear_ResultMatchesChanges(int itemCount)
    {
        var items = Enumerable
            .Range(1, itemCount)
            .ToArray();

        var uut = DistinctChangeSet.CreateForClear(items: items);
        
        var result = uut.AsClear();

        result.Items.Should().BeEquivalentTo(items, static config => config.WithStrictOrdering(), "all removed items should be listed");
    }
}
