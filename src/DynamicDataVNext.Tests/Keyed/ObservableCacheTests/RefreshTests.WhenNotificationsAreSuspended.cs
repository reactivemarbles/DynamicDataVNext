using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class RefreshTests
{
    [TestFixture]
    public class WhenNotificationsAreSuspended
        : Keyed.CacheTestBases.RefreshTests.Base<UutFixture.WhenNotificationsAreSuspended, ObservableCache<string, TestItem>>;
}
