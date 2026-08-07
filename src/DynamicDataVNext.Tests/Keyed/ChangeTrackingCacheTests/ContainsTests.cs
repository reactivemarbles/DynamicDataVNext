using NUnit.Framework;

using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingCacheTests;

[TestFixture]
public sealed class ContainsTests
    : CacheTestBases.ContainsTests.Base<UutFixture, ChangeTrackingCache<string, TestItem>>;
