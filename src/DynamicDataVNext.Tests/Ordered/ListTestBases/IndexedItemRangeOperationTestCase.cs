namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public class IndexedItemRangeOperationTestCase
{
    public required IReadOnlyList<string?> InitialItems { get; init; }

    public required int Index { get; init; }

    public required IReadOnlyList<string?> Items { get; init; }
}
