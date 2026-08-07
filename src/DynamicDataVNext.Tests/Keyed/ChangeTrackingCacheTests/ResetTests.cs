using NUnit.Framework;

using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingCacheTests;

[TestFixture]
public sealed class ResetTests
    : CacheTestBases.ResetTests.Base<UutFixture, ChangeTrackingCache<string, TestItem>>;
