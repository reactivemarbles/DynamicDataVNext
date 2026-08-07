using NUnit.Framework;

using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingCacheTests;

[TestFixture]
public sealed class RefreshTests
    : CacheTestBases.RefreshTests.Base<UutFixture, ChangeTrackingCache<string, TestItem>>;
