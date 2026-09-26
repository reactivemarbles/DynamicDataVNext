using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class AddRangeTests
{
    [TestFixture]
    public class WhenNotificationsAreSuspended
        : Keyed.CacheTestBases.AddRangeTests.Base<UutFixture.WhenNotificationsAreSuspended, ObservableCache<string, TestItem>>;
}
