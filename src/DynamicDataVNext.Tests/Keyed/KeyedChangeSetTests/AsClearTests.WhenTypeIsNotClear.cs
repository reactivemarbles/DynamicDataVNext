namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public partial class AsClearTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenTypeIsNotClear_TestCasea
        = new[]
        {
            new TestCaseData(KeyedChangeSet.Empty<int, int>())              .SetName("{m}(Empty Changeset)"),
            new TestCaseData(KeyedChangeSet.CreateForReset(
                removedItems:   new[] { 1 },
                addedItems:     new[] { 2 },
                keySelector:    static item => item + 10))                  .SetName("{m}(Reset Changeset)"),
            new TestCaseData(KeyedChangeSet.CreateForUpdate(changes: new[]
            {
                KeyedChange.CreateAddition(
                    key:    1,
                    item:   2)
            }))                                                             .SetName("{m}(Update Changeset)"),
        };

    [TestCaseSource(nameof(WhenTypeIsNotClear_TestCasea))]
    public void WhenTypeIsNotClear_ThrowsException(KeyedChangeSet<int, int> uut)
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
