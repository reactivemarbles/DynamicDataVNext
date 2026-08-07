using NUnit.Framework;

using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingCacheTests;

[TestFixture]
public sealed class CopyToTests
    : CacheTestBases.CopyToTests.Base<UutFixture, ChangeTrackingCache<string, TestItem>>;
