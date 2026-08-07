using NUnit.Framework;

using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingCacheTests;

[TestFixture]
public sealed class ConstructorTests
    : CacheTestBases.ConstructorTests.Base<UutFixture, ChangeTrackingCache<string, TestItem>>;
