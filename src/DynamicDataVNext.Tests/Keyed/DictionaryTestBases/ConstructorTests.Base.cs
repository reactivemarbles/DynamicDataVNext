using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed;

public static partial class ConstructorTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IDictionaryUutFixture<TUutFixture, TUut>
        where TUut : IDictionary<string, int>
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
            fixture.Uut.Keys.Should().BeEmpty("no initial items were given");
            fixture.Uut.Values.Should().BeEmpty("no initial items were given");
            fixture.UutCapacity.Should().BeGreaterThanOrEqualTo(capacity, "an initial capacity was given");
            fixture.UutComparer.Should().BeSameAs(EqualityComparer<string>.Default, "no equality comparer was specified");
            fixture.UutOptions.Should().Be(default(KeyedItemOptions), "no change tracking options were specified");
        }

        [TestCaseSource(typeof(ConstructorTests), nameof(WhenItemsContainsNullKey_TestCases))]
        public void WhenItemsContainsNullKey_ThrowsException(IReadOnlyList<KeyValuePair<string, int>> items)
        {
            var result = FluentActions.Invoking(() =>
                {
                    using var fixture = TUutFixture.Create(items);
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName(nameof(items))
                .Which;
            
            Console.WriteLine(result);
        }

        [TestCaseSource(typeof(ConstructorTests), nameof(WhenItemsIsNotNull_TestCases))]
        public void WhenItemsIsNotNull_ResultMatchesItems(IReadOnlyList<KeyValuePair<string, int>> items)
        {
            using var fixture = TUutFixture.Create(items);

            fixture.Uut.Should().BeEquivalentTo(items, "an initial set of items was given");
            fixture.Uut.Keys.Should().BeEquivalentTo(items.Select(static item => item.Key), "an initial set of items was given");
            fixture.Uut.Values.Should().BeEquivalentTo(items.Select(static item => item.Value), "an initial set of items was given");
            fixture.UutComparer.Should().BeSameAs(EqualityComparer<string>.Default, "no equality comparer was specified");
            fixture.UutOptions.Should().Be(default(KeyedItemOptions), "no change tracking options were specified");
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
            fixture.Uut.Keys.Should().BeEmpty("no initial items were given");
            fixture.Uut.Values.Should().BeEmpty("no initial items were given");
            fixture.UutComparer.Should().BeSameAs(EqualityComparer<string>.Default, "no equality comparer was specified");
            fixture.UutOptions.Should().Be(default(KeyedItemOptions), "no change tracking options were specified");
        }

        [Test]
        public void WhenComparerIsGiven_ResultUsesComparer()
        {
            var comparer = EqualityComparer<string>.Create(static (x, y) => x == y);
            
            using var fixture = TUutFixture.Create(comparer: comparer);
            
            fixture.UutComparer.Should().BeSameAs(comparer, "a non-default equality comparer was given");
        }

        [Test]
        public void WhenOptionsIsGiven_ResultUsesOptions()
        {
            var options = new KeyedItemOptions()
            {
                ItemsAreMutable = true
            };
            
            using var fixture = TUutFixture.Create(options: options);
            
            fixture.UutOptions.Should().Be(options, "a non-default set of options was given");
        }
    }
}
