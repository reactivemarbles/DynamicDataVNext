namespace DynamicDataVNext.Tests.Distinct;

public class SingleItemOperatorTestCase
{
    public required int Item { get; init; }

    public required IReadOnlyList<int> Items { get; init; }
    
    public required DistinctChangeSet<int> ChangeSet { get; init; }
}
