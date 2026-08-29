namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class InsertTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IListUutFixture<TUutFixture, TUut>
        where TUut : IList<string?>
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
            
            var result = fixture.Uut.Invoking(uut =>
                {
                    uut.Insert(
                        index:  index,
                        item:   (itemsCount + 1).ToString());
                })
                .Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName(nameof(index))
                .Which;
                
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(items, static options => options.WithStrictOrdering(), "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }
        
        [TestCase(null, TestName = "{m}(Item is null)")]
        [TestCase("1",  TestName = "{m}(Item is not null)")]
        public void WhenListIsEmpty_ResetsListToItem(string? item)
        {
            using var fixture = TUutFixture.Create();
            
            fixture.Uut.Insert(
                index:  0,
                item:   item);
            
            fixture.Uut.Should().BeEquivalentTo(new[] { item }, "the collection should have been reset to the given item");
            
            fixture.AssertUutWasReset(
                removedItems:   Array.Empty<string?>(),
                insertedItems:  new[] { item });
        }
        
        [TestCaseSource(typeof(InsertTests), nameof(WhenListIsNotEmpty_TestCases))]
        public void WhenListIsNotEmpty_InsertsItem(SingleIndexedItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
            
            fixture.Uut.Insert(
                index:  testCase.Index,
                item:   testCase.Item);
                
            var finalItems = Enumerable.Concat(
                    testCase.InitialItems.Take(testCase.Index)
                        .Append(testCase.Item),
                    testCase.InitialItems.Skip(testCase.Index))
                .ToArray();
                
            fixture.Uut.Should().BeEquivalentTo(finalItems, static options => options.WithStrictOrdering(), "the given item should have been inserted at the given index");
            
            fixture.AssertItemWasInserted(
                insertionIndex: testCase.Index,
                insertedItem:   testCase.Item);
        }
    }
}
