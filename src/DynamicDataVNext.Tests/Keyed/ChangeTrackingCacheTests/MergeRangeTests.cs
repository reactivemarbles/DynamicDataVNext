using NUnit.Framework;

using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingCacheTests;

[TestFixture]
public sealed class MergeRangeTests
    : CacheTestBases.MergeRangeTests.Base<UutFixture, ChangeTrackingCache<string, TestItem>>;
