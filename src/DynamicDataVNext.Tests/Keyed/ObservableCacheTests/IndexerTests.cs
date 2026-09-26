using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

[TestFixture]
public class IndexerTests
    : Keyed.CacheTestBases.IndexerTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableCache<string, TestItem>>;
