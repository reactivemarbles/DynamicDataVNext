namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class IndexerTests
{
    public abstract class GetTestsBase<TUutFixture, TUut>
        where TUutFixture : IReadOnlyListUutFixture<TUutFixture, TUut>
        where TUut : class, IReadOnlyList<string?>
    {
        [TestCaseSource(typeof(IndexerTests), nameof(WhenIndexIsOutOfRange_TestCases))]
        public void WhenIndexIsOutOfRange_ThrowsException(
            int index,
            int itemCount)
        {
            var initialItems = Enumerable.Range(1, itemCount)
                .Select(static item => item.ToString())
                .ToArray();
        
            using var fixture = TUutFixture.Create(initialItems);
            
            var result = fixture.Uut.Invoking(uut =>
                {
                    _ = uut[index];
                })
                .Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName(nameof(index))
                .Which;
                
            Console.WriteLine(result);
        }
        
        [TestCaseSource(typeof(IndexerTests), nameof(WhenIndexIsInRange_TestCases))]
        public void WhenIndexIsInRange_RetrievesMatchingItem(
            int index,
            int itemCount)
        {
            var initialItems = Enumerable.Range(1, itemCount)
                .Select(static item => item.ToString())
                .ToArray();
        
            using var fixture = TUutFixture.Create(initialItems);
            
            var result = fixture.Uut[index];
                
            result.Should().Be(initialItems[index], "the item at the given index should have been retrieved");
        }
    }
}
