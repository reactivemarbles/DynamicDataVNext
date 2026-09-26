using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class AddRangeTests
{
    [TestFixture]
    public class WhenNoSubscriptionsAreActive
        : Keyed.CacheTestBases.AddRangeTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableCache<string, TestItem>>;
}
