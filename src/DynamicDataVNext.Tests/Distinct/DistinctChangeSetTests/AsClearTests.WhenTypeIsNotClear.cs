namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

public partial class AsClearTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenTypeIsNotClear_TestCases
        = new[]
        {
            new TestCaseData(DistinctChangeSet.Empty<int>())                    .SetName("{m}(Empty Changeset)"),
            new TestCaseData(DistinctChangeSet.CreateForReset(
                removedItems:   new[] { 1 },
                addedItems:     new[] { 2 }))                                   .SetName("{m}(Reset Changeset)"),
            new TestCaseData(DistinctChangeSet.CreateForUpdate(changes: new[]
            {
                new DistinctChange<int>()
                {
                    Item = 1,
                    Type = DistinctChangeType.Addition
                }
            }))                                                                 .SetName("{m}(Update Changeset)"),
        };

    [TestCaseSource(nameof(WhenTypeIsNotClear_TestCases))]
    public void WhenTypeIsNotClear_ThrowsException(DistinctChangeSet<int> uut)
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
