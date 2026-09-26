using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class RemoveTests
{
    public partial class ForItem
    {
        [TestFixture]
        public class WhenNotificationsAreSuspended
            : Keyed.CacheTestBases.RemoveTests.ForItemBase<UutFixture.WhenNotificationsAreSuspended, ObservableCache<string, TestItem>>;
    }
}
