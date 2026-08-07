using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class IndexerTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IReadOnlyCacheUutFixture<TUutFixture, TUut>
        where TUut : class, IReadOnlyCache<string, TestItem>
    {
        [TestCaseSource(typeof(IndexerTests), nameof(WhenKeyIsNull_TestCases))]
        public void WhenKeyIsNull_ThrowsException(IReadOnlyList<TestItem> initialItems)
        {
            using var fixture = TUutFixture.Create(
                items:          initialItems,
                keySelector:    TestItem.SelectKey);
            
            var result = fixture.Uut.Invoking(uut =>
                {
                    _ = uut[null!];
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("key")
                .Which;
                
            Console.WriteLine(result);
        }
        
        [TestCaseSource(typeof(IndexerTests), nameof(WhenDictionaryContainsKey_TestCases))]
        public void WhenDictionaryContainsKey_ReturnsMatchingValue(SingleKeyOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                items:          testCase.InitialItems,
                keySelector:    TestItem.SelectKey);
            
            var result = fixture.Uut[testCase.Key];
            
            result.Should().Be(testCase.InitialItems.First(item => item.Key == testCase.Key), "the value in the collection for the given key should have been retrieved");
        }

        [TestCaseSource(typeof(IndexerTests), nameof(WhenDictionaryDoesNotContainKey_TestCases))]
        public void WhenDictionaryDoesNotContainKey_ThrowsException(SingleKeyOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                items:          testCase.InitialItems,
                keySelector:    TestItem.SelectKey);
            
            var result = fixture.Uut.Invoking(uut =>
                {
                    _ = uut[testCase.Key];
                })
                .Should().Throw<KeyNotFoundException>()
                .Which;

            Console.WriteLine(result);
        }
    }
}
