namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class ResetTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : ICacheUutFixture<TUutFixture, TUut>
        where TUut : ICache<string, TestItem>,
            IRangeAwareCache<TestItem>
    {
        [Test]
        public void WhenCacheAndItemsAreEmpty_DoesNothing()
        {
            using var fixture = TUutFixture.Create(TestItem.SelectKey);
                
            fixture.Uut.Reset(Array.Empty<TestItem>());
            
            fixture.Uut.Should().BeEmpty("the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }

        [TestCaseSource(typeof(ResetTests), nameof(WhenCacheIsNotEmpty_TestCases))]
        public void WhenItemsIsEmptyAndCacheIsNot_ClearsCache(IReadOnlyList<TestItem> initialItems)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          initialItems);
                
            fixture.Uut.Reset(Array.Empty<TestItem>());
            
            fixture.Uut.Should().BeEmpty("the dictionary should have been cleared");
            
            fixture.AssertUutWasCleared(initialItems);
        }

        [TestCaseSource(typeof(ResetTests), nameof(WhenItemsIsNotEmpty_TestCases))]
        public void WhenItemsIsNotEmpty_ResetsCacheToItems(ItemRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems);
                
            fixture.Uut.Reset(testCase.Items);

            fixture.Uut.Should().BeEquivalentTo(testCase.Items, "the collection should have been reset to the given items");
            
            fixture.AssertUutWasReset(
                removedItems:   testCase.InitialItems,
                addedItems:     testCase.Items);
        }

        [TestCaseSource(typeof(ResetTests), nameof(WhenItemsKeysContainsNull_TestCases))]
        public void WhenItemsKeysContainsNull_ThrowsExceptionAndDoesNothing(ItemRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems);
                
            var result = FluentActions.Invoking(() =>
                {
                    fixture.Uut.Reset(testCase.Items);
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName("items")
                .Which;
                
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }

        [TestCaseSource(typeof(ResetTests), nameof(WhenCacheIsNotEmpty_TestCases))]
        public void WhenItemsKeysContainsNull_ThrowsExceptionAndDoesNothing(IReadOnlyList<TestItem> initialItems)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          initialItems);
                
            var result = FluentActions.Invoking(() =>
                {
                    fixture.Uut.Reset<IEnumerable<TestItem>>(null!);
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName("items")
                .Which;
                
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(initialItems, "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }
    }
}
