using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class ConstructorTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : ICacheUutFixture<TUutFixture, TUut>
        where TUut : ICache<string, TestItem>,
            IExpandableCollection
    {
        [TestCase(-1,           TestName = "{m}(Max negative value)")]
        [TestCase(int.MinValue, TestName = "{m}(Min negative value)")]
        public void WhenCapacityIsNegative_ThrowsException(int capacity)
        {
            var result = FluentActions.Invoking(() =>
                {
                    using var fixture = TUutFixture.Create(
                        capacity:       capacity,
                        keySelector:    TestItem.SelectKey);
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
            var keySelector = TestItem.SelectKey;
        
            using var fixture = TUutFixture.Create(
                capacity:       capacity,
                keySelector:    keySelector);

            fixture.Uut.Should().BeEmpty("no initial items were given");
            fixture.Uut.Keys.Should().BeEmpty("no initial items were given");
            fixture.Uut.Capacity.Should().BeGreaterThanOrEqualTo(capacity, "an initial capacity was given");
            fixture.Uut.KeySelector.Should().BeSameAs(keySelector);
            fixture.UutComparer.Should().BeSameAs(EqualityComparer<string>.Default, "no equality comparer was specified");
            fixture.UutOptions.Should().Be(default(KeyedItemOptions), "no change tracking options were specified");
        }

        [TestCaseSource(typeof(ConstructorTests), nameof(WhenItemsContainsNullKey_TestCases))]
        public void WhenItemsContainsNullKey_ThrowsException(IReadOnlyList<TestItem> items)
        {
            var result = FluentActions.Invoking(() =>
                {
                    using var fixture = TUutFixture.Create(
                        items:          items,
                        keySelector:    TestItem.SelectKey);
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName(nameof(items))
                .Which;
            
            Console.WriteLine(result);
        }

        [TestCaseSource(typeof(ConstructorTests), nameof(WhenItemsIsNotNull_TestCases))]
        public void WhenItemsIsNotNull_ResultMatchesItems(IReadOnlyList<TestItem> items)
        {
            var keySelector = TestItem.SelectKey;

            using var fixture = TUutFixture.Create(
                items:          items,
                keySelector:    keySelector);

            fixture.Uut.Should().BeEquivalentTo(items, "an initial set of items was given");
            fixture.Uut.Keys.Should().BeEquivalentTo(items.Select(TestItem.SelectKey), "an initial set of items was given");
            fixture.Uut.KeySelector.Should().BeSameAs(keySelector);
            fixture.UutComparer.Should().BeSameAs(EqualityComparer<string>.Default, "no equality comparer was specified");
            fixture.UutOptions.Should().Be(default(KeyedItemOptions), "no change tracking options were specified");
        }

        [Test]
        public void WhenItemsIsNull_ThrowsException()
        {
            var result = FluentActions.Invoking(() =>
                {
                    using var fixture = TUutFixture.Create(
                        items:          null!,
                        keySelector:    TestItem.SelectKey);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("items")
                .Which;
            
            Console.WriteLine(result);
        }

        public static readonly IReadOnlyList<TestCaseData> WhenKeySelectorIsNull_TestCases
            = new[]
            {
                new TestCaseData(new Func<TUutFixture>(() => TUutFixture.Create(keySelector: null!)))
                    .SetName("{m}(Basic constructor)"),
                new TestCaseData(new Func<TUutFixture>(() => TUutFixture.Create(
                        capacity:       0,
                        keySelector:    null!)))
                    .SetName("{m}(Capacity constructor)"),
                new TestCaseData(new Func<TUutFixture>(() => TUutFixture.Create(
                        items:          Array.Empty<TestItem>(),
                        keySelector:    null!)))
                    .SetName("{m}(Items constructor)"),
            };
        [TestCaseSource(nameof(WhenKeySelectorIsNull_TestCases))]
        public void WhenKeySelectorIsNull_ThrowsException(Func<TUutFixture> constructor)
        {
            var result = FluentActions.Invoking(constructor)
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("keySelector")
                .Which;
            
            Console.WriteLine(result);
        }

        [Test]
        public void WhenNothingIsGiven_ResultIsEmpty()
        {
            var keySelector = TestItem.SelectKey;

            using var fixture = TUutFixture.Create(keySelector);

            fixture.Uut.Should().BeEmpty("no initial items were given");
            fixture.Uut.Keys.Should().BeEmpty("no initial items were given");
            fixture.Uut.KeySelector.Should().BeSameAs(keySelector);
            fixture.UutComparer.Should().BeSameAs(EqualityComparer<string>.Default, "no equality comparer was specified");
            fixture.UutOptions.Should().Be(default(KeyedItemOptions), "no change tracking options were specified");
        }

        [Test]
        public void WhenComparerIsGiven_ResultUsesComparer()
        {
            var comparer = EqualityComparer<string>.Create(static (x, y) => x == y);
            
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                comparer:       comparer);
            
            fixture.UutComparer.Should().BeSameAs(comparer, "a non-default equality comparer was given");
        }

        [Test]
        public void WhenOptionsIsGiven_ResultUsesOptions()
        {
            var options = new KeyedItemOptions()
            {
                ItemsAreMutable = true
            };
            
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey, 
                options:        options);
            
            fixture.UutOptions.Should().Be(options, "a non-default set of options was given");
        }
    }
}
