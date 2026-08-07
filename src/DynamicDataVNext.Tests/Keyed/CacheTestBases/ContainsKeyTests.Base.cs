using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class ContainsKeyTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IReadOnlyCacheUutFixture<TUutFixture, TUut>
        where TUut : IReadOnlyCache<string, TestItem>
    {
        [TestCaseSource(typeof(ContainsKeyTests), nameof(WhenKeyIsInCache_TestCases))]
        public void WhenKeyIsInCache_ReturnsTrue(SingleKeyOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems);

            var result = fixture.Uut.ContainsKey(testCase.Key);
                
            result.Should().BeTrue("the item is in the initial set of items");
        }

        [TestCaseSource(typeof(ContainsKeyTests), nameof(WhenKeyIsNotInCache_TestCases))]
        public void WhenKeyIsNotInCache_ReturnsFalse(SingleKeyOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems);

            var result = fixture.Uut.ContainsKey(testCase.Key);
                
            result.Should().BeFalse("the item is not in the initial set of items");
        }

        [Test]
        public void WhenKeyIsNull_ThrowsException()
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          Array.Empty<TestItem>());

            var result = fixture.Uut.Invoking(uut =>
                {
                    uut.ContainsKey(null!);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("key")
                .Which;
            
            Console.WriteLine(result);
        }
    }
}
