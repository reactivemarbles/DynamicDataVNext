using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class MergeRangeTests
{
    [TestFixture]
    public class WhenChangeStreamSourceHasSubscribers
        : Keyed.CacheTestBases.MergeRangeTests.Base<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableCache<string, TestItem>>;
}
