namespace DynamicDataVNext.Tests.Distinct.SetTestBases;

public class SetOperationTestCase
{
    public string? Because { get; init; }
    
    public required IReadOnlyList<int> Items { get; init; }

    public required IReadOnlyList<int> Other { get; init; }
}
