using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class RemoveTests
{
    public partial class ForItem
    {
        [TestFixture]
        public class WhenNoSubscriptionsAreActive
            : Keyed.CacheTestBases.RemoveTests.ForItemBase<UutFixture.WhenNoSubscriptionsAreActive, ObservableCache<string, TestItem>>;
    }
}
