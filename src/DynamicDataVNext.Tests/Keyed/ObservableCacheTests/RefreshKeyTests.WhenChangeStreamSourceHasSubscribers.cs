using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class RefreshKeyTests
{
    [TestFixture]
    public class WhenChangeStreamSourceHasSubscribers
        : Keyed.CacheTestBases.RefreshKeyTests.Base<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableCache<string, TestItem>>;
}
