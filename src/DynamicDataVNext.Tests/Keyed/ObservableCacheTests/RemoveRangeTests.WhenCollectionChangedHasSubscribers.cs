using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class RemoveRangeTests
{
    [TestFixture]
    public class WhenCollectionChangedHasSubscribers
        : Keyed.CacheTestBases.RemoveRangeTests.Base<UutFixture.WhenCollectionChangedHasSubscribers, ObservableCache<string, TestItem>>;
}
