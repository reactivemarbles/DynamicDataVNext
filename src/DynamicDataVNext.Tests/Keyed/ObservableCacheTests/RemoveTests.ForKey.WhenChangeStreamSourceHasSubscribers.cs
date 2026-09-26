using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class RemoveTests
{
    public partial class ForKey
    {
        [TestFixture]
        public class WhenChangeStreamSourceHasSubscribers
            : Keyed.CacheTestBases.RemoveTests.ForKeyBase<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableCache<string, TestItem>>;
    }
}
