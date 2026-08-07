using DynamicDataVNext.Tests.Keyed.CacheTestBases;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingCacheTests;

public static partial class RemoveTests
{
    [TestFixture]
    public sealed class ForKey
        : CacheTestBases.RemoveTests.ForKeyBase<UutFixture, ChangeTrackingCache<string, TestItem>>;

}
