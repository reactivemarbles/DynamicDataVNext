using NUnit.Framework;

using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingCacheTests;

[TestFixture]
public sealed class RemoveRangeTests
    : CacheTestBases.RemoveRangeTests.Base<UutFixture, ChangeTrackingCache<string, TestItem>>;
