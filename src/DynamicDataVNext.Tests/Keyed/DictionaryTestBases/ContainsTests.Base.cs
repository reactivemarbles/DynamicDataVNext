using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

public static partial class ContainsTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IDictionaryUutFixture<TUutFixture, TUut>
        where TUut : IDictionary<string, int>
    {
        [TestCaseSource(typeof(ContainsTests), nameof(WhenItemIsInDictionary_TestCases))]
        public void WhenItemIsInDictionary_ReturnsTrue(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(testCase.InitialItems);

            var result = fixture.Uut.Contains(new KeyValuePair<string, int>(testCase.Key, testCase.Value));
                
            result.Should().BeTrue("the item is in the initial set of items");
        }

        [TestCaseSource(typeof(ContainsTests), nameof(WhenItemIsNotInDictionary_TestCases))]
        public void WhenItemIsNotInDictionary_ReturnsFalse(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(testCase.InitialItems);

            var result = fixture.Uut.Contains(new KeyValuePair<string, int>(testCase.Key, testCase.Value));
                
            result.Should().BeFalse("the item is not in the initial set of items");
        }

        [Test]
        public void WhenItemKeyIsNull_ThrowsException()
        {
            using var fixture = TUutFixture.Create(Array.Empty<KeyValuePair<string, int>>());

            var result = FluentActions.Invoking(() =>
                {
                    _ = fixture.Uut.Contains(new(null!, 1));
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName("item")
                .Which;
            
            Console.WriteLine(result);
        }
    }
}
