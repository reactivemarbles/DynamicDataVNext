namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class ClearTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : ICacheUutFixture<TUutFixture, TUut>
        where TUut : ICache<string, TestItem>
    {
        [Test]
        public void WhenCacheIsEmpty_DoesNothing()
        {
            using var fixture = TUutFixture.Create(TestItem.SelectKey);

            fixture.Uut.Clear();
                
            fixture.Uut.Should().BeEmpty("the collection should not have been changed");
                
            fixture.AssertUutDidNothing();
        }
            
        [TestCaseSource(typeof(ClearTests), nameof(WhenCacheIsNotEmpty_TestCases))]
        public void WhenCacheIsNotEmpty_ClearsCache(IReadOnlyList<TestItem> initialItems)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey, 
                items:          initialItems);
                
            fixture.Uut.Clear();
            
            fixture.Uut.Should().BeEmpty("the dictionary should have been cleared");
            
            fixture.AssertUutWasCleared(initialItems);
        }
    }
}
