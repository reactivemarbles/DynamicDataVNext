using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class ResetTests
{
    [TestFixture]
    public class WhenNoSubscriptionsAreActive
        : Keyed.CacheTestBases.ResetTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableCache<string, TestItem>>;
}
