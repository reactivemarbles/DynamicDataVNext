using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class AddTests
{
    [TestFixture]
    public class WhenCollectionChangedHasSubscribers
        : Keyed.CacheTestBases.AddTests.Base<UutFixture.WhenCollectionChangedHasSubscribers, ObservableCache<string, TestItem>>;
}
