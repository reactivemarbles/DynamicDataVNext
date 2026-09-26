using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public partial class ResetTests
{
    [TestFixture]
    public class WhenChangeStreamSourceHasSubscribers
        : Keyed.CacheTestBases.ResetTests.Base<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableCache<string, TestItem>>;
}
