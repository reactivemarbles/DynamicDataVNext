namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class IndexerTests
{
    public abstract class SetTestsBase<TUutFixture, TUut>
        where TUutFixture : IListUutFixture<TUutFixture, TUut>
        where TUut : class, IList<string?>
    {
        [TestCaseSource(typeof(IndexerTests), nameof(WhenIndexIsOutOfRange_TestCases))]
        public void WhenIndexIsOutOfRange_DoesNothingAndThrowsException(
            int index,
            int itemCount)
        {
            var initialItems = Enumerable.Range(1, itemCount)
                .Select(static item => item.ToString())
                .ToArray();
        
            using var fixture = TUutFixture.Create(initialItems);
            
            var result = fixture.Uut.Invoking(uut =>
                {
                    uut[index] = (itemCount + 1).ToString();
                })
                .Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName(nameof(index))
                .Which;
                
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(initialItems, options => options.WithStrictOrdering(), "the collection should not have been changed");

            fixture.AssertUutDidNothing();
        }

        [TestCaseSource(typeof(IndexerTests), nameof(WhenIndexIsInRange_TestCases))]
        public void WhenIndexIsInRangeAndItemIsDifferent_ReplacesItem(
            int index,
            int itemCount)
        {
            var initialItems = Enumerable.Range(1, itemCount)
                .Select(static item => item.ToString())
                .ToArray();
        
            using var fixture = TUutFixture.Create(initialItems);
            
            var item = (itemCount + 1).ToString();
            fixture.Uut[index] = item;

            var finalItems = initialItems
                .Select((existingItem, existingIndex) => (existingIndex == index)
                    ? item
                    : existingItem)
                .ToArray();

            fixture.Uut.Should().BeEquivalentTo(finalItems, options => options.WithStrictOrdering(), "the collection should not have been changed");

            fixture.AssertItemWasReplaced(
                replacementIndex:   index,
                replacedItem:       initialItems[index],
                replacementItem:    item);
        }

        [TestCaseSource(typeof(IndexerTests), nameof(WhenIndexIsInRange_TestCases))]
        public void WhenIndexIsInRangeAndItemIsSame_DoesNothing(
            int index,
            int itemCount)
        {
            var initialItems = Enumerable.Range(1, itemCount)
                .Select(static item => item.ToString())
                .ToArray();
        
            using var fixture = TUutFixture.Create(initialItems);
            
            fixture.Uut[index] = initialItems[index];

            fixture.Uut.Should().BeEquivalentTo(initialItems, options => options.WithStrictOrdering(), "the collection should not have been changed");

            fixture.AssertUutDidNothing();
        }
    }
}
