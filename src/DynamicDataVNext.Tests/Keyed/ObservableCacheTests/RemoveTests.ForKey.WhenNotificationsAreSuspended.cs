using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class RemoveTests
{
    public partial class ForKey
    {
        [TestFixture]
        public class WhenNotificationsAreSuspended
            : Keyed.CacheTestBases.RemoveTests.ForKeyBase<UutFixture.WhenNotificationsAreSuspended, ObservableCache<string, TestItem>>;
    }
}
