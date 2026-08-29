using System.Xml.XPath;

namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class AddRangeTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IListUutFixture<TUutFixture, TUut>
        where TUut : IList<string?>
    {
        [TestCaseSource(typeof(AddRangeTests), nameof(InitialITems_TestCases))]
        public void WhenItemsIsNull_DoesNothingAndThrowsException(IReadOnlyList<string?> initialItems)
        {
            using var fixture = TUutFixture.Create(items: initialItems);
            
            var result = fixture.Invoking(fixture =>
                {
                    fixture.AddRangeToUut(null!);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("items")
                .Which;
                
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(initialItems, static options => options.WithStrictOrdering(), "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }
        
        [TestCaseSource(typeof(AddRangeTests), nameof(InitialITems_TestCases))]
        public void WhenItemsIsEmpty_DoesNothing(IReadOnlyList<string?> initialItems)
        {
            using var fixture = TUutFixture.Create(items: initialItems);
            
            fixture.AddRangeToUut(Array.Empty<string?>());
    
            fixture.Uut.Should().BeEquivalentTo(initialItems, static options => options.WithStrictOrdering(), "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }
        
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
                    fixture.AddRangeToUut(items);
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

        [TestCase(1, TestName = "{m}(Single item to add)")]
        [TestCase(3, TestName = "{m}(Multiple items to add)")]
        public void WhenListIsEmptyAndItemsIsNot_ResetsListToItems(int itemsCount)
        {
            var items = Enumerable.Range(1, itemsCount)
                .Select(static item => item.ToString())
                .ToArray();
            
            using var fixture = TUutFixture.Create();
            
            fixture.AddRangeToUut(items);
            
            fixture.Uut.Should().BeEquivalentTo(items, static options => options.WithStrictOrdering(), "the collection should have been reset to the given items");
            
            fixture.AssertUutWasReset(
                removedItems:   Array.Empty<string?>(),
                insertedItems:  items);
        }
        
        [TestCaseSource(typeof(AddRangeTests), nameof(WhenListAndItemsAreNotEmpty_TestCases))]
        public void WhenListIsNotEmpty_AppendsItemsToList(ItemRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
            
            fixture.AddRangeToUut(testCase.Items);
            
            var finalItems = Enumerable.Concat(
                testCase.InitialItems,
                testCase.Items);
            
            fixture.Uut.Should().BeEquivalentTo(finalItems, static options => options.WithStrictOrdering(), "the collection should have been reset to the given items");
            
            fixture.AssertItemsWereInserted(
                insertionIndex: testCase.InitialItems.Count,
                insertedItems:  testCase.Items);
        }
    }
}
