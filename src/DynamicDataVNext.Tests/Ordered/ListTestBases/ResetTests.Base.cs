namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class ResetTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IListUutFixture<TUutFixture, TUut>
        where TUut : IList<string?>
    {
        [TestCase(0, 1, 0, TestName = "{m}(Empty collection, Single item to add)")]
        [TestCase(1, 1, 0, TestName = "{m}(Single item in collection, Single item to add)")]
        [TestCase(1, 3, 0, TestName = "{m}(Single item in collection, Multiple items to add, First item throws)")]
        [TestCase(1, 3, 1, TestName = "{m}(Single item in collection, Multiple items to add, Median item throws)")]
        [TestCase(1, 3, 2, TestName = "{m}(Single item in collection, Multiple items to add, Last item throws)")]
        [TestCase(3, 1, 0, TestName = "{m}(Multiple items in collection, Single item to add)")]
        [TestCase(3, 3, 0, TestName = "{m}(Multiple items in collection, Multiple items to add, First item throws)")]
        [TestCase(3, 3, 1, TestName = "{m}(Multiple items in collection, Multiple items to add, Median item throws)")]
        [TestCase(3, 3, 2, TestName = "{m}(Multiple items in collection, Multiple items to add, Last item throws)")]
        public void WhenItemsThrows_DoesNothingAndPropagatesException(
            int initialItemCount,
            int itemCount,
            int exceptionIndex)
        {
            var initialItems = Enumerable.Range(1, initialItemCount)
                .Select(static item => item.ToString())
                .ToArray();
            
            using var fixture = TUutFixture.Create(items: initialItems);

            var exception = new TestException();
            var items = EnumerateItems();
            
            var result = fixture.Invoking(fixture =>
                {
                    fixture.ResetUut(items);
                })
                .Should().Throw<TestException>()
                .Which;
                
            result.Should().Be(exception);
                
            fixture.Uut.Should().BeEquivalentTo(initialItems, static options => options.WithStrictOrdering(), "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
            
            IEnumerable<string> EnumerateItems()
            {
                for (var index = 0; index < itemCount; ++index)
                {
                    if (index == exceptionIndex)
                        throw exception;
                        
                    yield return (index + initialItemCount).ToString();
                } 
            }
        }
    
        [Test]
        public void WhenItemsAndListAreEmpty_DoesNothing()
        {
            using var fixture = TUutFixture.Create();
            
            fixture.ResetUut(Array.Empty<string?>());
            
            fixture.Uut.Should().BeEmpty("the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }
        
        [TestCaseSource(typeof(ResetTests), nameof(WhenItemsIsEmptyAndListIsNot_TestCases))]
        public void WhenItemsIsEmptyAndListIsNot_ClearsList(IReadOnlyList<string?> initialItems)
        {
            using var fixture = TUutFixture.Create(items: initialItems);
            
            fixture.ResetUut(Array.Empty<string?>());
            
            fixture.Uut.Should().BeEmpty("the list should have been reset to empty");
            
            fixture.AssertUutWasCleared(removedItems: initialItems);
        }
        
        [TestCaseSource(typeof(ResetTests), nameof(WhenItemsIsNotEmpty_TestCases))]
        public void WhenItemsIsNotEmpty_ResetsListToItems(ItemRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
            
            fixture.ResetUut(testCase.Items);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.Items, options => options.WithStrictOrdering(), "the list should have been reset to the given items");
            
            fixture.AssertUutWasReset(
                removedItems:   testCase.InitialItems,
                insertedItems:  testCase.Items);
        }
    }
}
