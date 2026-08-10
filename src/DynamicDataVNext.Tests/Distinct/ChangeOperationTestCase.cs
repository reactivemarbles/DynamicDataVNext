namespace DynamicDataVNext.Tests.Distinct;

public class ChangeOperationTestCase
{
    public required string Because { get; init; }
    
    public required DistinctChangeSet<int> ChangeSet { get; init; }
    
    public required IReadOnlyList<int> ExpectedItems { get; init; }
    
    public required IReadOnlyList<int> InitialItems { get; init; }
}
