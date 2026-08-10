using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingCacheTests;

[TestFixture]
public sealed class AddRangeTests
    : CacheTestBases.AddRangeTests.Base<UutFixture, ChangeTrackingCache<string, TestItem>>;
