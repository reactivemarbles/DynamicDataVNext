using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

public static partial class ResetTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IDictionaryUutFixture<TUutFixture, TUut>
        where TUut : IDictionary<string, int>
    {
        [TestCaseSource(typeof(ResetTests), nameof(WhenKeySelectorReturnsNull_TestCases))]
        public void WhenKeySelectorReturnsNull_DoesNothingAndThrowsException(ValueRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
                
            var result = FluentActions.Invoking(() =>
                {
                    fixture.ResetUut(testCase.Values, testCase.KeySelector);
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName("keySelector")
                .Which;
                
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }
    
        [Test]
        public void WhenValuesAndDictionaryAreEmpty_DoesNothing()
        {
            using var fixture = TUutFixture.Create();
                
            fixture.ResetUut(
                values:         Array.Empty<int>(),
                keySelector:    static value => value.ToString());
            
            fixture.Uut.Should().BeEmpty("the dictionary should remain empty");
            
            fixture.AssertUutDidNothing();
        }

        [TestCaseSource(typeof(ResetTests), nameof(WhenValuesIsEmptyAndDictionaryIsNot_TestCases))]
        public void WhenValuesIsEmptyAndDictionaryIsNot_ClearsDictionary(IReadOnlyList<KeyValuePair<string, int>> initialItems)
        {
            using var fixture = TUutFixture.Create(initialItems);
                
            fixture.ResetUut(
                values:         Array.Empty<int>(),
                keySelector:    static value => value.ToString());
            
            fixture.Uut.Should().BeEmpty("the dictionary should have been cleared");
            
            fixture.AssertUutWasCleared(initialItems);
        }

        [TestCaseSource(typeof(ResetTests), nameof(WhenValuesIsNotEmpty_TestCases))]
        public void WhenValuesIsNotEmpty_ResetsDictionaryToValues(ValueRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
                
            fixture.ResetUut(testCase.Values, testCase.KeySelector);
            
            var items = testCase.Values
                .Select(value => new KeyValuePair<string, int>(
                    key:    testCase.KeySelector.Invoke(value),
                    value:  value))
                .ToArray();

            fixture.Uut.Should().BeEquivalentTo(items, "the dictionary should have been reset to the given values");
            
            fixture.AssertUutWasReset(
                oldItems: testCase.InitialItems,
                newItems: items);
        }
    }
}
