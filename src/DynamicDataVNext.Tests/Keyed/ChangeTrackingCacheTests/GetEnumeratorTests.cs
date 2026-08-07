using NUnit.Framework;

using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingCacheTests;

[TestFixture]
public sealed class GetEnumeratorTests
    : CacheTestBases.GetEnumeratorTests.Base<UutFixture, ChangeTrackingCache<string, TestItem>>;
