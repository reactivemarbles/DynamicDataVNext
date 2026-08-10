namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class TryGetItemTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : ICacheUutFixture<TUutFixture, TUut>
        where TUut : ICache<string, TestItem>
    {
        [TestCaseSource(typeof(TryGetItemTests), nameof(WhenCacheContainsKey_TestCases))]
        public void WhenCacheContainsKey_ReturnsTrueAndItem(SingleKeyOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems);
                
            var result = fixture.Uut.TryGetItem(testCase.Key, out var item);
            
            result.Should().BeTrue("the collection contains the given key");
            item.Should().Be(testCase.InitialItems.First(initialItem => initialItem.Key == testCase.Key), "the given key's item should have been retrieved");
        }

        [TestCaseSource(typeof(TryGetItemTests), nameof(WhenCacheDoesNotContainKey_TestCases))]
        public void WhenCacheDoesNotContainKey_ReturnsTrueAndItem(SingleKeyOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems);
                
            var result = fixture.Uut.TryGetItem(testCase.Key, out var item);
            
            result.Should().BeFalse("the collection does not contain the given key");
            item.Should().Be(default, "the default item value should have been retrieved");
        }

        [TestCaseSource(typeof(TryGetItemTests), nameof(InitialItems_TestCases))]
        public void WhenKeyIsNull_ThrowsException(IReadOnlyList<TestItem> initialItems)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          initialItems);
                
            var result = fixture.Uut.Invoking(uut =>
                {
                    uut.TryGetItem(null!, out _);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("key")
                .Which;
                
            Console.WriteLine(result);
        }
    }
}
