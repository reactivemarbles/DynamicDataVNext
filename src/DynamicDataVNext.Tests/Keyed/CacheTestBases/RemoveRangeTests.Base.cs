namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class RemoveRangeTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : ICacheUutFixture<TUutFixture, TUut>
        where TUut : ICache<string, TestItem>,
            IRangeAwareCache<TestItem>
    {
        [TestCaseSource(typeof(RemoveRangeTests), nameof(WhenCacheContainsAnyOfItems_TestCases))]
        public void WhenCacheContainsAnyOfItems_RemovesMatchingItems(ItemRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems);
                
            fixture.Uut.RemoveRange(testCase.Items);
            
            var matchingItems = Enumerable.Intersect(
                    testCase.InitialItems,
                    testCase.Items)
                .ToArray();
                
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems.Except(testCase.Items), "the given items present in the collection should have been removed");
            
            if (matchingItems.Length == testCase.InitialItems.Count)
                fixture.AssertUutWasCleared(matchingItems);
            else
                fixture.AssertItemsWereRemoved(matchingItems);
        }
        
        [TestCaseSource(typeof(RemoveRangeTests), nameof(WhenCacheContainsNoneOfItems_TestCases))]
        public void WhenCacheContainsNoneOfItems_DoesNothing(ItemRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems);
                
            fixture.Uut.RemoveRange(testCase.Items);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }
        
        [TestCaseSource(typeof(RemoveRangeTests), nameof(WhenItemsKeysContainsNull_TestCases))]
        public void WhenItemsKeysContainsNull_ThrowsExceptionAndDoesNothing(ItemRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems);
                
            var result = fixture.Invoking(fixture =>
                {
                    fixture.Uut.RemoveRange(testCase.Items);
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName("items")
                .Which;
                
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }
    }
}
