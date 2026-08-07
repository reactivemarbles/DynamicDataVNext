using NUnit.Framework;

using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingCacheTests;

[TestFixture]
public sealed class ContainsKeyTests
    : CacheTestBases.ContainsKeyTests.Base<UutFixture, ChangeTrackingCache<string, TestItem>>;
