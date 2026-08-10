namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

[TestFixture]
public partial class AsResetTests
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
}
