namespace DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

public class ItemRangeOperationTestCase
{
    public required IReadOnlyList<KeyValuePair<string, int>> InitialItems { get; init; }

    public required IReadOnlyList<KeyValuePair<string, int>> Items { get; init; }
}
