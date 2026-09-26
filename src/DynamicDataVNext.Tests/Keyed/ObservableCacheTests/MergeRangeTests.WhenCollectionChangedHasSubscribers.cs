using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class MergeRangeTests
{
    [TestFixture]
    public class WhenCollectionChangedHasSubscribers
        : Keyed.CacheTestBases.MergeRangeTests.Base<UutFixture.WhenCollectionChangedHasSubscribers, ObservableCache<string, TestItem>>;
}
