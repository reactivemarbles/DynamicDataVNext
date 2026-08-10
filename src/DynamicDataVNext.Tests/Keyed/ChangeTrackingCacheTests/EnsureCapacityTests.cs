using DynamicDataVNext.Tests.Keyed.CacheTestBases;


namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingCacheTests;

[TestFixture]
public class EnsureCapacityTests
    : EnsureCapacityTestsBase<ChangeTrackingCache<string, TestItem>>
{
    protected override ChangeTrackingCache<string, TestItem> CreateUut(int initialCapacity)
        => new(
            keySelector:    TestItem.SelectKey,
            capacity:       initialCapacity);
}
