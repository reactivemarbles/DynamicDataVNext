using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

[TestFixture]
public class CopyToTests
    : Keyed.CacheTestBases.CopyToTestsBase<UutFixture.WhenNoSubscriptionsAreActive, ObservableCache<string, TestItem>>;
