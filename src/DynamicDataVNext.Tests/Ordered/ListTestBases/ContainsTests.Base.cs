namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class ContainsTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IReadOnlyListUutFixture<TUutFixture, TUut>
        where TUut : IReadOnlyList<string?>
    {
        [TestCaseSource(typeof(ContainsTests), nameof(WhenListDoesNotContainItem_TestCases))]
        public void WhenListDoesNotContainItem_ReturnsFalse(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
            
            var result = fixture.Uut.Contains(testCase.Item);
            
            result.Should().BeFalse("the collection does not contain the given item");
        }

        [TestCaseSource(typeof(ContainsTests), nameof(WhenListContainsItem_TestCases))]
        public void WhenListContainsItem_ReturnsTrue(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
            
            var result = fixture.Uut.Contains(testCase.Item);
            
            result.Should().BeTrue("the collection contains the given item");
        }
    }
}
