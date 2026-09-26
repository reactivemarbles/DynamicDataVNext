using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class MergeTests
{
    [TestFixture]
    public class WhenNoSubscriptionsAreActive
        : Keyed.CacheTestBases.MergeTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableCache<string, TestItem>>;
}
