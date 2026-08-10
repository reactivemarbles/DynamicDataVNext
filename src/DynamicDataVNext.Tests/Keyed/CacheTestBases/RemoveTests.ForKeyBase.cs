namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class RemoveTests
{
    public abstract class ForKeyBase<TUutFixture, TUut>
        where TUutFixture : ICacheUutFixture<TUutFixture, TUut>
        where TUut : ICache<string, TestItem>
    {
        [TestCaseSource(typeof(RemoveTests), nameof(WhenCacheContainsKey_TestCases))]
        public void WhenCacheContainsItem_ReturnsTrueAndRemovesItem(SingleKeyOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems);
                
            var result = fixture.Uut.Remove(testCase.Key, out var item);
            
            result.Should().BeTrue("the collection contained the given item");

            var removedItem = testCase.InitialItems.First(item => item.Key == testCase.Key);
            item.Should().Be(removedItem, "the given key's item should have been returned");
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems.Where(item => item.Key != testCase.Key), "the given item should have been removed");
            
            if (testCase.InitialItems.Count is 1)
                fixture.AssertUutWasCleared(new[] { removedItem });
            else
                fixture.AssertItemWasRemoved(removedItem);
        }

        [TestCaseSource(typeof(RemoveTests), nameof(WhenCacheDoesNotContainKey_TestCases))]
        public void WhenCacheDoesNotContainItem_ReturnsFalseAndDoesNothing(SingleKeyOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems);
                
            var result = fixture.Uut.Remove(testCase.Key, out var item);
            
            result.Should().BeFalse("the collection does not contain the given item");
            item.Should().Be(default, "the default value should have been returned");
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have been changed");

            fixture.AssertUutDidNothing();
        }
        
        [TestCaseSource(typeof(RemoveTests), nameof(InitialItems_TestCases))]
        public void WhenItemKeyIsNull_ThrowsExceptionAndDoesNothing(IReadOnlyList<TestItem> initialItems)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          initialItems);

            var result = fixture.Uut.Invoking(uut =>
                {
                    uut.Remove(null!, out _);
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName("key")
                .Which;
                
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(initialItems, "the collection should not have changed");
            
            fixture.AssertUutDidNothing();
        }
    }
}
