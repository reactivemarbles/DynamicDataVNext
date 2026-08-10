namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public partial class AsClearTests
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
}
