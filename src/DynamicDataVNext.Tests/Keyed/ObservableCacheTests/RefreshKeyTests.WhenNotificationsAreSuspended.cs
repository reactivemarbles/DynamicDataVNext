using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class RefreshKeyTests
{
    [TestFixture]
    public class WhenNotificationsAreSuspended
        : Keyed.CacheTestBases.RefreshKeyTests.Base<UutFixture.WhenNotificationsAreSuspended, ObservableCache<string, TestItem>>;
}
