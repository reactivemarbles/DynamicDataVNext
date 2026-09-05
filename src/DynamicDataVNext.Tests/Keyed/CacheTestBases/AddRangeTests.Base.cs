namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class AddRangeTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : ICacheUutFixture<TUutFixture, TUut>
        where TUut : ICache<string, TestItem>,
            IRangeAwareCache<TestItem>
    {
        [TestCaseSource(typeof(AddRangeTests), nameof(WhenCacheIsEmptyAndItemsIsNot_TestCases))]
        public void WhenCacheIsEmptyAndItemsIsNot_ResetsCache(IReadOnlyList<TestItem> items)
        {
            using var fixture = TUutFixture.Create(TestItem.SelectKey);
            
            fixture.Uut.AddRange(items);
            
            fixture.Uut.Should().BeEquivalentTo(items, "the collection should not have been changed");
            
            fixture.AssertUutWasReset(
                removedItems:   Array.Empty<TestItem>(),
                addedItems:     items);
        }
        
        [TestCaseSource(typeof(AddRangeTests), nameof(WhenCacheIsNotEmptyAndContainsAnyItemsKeys_TestCases))]
        public void WhenCacheIsNotEmptyAndContainsAnyItemsKeys_DoesNothingAndThrowsException(ItemRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems);
            
            var result = fixture.Invoking(fixture =>
                {
                    fixture.Uut.AddRange(testCase.Items);
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName("items")
                .Which;
            
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }

        [TestCaseSource(typeof(AddRangeTests), nameof(WhenCacheIsNotEmptyAndDoesNotContainAnyItemsKeys_TestCases))]
        public void WhenCacheIsNotEmptyAndDoesNotContainAnyItemsKeys_AddsItems(ItemRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems);
            
            fixture.Uut.AddRange(testCase.Items);
            
            var finalItems = testCase.InitialItems
                .Concat(testCase.Items)
                .ToArray();
            
            fixture.Uut.Should().BeEquivalentTo(finalItems, "the given items should have been added to the collection");
            
            fixture.AssertItemsWereAdded(testCase.Items);
        }
    
        [TestCaseSource(typeof(AddRangeTests), nameof(InitialItems_TestCases))]
        public void WhenItemsIsEmpty_DoesNothing(IReadOnlyList<TestItem> initialItems)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          initialItems);
            
            fixture.Uut.AddRange(items: Array.Empty<TestItem>());
        
            fixture.Uut.Should().BeEquivalentTo(initialItems, "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }

        [TestCaseSource(typeof(AddRangeTests), nameof(InitialItems_TestCases))]
        public void WhenItemsIsNull_DoesNothingAndThrowsException(IReadOnlyList<TestItem> initialItems)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          initialItems);
            
            var result = fixture.Invoking(fixture =>
                {
                    fixture.Uut.AddRange(items: null!);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("items")
                .Which;
                
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(initialItems, "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }

        [TestCaseSource(typeof(AddRangeTests), nameof(WhenItemsKeysContainsNull_TestCases))]
        public void WhenItemsKeysContainsNull_DoesNothingAndThrowsException(ItemRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems);
            
            var result = fixture.Invoking(fixture =>
                {
                    fixture.Uut.AddRange(testCase.Items);
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
