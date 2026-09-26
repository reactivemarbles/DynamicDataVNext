using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class AddTests
{
    [TestFixture]
    public class WhenChangeStreamSourceHasSubscribers
        : Keyed.CacheTestBases.AddTests.Base<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableCache<string, TestItem>>;
}
