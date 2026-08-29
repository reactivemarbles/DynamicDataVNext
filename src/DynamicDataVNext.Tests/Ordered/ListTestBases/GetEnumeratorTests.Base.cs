namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class GetEnumeratorTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IReadOnlyListUutFixture<TUutFixture, TUut>
        where TUut : IReadOnlyList<string?>
    {
        [TestCaseSource(typeof(GetEnumeratorTests), nameof(Always_TestCases))]
        public void Always_EnumerationMatchesItems(IReadOnlyList<string?> items)
        {
            using var fixture = TUutFixture.Create(items: items);

            fixture.Uut.Should().BeEquivalentTo(items, options => options.WithStrictOrdering(), "all items in the set should be enumerated");
        }
    }
}
