namespace DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

public class SingleItemOperationTestCase
{
    public required string Key { get; init; }

    public required int Value { get; init; }

    public required IReadOnlyList<KeyValuePair<string, int>> InitialItems { get; init; }
}
