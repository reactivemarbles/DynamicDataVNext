namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

[TestFixture]
public partial class AsClearTests
{
    [TestCase(1, TestName = "{m}(Single item)")]
    [TestCase(5, TestName = "{m}(Multiple items)")]
    public void WhenTypeIsClear_ResultMatchesChanges(int itemCount)
    {
        var removals = Enumerable
            .Range(1, itemCount)
            .Select(item => new KeyedItem<int, int>()
            {
                Item    = item,
                Key     = item + 10
            })
            .ToArray();

        var uut = KeyedChangeSet.CreateForClear(removals);
        
        var result = uut.AsClear();

        result.Items.Should().BeEquivalentTo(removals, static config => config.WithStrictOrdering(), "all removed items should be listed");
    }
}
