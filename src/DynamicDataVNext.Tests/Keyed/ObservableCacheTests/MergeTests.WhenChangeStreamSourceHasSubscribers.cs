using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class MergeTests
{
    [TestFixture]
    public class WhenChangeStreamSourceHasSubscribers
        : Keyed.CacheTestBases.MergeTests.Base<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableCache<string, TestItem>>;
}
