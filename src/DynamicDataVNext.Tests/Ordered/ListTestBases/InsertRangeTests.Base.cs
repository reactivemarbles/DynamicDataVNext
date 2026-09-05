namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class InsertRangeTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IListUutFixture<TUutFixture, TUut>
        where TUut : IList<string?>,
            IRangeAwareList<string?>
    {
        [TestCase(1, int.MinValue,  TestName = "{m}(Min negative value)")]
        [TestCase(1, -1,            TestName = "{m}(Max negative value)")]
        [TestCase(0, 1,             TestName = "{m}(Index exceeds bounds, Empty list)")]
        [TestCase(1, 2,             TestName = "{m}(Index exceeds bounds, Single item in list)")]
        [TestCase(3, 4,             TestName = "{m}(Index exceeds bounds, Multiple items in list)")]
        [TestCase(1, int.MaxValue,  TestName = "{m}(Max positive value)")]
        public void WhenIndexIsOutOfRange_DoesNothingAndThrowsException(
            int itemsCount,
            int index)
        {
            var items = Enumerable.Range(1, itemsCount)
                .Select(static item => item.ToString())
                .ToArray();
                
            using var fixture = TUutFixture.Create(items);
            
            var result = fixture.Invoking(fixture =>
                {
                    fixture.Uut.InsertRange(
                        index: index,
                        items: new[] { "1" });
                })
                .Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName(nameof(index))
                .Which;
                
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(items, static options => options.WithStrictOrdering(), "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }
        
        [TestCase(0,    1,  0,  TestName = "{m}(Empty collection, Single item to add)")]
        [TestCase(1,    1,  0,  TestName = "{m}(Single item in collection, Single item to insert)")]
        [TestCase(1,    3,  0,  TestName = "{m}(Single item in collection, Multiple items to insert, First item throws)")]
        [TestCase(1,    3,  1,  TestName = "{m}(Single item in collection, Multiple items to insert, Median item throws)")]
        [TestCase(1,    3,  2,  TestName = "{m}(Single item in collection, Multiple items to insert, Last item throws)")]
        [TestCase(3,    1,  0,  TestName = "{m}(Multiple items in collection, Single item to insert)")]
        [TestCase(3,    3,  0,  TestName = "{m}(Multiple items in collection, Multiple items to insert, First item throws)")]
        [TestCase(3,    3,  1,  TestName = "{m}(Multiple items in collection, Multiple items to insert, Median item throws)")]
        [TestCase(3,    3,  2,  TestName = "{m}(Multiple items in collection, Multiple items to insert, Last item throws)")]
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
            var items = BuildItems();
            
            var result = fixture.Invoking(fixture =>
                {
                    fixture.Uut.InsertRange(
                        index: 0,
                        items: items);
                })
                .Should().Throw<TestException>()
                .Which;
                
            result.Should().Be(exception);
                
            fixture.Uut.Should().BeEquivalentTo(initialItems, static options => options.WithStrictOrdering(), "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
            
            IEnumerable<string> BuildItems()
            {
                for (var index = 0; index < itemCount; ++index)
                {
                    if (index == exceptionIndex)
                        throw exception;
                        
                    yield return (index + initialItemCount).ToString();
                } 
            }
        }

        [TestCase(1, TestName = "{m}(Single item to insert)")]
        [TestCase(3, TestName = "{m}(Multiple items to insert)")]
        public void WhenListIsEmptyAndItemsIsNot_ResetsListToItems(int itemsCount)
        {
            var items = Enumerable.Range(1, itemsCount)
                .Select(static item => item.ToString())
                .ToArray();
            
            using var fixture = TUutFixture.Create();
            
            fixture.Uut.InsertRange(
                index: 0,
                items: items);
            
            fixture.Uut.Should().BeEquivalentTo(items, static options => options.WithStrictOrdering(), "the collection should have been reset to the given items");
            
            fixture.AssertUutWasReset(
                removedItems:   Array.Empty<string?>(),
                insertedItems:  items);
        }
        
        [TestCaseSource(typeof(InsertRangeTests), nameof(WhenListAndItemsAreNotEmpty_TestCases))]
        public void WhenListAndItemsAreNotEmpty_InsertsItems(IndexedItemRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
            
            fixture.Uut.InsertRange(
                index: testCase.Index,
                items: testCase.Items);
            
            var finalItems = Enumerable.Concat(
                    Enumerable.Concat(
                        testCase.InitialItems.Take(testCase.Index),
                        testCase.Items),
                    testCase.InitialItems.Skip(testCase.Index))
                .ToArray();
            
            fixture.Uut.Should().BeEquivalentTo(finalItems, static options => options.WithStrictOrdering(), "the given items should have been inserted into the collection");
            
            fixture.AssertItemsWereInserted(
                insertionIndex: testCase.Index,
                insertedItems:  testCase.Items);
        }
    }
}
