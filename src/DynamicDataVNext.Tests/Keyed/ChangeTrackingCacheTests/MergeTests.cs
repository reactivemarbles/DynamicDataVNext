using NUnit.Framework;

using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingCacheTests;

[TestFixture]
public sealed class MergeTests
    : CacheTestBases.MergeTests.Base<UutFixture, ChangeTrackingCache<string, TestItem>>;
