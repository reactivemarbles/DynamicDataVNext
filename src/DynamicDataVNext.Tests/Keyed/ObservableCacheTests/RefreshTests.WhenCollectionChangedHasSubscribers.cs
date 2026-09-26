using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class RefreshTests
{
    [TestFixture]
    public class WhenCollectionChangedHasSubscribers
        : Keyed.CacheTestBases.RefreshTests.Base<UutFixture.WhenCollectionChangedHasSubscribers, ObservableCache<string, TestItem>>;
}
