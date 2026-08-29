using Microsoft.VisualStudio.TestPlatform.Common.Filtering;

namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class RemoveTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IListUutFixture<TUutFixture, TUut>
        where TUut : IList<string?>
    {
        [TestCase(null, TestName = "{m}(Item is null)")]
        [TestCase("1",  TestName = "{m}(Unique item)")]
        public void WhenListContainsOnlyItem_ReturnsTrueAndClearsList(string? item)
        {
            var items = new[] { item };
        
            using var fixture = TUutFixture.Create(items);
            
            var result = fixture.Uut.Remove(item);
            
            result.Should().BeTrue("the collection contained the given item");
            
            fixture.Uut.Should().BeEmpty("the last item should have been removed from the collection");
            
            fixture.AssertUutWasCleared(removedItems: items);
        }

        [TestCaseSource(typeof(RemoveTests), nameof(WhenListContainsItemAndOtherItems_TestCases))]
        public void WhenListContainsItemAndOtherItems_ReturnsTrueAndRemovesItem(SingleIndexedItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
            
            var result = fixture.Uut.Remove(testCase.Item);
            
            result.Should().BeTrue("the collection contained the given item");
            
            var finalItems = Enumerable.Concat( 
                    testCase.InitialItems.Take(testCase.Index),
                    testCase.InitialItems.Skip(testCase.Index + 1))
                .ToArray();
            
            fixture.Uut.Should().BeEquivalentTo(finalItems, options => options.WithStrictOrdering(), "the given item should have been removed from the collection");
            
            fixture.AssertItemWasRemoved(
                removalIndex:   testCase.Index,
                removedItem:    testCase.Item);
        }
        
        [TestCaseSource(typeof(RemoveTests), nameof(WhenListDoesNotContainItem_TestCases))]
        public void WhenListDoesNotContainItem_ReturnsFalseAndDoesNothing(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
            
            var result = fixture.Uut.Remove(testCase.Item);
            
            result.Should().BeFalse("the collection did not contain the given item");
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, options => options.WithStrictOrdering(), "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }
        
        [Test]
        public void WhenItemsAreRemovedInForwardOrder_UpdatesList()
        {
            var initialItems = new[] { "1", "2", "3" };
        
            using var fixture = TUutFixture.Create(items: initialItems);
            
            foreach (var item in initialItems)
            {
                var result = fixture.Uut.Remove(item);
                
                result.Should().BeTrue("the collection contained the given item");
            }
            
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
            
            foreach (var item in initialItems.Reverse())
            {
                var result = fixture.Uut.Remove(item);
                
                result.Should().BeTrue("the collection contained the given item");
            }
            
            fixture.Uut.Should().BeEmpty("all items should have been removed from the collection");
            
            fixture.AssertUutWasCleared(removedItems: initialItems);
        }
    }    
}
