using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingCacheTests;

[TestFixture]
public sealed class ClearTests
    : CacheTestBases.ClearTests.Base<UutFixture, ChangeTrackingCache<string, TestItem>>;
