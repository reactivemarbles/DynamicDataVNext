namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class ConstructorTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IListUutFixture<TUutFixture, TUut>
        where TUut : IList<string?>,
            IExpandableCollection
    {
        [TestCase(-1,           TestName = "{m}(Max negative value)")]
        [TestCase(int.MinValue, TestName = "{m}(Min negative value)")]
        public void WhenCapacityIsNegative_ThrowsException(int capacity)
        {
            var result = FluentActions.Invoking(() =>
                {
                    using var fixture = TUutFixture.Create(capacity);
                })
                .Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName(nameof(capacity))
                .Which;
            
            Console.WriteLine(result);
        }

        [TestCase(0,    TestName = "{m}(Empty capacity)")]
        [TestCase(1,    TestName = "{m}(Trivial capacity)")]
        [TestCase(10,   TestName = "{m}(Non-trivial capacity)")]
        public void WhenCapacityIsNotNegative_ResultIsEmpty(int capacity)
        {
            using var fixture = TUutFixture.Create(capacity);

            fixture.Uut.Should().BeEmpty("no initial items were given");
            fixture.Uut.Capacity.Should().BeGreaterThanOrEqualTo(capacity, "an initial capacity was given");
            fixture.UutOptions.Should().Be(default(OrderedItemOptions), "no change tracking options were specified");
        }

        [TestCaseSource(typeof(ConstructorTests), nameof(WhenItemsIsNotNull_TestCases))]
        public void WhenItemsIsNotNull_ResultMatchesItems(IReadOnlyList<string?> items)
        {
            using var fixture = TUutFixture.Create(items);

            fixture.Uut.Should().BeEquivalentTo(items, static options => options.WithStrictOrdering(), "an initial set of items was given");
            fixture.UutOptions.Should().Be(default(OrderedItemOptions), "no change tracking options were specified");
        }

        [Test]
        public void WhenItemsIsNull_ThrowsException()
        {
            var result = FluentActions.Invoking(() =>
                {
                    using var fixture = TUutFixture.Create(items: null!);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("items")
                .Which;
            
            Console.WriteLine(result);
        }

        [Test]
        public void WhenNothingIsGiven_ResultIsEmpty()
        {
            using var fixture = TUutFixture.Create();

            fixture.Uut.Should().BeEmpty("no initial items were given");
            fixture.UutOptions.Should().Be(default(OrderedItemOptions), "no change tracking options were specified");
        }

        [Test]
        public void WhenOptionsIsGiven_ResultUsesOptions()
        {
            var options = new OrderedItemOptions()
            {
                ItemsAreMutable = true
            };
            
            using var fixture = TUutFixture.Create(options: options);
            
            fixture.UutOptions.Should().Be(options, "a non-default set of options was given");
        }
    }
}
