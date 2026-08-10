namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public class SingleKeyOperationTestCase
{
    public required string Key { get; init; }

    public required IReadOnlyList<TestItem> InitialItems { get; init; }
}
