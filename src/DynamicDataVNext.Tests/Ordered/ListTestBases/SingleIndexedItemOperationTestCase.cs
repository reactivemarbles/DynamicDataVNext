namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public class SingleIndexedItemOperationTestCase
{
    public required IReadOnlyList<string?> InitialItems { get; init; }

    public required int Index { get; init; }

    public required string? Item { get; init; }
}
