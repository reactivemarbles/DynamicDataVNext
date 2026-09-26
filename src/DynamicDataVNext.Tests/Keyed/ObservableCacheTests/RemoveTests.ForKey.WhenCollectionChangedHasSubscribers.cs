using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class RemoveTests
{
    public partial class ForKey
    {
        [TestFixture]
        public class WhenCollectionChangedHasSubscribers
            : Keyed.CacheTestBases.RemoveTests.ForKeyBase<UutFixture.WhenCollectionChangedHasSubscribers, ObservableCache<string, TestItem>>;
    }
}
