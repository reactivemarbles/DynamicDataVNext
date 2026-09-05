namespace DynamicDataVNext.Tests.Distinct.SetTestBases;

public static partial class RefreshTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : ISetUutFixture<TUutFixture, TUut>
        where TUut : ISet<int>,
            IRefreshableSet<int>
    {
        [TestCaseSource(typeof(ContainsTests), nameof(WhenItemIsInSet_TestCases))]
        public void WhenItemIsInSet_RefreshesItemAndReturnsTrue(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                items:      testCase.Items,
                options:    new DistinctItemOptions()
                {
                    ItemsAreMutable = true
                });

            var result = fixture.Uut.Refresh(testCase.Item);
                
            result.Should().BeTrue("the item is in the initial set of items");
            
            fixture.AssertItemWasRefreshed(testCase.Item);
        }

        [TestCaseSource(typeof(ContainsTests), nameof(WhenItemIsNotInSet_TestCases))]
        public void WhenItemIsNotInSet_DoesNothingAndReturnsFalse(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                items:      testCase.Items,
                options:    new DistinctItemOptions()
                {
                    ItemsAreMutable = true
                });

            var result = fixture.Uut.Refresh(testCase.Item);
                
            result.Should().BeFalse("the item is not in the initial set of items");
            
            fixture.AssertUutDidNothing();
        }

        [TestCaseSource(typeof(ContainsTests), nameof(WhenItemIsInSet_TestCases))]
        public void WhenItemsAreNotMutable_DoesNothingAndThrowsException(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                items:      testCase.Items,
                options:    new DistinctItemOptions()
                {
                    ItemsAreMutable = false
                });

            var result = fixture.Uut.Invoking(uut =>
                {
                    uut.Refresh(testCase.Item);
                })
                .Should().Throw<ImmutableRefreshException>()
                .Which;
                
            Console.WriteLine(result);
                
            fixture.Uut.Should().BeEquivalentTo(testCase.Items, "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }
    }
}
