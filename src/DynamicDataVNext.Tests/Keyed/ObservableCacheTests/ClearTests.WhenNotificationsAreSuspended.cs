using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class ClearTests
{
    [TestFixture]
    public class WhenNotificationsAreSuspended
        : Keyed.CacheTestBases.ClearTests.Base<UutFixture.WhenNotificationsAreSuspended, ObservableCache<string, TestItem>>;
}
