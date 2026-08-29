namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class IndexOfTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IListUutFixture<TUutFixture, TUut>
        where TUut : IList<string?>
    {
        [TestCaseSource(typeof(IndexOfTests), nameof(WhenListDoesNotContainItem_TestCases))]
        public void WhenListDoesNotContainItem_ReturnsNegative1(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
            
            var result = fixture.Uut.IndexOf(testCase.Item);
            
            result.Should().Be(-1, "the collection does not contain the given item");
        }

        [TestCaseSource(typeof(IndexOfTests), nameof(WhenListContainsItem_TestCases))]
        public void WhenListContainsItem_ReturnsIndexOfFirstMatchingItem(SingleIndexedItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
            
            var result = fixture.Uut.IndexOf(testCase.Item);
            
            result.Should().Be(testCase.Index, "the index of the target item should have been retrieved");
        }
    }
}
