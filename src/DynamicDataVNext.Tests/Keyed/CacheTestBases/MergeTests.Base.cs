namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class MergeTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : ICacheUutFixture<TUutFixture, TUut>
        where TUut : class, ICache<string, TestItem>
    {
        [TestCaseSource(typeof(MergeTests), nameof(WhenDictionaryContainsKeyWithDifferentItem_TestCases))]
        public void WhenDictionaryContainsKeyWithDifferentItem_ReplacesItem(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                items:          testCase.InitialItems,
                keySelector:    TestItem.SelectKey);
            
            fixture.Uut.Merge(testCase.Item);
            
            var finalItems = testCase.InitialItems
                .Where(item => item.Key != testCase.Item.Key)
                .Append(testCase.Item)
                .ToArray();

            fixture.Uut.Should().BeEquivalentTo(finalItems, "the value for the given key should have been replaced");
            
            fixture.AssertItemWasReplaced(
                oldItem: testCase.InitialItems.First(item => item.Key == testCase.Item.Key),
                newItem: testCase.Item);
        }

        [TestCaseSource(typeof(MergeTests), nameof(WhenDictionaryContainsKeyWithSameItem_TestCases))]
        public void WhenDictionaryContainsKeyWithSameItem_DoesNothing(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                items:          testCase.InitialItems,
                keySelector:    TestItem.SelectKey);
            
            fixture.Uut.Merge(testCase.Item);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }

        [TestCaseSource(typeof(MergeTests), nameof(WhenDictionaryDoesNotContainKey_TestCases))]
        public void WhenDictionaryDoesNotContainKey_AddsItem(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                items:          testCase.InitialItems,
                keySelector:    TestItem.SelectKey);
            
            fixture.Uut.Merge(testCase.Item);
            
            var finalItems = testCase.InitialItems
                .Append(testCase.Item)
                .ToArray();

            fixture.Uut.Should().BeEquivalentTo(finalItems, "the given item should have been added to the collection");
            
            if (testCase.InitialItems.Count is 0)
                fixture.AssertUutWasReset(
                    removedItems:   Array.Empty<TestItem>(),
                    addedItems:     new[] { testCase.Item });
            else
                fixture.AssertItemWasAdded(testCase.Item);
        }
    }
}
