namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public partial class AsResetTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenTypeIsNotReset_TestCasea
        = new[]
        {
            new TestCaseData(KeyedChangeSet.CreateForClear(new[]
                {
                    new KeyedItem<int, int>() { Key = 1, Item = 2 }
                }))
                .SetName("{m}(Clear Changeset)"),
            new TestCaseData(KeyedChangeSet.Empty<int, int>())
                .SetName("{m}(Empty Changeset)"),
            new TestCaseData(KeyedChangeSet.CreateForUpdate(new[]
                {
                    KeyedChange.CreateAddition(
                        key:    1,
                        item:   2)
                }))
                .SetName("{m}(Update Changeset)"),
        };

    [TestCaseSource(nameof(WhenTypeIsNotReset_TestCasea))]
    public void WhenTypeIsNotReset_ThrowsException(KeyedChangeSet<int, int> uut)
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
}
