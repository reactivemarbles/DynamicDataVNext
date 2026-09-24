namespace DynamicDataVNext.Tests.Keyed;

public class ChangeOperationTestCase
{
    public required string Because { get; init; }
    
    public required KeyedChangeSet<string, int> ChangeSet { get; init; }
    
    public required IReadOnlyList<KeyValuePair<string, int>> ExpectedItems { get; init; }
    
    public required IReadOnlyList<KeyValuePair<string, int>> InitialItems { get; init; }
}
