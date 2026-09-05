namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class RemoveRangeTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IListUutFixture<TUutFixture, TUut>
        where TUut : IList<string?>,
            IRangeAwareList<string?>
    {
        [TestCaseSource(typeof(RemoveRangeTests), nameof(WhenCountIsNotInRange_TestCases))]
        public void WhenCountIsNotInRange_DoesNothingAndThrowsException(NumericRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
            
            var result = fixture.Uut.Invoking(uut =>
                {
                    uut.RemoveRange(
                        index:  testCase.Index,
                        count:  testCase.Count);
                })
                .Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("count")
                .Which;
                
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, static options => options.WithStrictOrdering(), "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }

        [TestCaseSource(typeof(RemoveRangeTests), nameof(WhenIndexAndCountArePartOfList_TestCases))]
        public void WhenIndexAndCountArePartOfList_RemovesItemsInReverseOrder(NumericRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
            
            fixture.Uut.RemoveRange(
                index:  testCase.Index,
                count:  testCase.Count);
                
            var finalItems = Enumerable.Concat(
                    testCase.InitialItems.Take(testCase.Index),
                    testCase.InitialItems.Skip(testCase.Index + testCase.Count))
                .ToArray();
                
            fixture.Uut.Should().BeEquivalentTo(finalItems, static options => options.WithStrictOrdering(), "the items in the given range should have been removed");
            
            fixture.AssertItemsWereRemoved(removals: testCase.InitialItems
                .Select((item, index) => new OrderedItem<string?>()
                {
                    Index   = index,
                    Item    = item
                })
                .Skip(testCase.Index)
                .Take(testCase.Count)
                .Reverse()
                .ToArray());
        }

        [TestCaseSource(typeof(RemoveRangeTests), nameof(WhenIndexAndCountAreWholeList_TestCases))]
        public void WhenIndexAndCountAreWholeList_ClearsList(IReadOnlyList<string?> initialItems)
        {
            using var fixture = TUutFixture.Create(items: initialItems);
            
            fixture.Uut.RemoveRange(
                index:  0,
                count:  initialItems.Count);
                
            fixture.Uut.Should().BeEmpty("all items should have been removed from the list");
            
            fixture.AssertUutWasCleared(initialItems);
        }

        [TestCaseSource(typeof(RemoveRangeTests), nameof(WhenIndexIsInRangeAndCountIsZero_TestCases))]
        public void WhenIndexIsInRangeAndCountIsZero_DoesNothing(SingleIndexOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
            
            fixture.Uut.RemoveRange(
                index:  testCase.Index,
                count:  0);
                
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, static options => options.WithStrictOrdering(), "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }

        [TestCaseSource(typeof(RemoveRangeTests), nameof(WhenIndexIsNotInRange_TestCases))]
        public void WhenIndexIsNotInRange_DoesNothingAndThrowsException(SingleIndexOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
            
            var result = fixture.Uut.Invoking(uut =>
                {
                    uut.RemoveRange(
                        index:  testCase.Index,
                        count:  0);
                })
                .Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("index")
                .Which;
                
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, static options => options.WithStrictOrdering(), "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }
    }
}
