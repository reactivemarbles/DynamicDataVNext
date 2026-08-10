using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingCacheTests;

[TestFixture]
public sealed class AddTests
    : CacheTestBases.AddTests.Base<UutFixture, ChangeTrackingCache<string, TestItem>>;
