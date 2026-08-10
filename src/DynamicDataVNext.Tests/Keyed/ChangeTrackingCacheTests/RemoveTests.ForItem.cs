using DynamicDataVNext.Tests.Keyed.CacheTestBases;


namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingCacheTests;

public static partial class RemoveTests
{
    [TestFixture]
    public sealed class ForItem
        : CacheTestBases.RemoveTests.ForItemBase<UutFixture, ChangeTrackingCache<string, TestItem>>;

}
