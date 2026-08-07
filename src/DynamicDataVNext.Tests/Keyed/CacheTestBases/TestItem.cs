namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public record TestItem
{
    public static string SelectKey(TestItem item)
        => item.Key;

    public required string Key { get; init; }
    
    public int Version { get; init; }
}
