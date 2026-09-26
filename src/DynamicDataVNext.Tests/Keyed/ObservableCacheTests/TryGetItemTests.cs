using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

[TestFixture]
public class TryGetItemTests
    : Keyed.CacheTestBases.TryGetItemTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableCache<string, TestItem>>;
