using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class RemoveTests
{
    public partial class ForItem
    {
        [TestFixture]
        public class WhenCollectionChangedHasSubscribers
            : Keyed.CacheTestBases.RemoveTests.ForItemBase<UutFixture.WhenCollectionChangedHasSubscribers, ObservableCache<string, TestItem>>;
    }
}
