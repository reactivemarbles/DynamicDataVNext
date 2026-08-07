using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class ContainsTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IReadOnlyCacheUutFixture<TUutFixture, TUut>
        where TUut : IReadOnlyCache<string, TestItem>
    {
        [TestCaseSource(typeof(ContainsTests), nameof(WhenItemIsInCache_TestCases))]
        public void WhenItemIsInCache_ReturnsTrue(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems);

            var result = fixture.Uut.Contains(testCase.Item);
                
            result.Should().BeTrue("the item is in the initial set of items");
        }

        [TestCaseSource(typeof(ContainsTests), nameof(WhenItemIsNotInCache_TestCases))]
        public void WhenItemIsNotInCache_ReturnsFalse(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems);

            var result = fixture.Uut.Contains(testCase.Item);
                
            result.Should().BeFalse("the item is not in the initial set of items");
        }

        [Test]
        public void WhenItemKeyIsNull_ThrowsException()
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          Array.Empty<TestItem>());

            var result = fixture.Uut.Invoking(uut =>
                {
                    _ = uut.Contains(new() { Key = null! });
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName("item")
                .Which;
            
            Console.WriteLine(result);
        }
    }
}
