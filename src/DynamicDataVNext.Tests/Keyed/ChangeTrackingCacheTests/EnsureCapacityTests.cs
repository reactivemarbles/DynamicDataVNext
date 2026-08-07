using DynamicDataVNext.Tests.Keyed.CacheTestBases;

using NUnit.Framework;

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
