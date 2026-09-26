using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class RemoveTests
{
    public partial class ForItem
    {
        [TestFixture]
        public class WhenChangeStreamSourceHasSubscribers
            : Keyed.CacheTestBases.RemoveTests.ForItemBase<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableCache<string, TestItem>>;
    }
}
