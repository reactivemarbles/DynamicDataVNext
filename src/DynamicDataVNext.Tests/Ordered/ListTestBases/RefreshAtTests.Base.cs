namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class RefreshAtTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IListUutFixture<TUutFixture, TUut>
        where TUut : IList<string?>,
            IRefreshableList
    {
        [TestCaseSource(typeof(RefreshAtTests), nameof(WhenIndexIsNotInRange_TestCases))]
        public void WhenIndexIsNotInRange_DoesNothingAndThrowsException(SingleIndexOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                items:      testCase.InitialItems,
                options:    new OrderedItemOptions()
                {
                    ItemsAreMutable = true
                });

            var result = fixture.Uut.Invoking(uut =>
                {
                    uut.RefreshAt(testCase.Index);
                })
                .Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("index")
                .Which;
                
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, static options => options.WithStrictOrdering(), "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }

        [TestCaseSource(typeof(RefreshAtTests), nameof(WhenIndexIsInRange_TestCases))]
        public void WhenIndexIsInRange_RefreshesItem(SingleIndexOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                items:      testCase.InitialItems,
                options:    new OrderedItemOptions()
                {
                    ItemsAreMutable = true
                });

            fixture.Uut.RefreshAt(testCase.Index);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, static options => options.WithStrictOrdering(), "the collection should not have been changed");
            
            fixture.AssertItemWasRefreshed(
                refreshmentIndex:   testCase.Index,
                refreshedItem:      testCase.InitialItems[testCase.Index]);
        }

        [TestCaseSource(typeof(RefreshAtTests), nameof(WhenIndexIsInRange_TestCases))]
        public void WhenItemsAreNotMutable_DoesNothingAndThrowsException(SingleIndexOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                items:      testCase.InitialItems,
                options:    new OrderedItemOptions()
                {
                    ItemsAreMutable = false
                });

            var result = fixture.Uut.Invoking(uut =>
                {
                    uut.RefreshAt(testCase.Index);
                })
                .Should().Throw<ImmutableRefreshException>()
                .Which;
                
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, static options => options.WithStrictOrdering(), "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }
    }
}
