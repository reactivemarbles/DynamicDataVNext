using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class RemoveTests
{
    public partial class ForKey
    {
        [TestFixture]
        public class WhenNoSubscriptionsAreActive
            : Keyed.CacheTestBases.RemoveTests.ForKeyBase<UutFixture.WhenNoSubscriptionsAreActive, ObservableCache<string, TestItem>>;
    }
}
