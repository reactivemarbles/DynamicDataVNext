using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class AddTests
{
    [TestFixture]
    public class WhenNotificationsAreSuspended
        : Keyed.CacheTestBases.AddTests.Base<UutFixture.WhenNotificationsAreSuspended, ObservableCache<string, TestItem>>;
}
