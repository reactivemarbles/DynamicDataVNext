using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class ClearTests
{
    [TestFixture]
    public class WhenNoSubscriptionsAreActive
        : Keyed.CacheTestBases.ClearTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableCache<string, TestItem>>;
}
