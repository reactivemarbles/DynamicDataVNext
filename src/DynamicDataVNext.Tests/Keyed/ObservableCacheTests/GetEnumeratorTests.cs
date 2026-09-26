using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

[TestFixture]
public class GetEnumeratorTests
    : Keyed.CacheTestBases.GetEnumeratorTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableCache<string, TestItem>>;
