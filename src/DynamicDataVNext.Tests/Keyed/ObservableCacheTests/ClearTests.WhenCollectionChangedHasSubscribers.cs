using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class ClearTests
{
    [TestFixture]
    public class WhenCollectionChangedHasSubscribers
        : Keyed.CacheTestBases.ClearTests.Base<UutFixture.WhenCollectionChangedHasSubscribers, ObservableCache<string, TestItem>>;
}
