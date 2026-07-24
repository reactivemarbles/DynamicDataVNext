using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

public static partial class ContainsKeyTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IReadOnlyDictionaryUutFixture<TUutFixture, TUut>
        where TUut : IReadOnlyDictionary<string, int>
    {
        [TestCaseSource(typeof(ContainsKeyTests), nameof(WhenKeyIsInDictionary_TestCases))]
        public void WhenKeyIsInDictionary_ReturnsTrue(SingleKeyOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(testCase.InitialItems);

            var result = fixture.Uut.ContainsKey(testCase.Key);
                
            result.Should().BeTrue("the item is in the initial set of items");
        }

        [TestCaseSource(typeof(ContainsKeyTests), nameof(WhenKeyIsNotInDictionary_TestCases))]
        public void WhenKeyIsNotInDictionary_ReturnsFalse(SingleKeyOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(testCase.InitialItems);

            var result = fixture.Uut.ContainsKey(testCase.Key);
                
            result.Should().BeFalse("the item is not in the initial set of items");
        }

        [Test]
        public void WhenKeyIsNull_ThrowsException()
        {
            using var fixture = TUutFixture.Create(Array.Empty<KeyValuePair<string, int>>());

            var result = FluentActions.Invoking(() =>
                {
                    _ = fixture.Uut.ContainsKey(null!);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("key")
                .Which;
            
            Console.WriteLine(result);
        }
    }
}
