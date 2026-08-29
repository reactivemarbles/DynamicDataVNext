namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class RemoveAtTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IListUutFixture<TUutFixture, TUut>
        where TUut : IList<string?>
    {
        [TestCaseSource(typeof(RemoveAtTests), nameof(WhenIndexIsInRangeAndListContainsManyItems_TestCases))]
        public void WhenIndexIsInRangeAndListContainsManyItems_RemovesItem(SingleIndexOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(testCase.InitialItems);
            
            fixture.Uut.RemoveAt(testCase.Index);
            
            var finalItems = Enumerable.Concat(
                    testCase.InitialItems.Take(testCase.Index),
                    testCase.InitialItems.Skip(testCase.Index + 1))
                .ToArray();
            
            fixture.Uut.Should().BeEquivalentTo(finalItems, options => options.WithStrictOrdering(), "the item at the given index should have been removed from the collection");
            
            fixture.AssertItemWasRemoved(
                removalIndex:   testCase.Index,
                removedItem:    testCase.InitialItems[testCase.Index]);
        }
        
        [TestCase(null, TestName = "{m}(Item is null)")]
        [TestCase("1",  TestName = "{m}(Unique item)")]
        public void WhenIndexIsInRangeAndListContainsSingleItem_ClearsList(string? item)
        {
            var items = new[] { item };
        
            using var fixture = TUutFixture.Create(items);
            
            fixture.Uut.RemoveAt(0);
            
            fixture.Uut.Should().BeEmpty("the last item should have been removed from the collection");
            
            fixture.AssertUutWasCleared(removedItems: items);
        }
        
        [TestCaseSource(typeof(RemoveAtTests), nameof(WhenIndexIsNotInRange_TestCases))]
        public void WhenIndexIsNotInRange_DoesNothingAndThrowsException(SingleIndexOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(testCase.InitialItems);

            var result = fixture.Uut.Invoking(uut =>
                {
                    uut.RemoveAt(testCase.Index);
                })
                .Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("index")
                .Which;
            
            Console.WriteLine(result);

            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, options => options.WithStrictOrdering(), "the collection should not have been changed");

            fixture.AssertUutDidNothing();
        }

        [Test]
        public void WhenItemsAreRemovedInForwardOrder_UpdatesList()
        {
            var initialItems = new[] { "1", "2", "3" };
        
            using var fixture = TUutFixture.Create(items: initialItems);
            
            for (var i = 0; i < initialItems.Length; ++i)
                fixture.Uut.RemoveAt(0);
            
            fixture.Uut.Should().BeEmpty("all items should have been removed from the collection");
            
            fixture.AssertItemsWereRemoved(removals: initialItems
                .Select(item => new OrderedItem<string?>()
                {
                    Index   = 0,
                    Item    = item
                })
                .ToArray());
        }

        [Test]
        public void WhenItemsAreRemovedInReverseOrder_ClearsList()
        {
            var initialItems = new[] { "1", "2", "3" };
        
            using var fixture = TUutFixture.Create(items: initialItems);
            
            for (var i = initialItems.Length - 1; i >= 0; --i)
                fixture.Uut.RemoveAt(i);
            
            fixture.Uut.Should().BeEmpty("all items should have been removed from the collection");
            
            fixture.AssertUutWasCleared(removedItems: initialItems);
        }
    }
}
