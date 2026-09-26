using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class RefreshKeyTests
{
    [TestFixture]
    public class WhenNoSubscriptionsAreActive
        : Keyed.CacheTestBases.RefreshKeyTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableCache<string, TestItem>>;
}
