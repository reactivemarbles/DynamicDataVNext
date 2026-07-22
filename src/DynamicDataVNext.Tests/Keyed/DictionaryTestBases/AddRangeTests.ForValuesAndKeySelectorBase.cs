using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed;

public static partial class AddRangeTests
{
    public abstract class ForValuesAndKeySelectorBase<TUutFixture, TUut>
        where TUutFixture : IDictionaryUutFixture<TUutFixture, TUut>
        where TUut : IDictionary<string, int>
    {
        [Test]
        public void WhenKeySelectorIsNull_ThrowsException()
        {
            using var fixture = TUutFixture.Create();
                
            var result = FluentActions.Invoking(() =>
                {
                    fixture.AddRangeToUut(
                        values:         Array.Empty<int>(),
                        keySelector:    null!);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("keySelector")
                .Which;
                
            Console.WriteLine(result);
            
            fixture.AssertUutDidNothing();
        }
        
        [TestCaseSource(typeof(AddRangeTests), nameof(WhenKeySelectorReturnsNull_TestCases))]
        public void WhenKeySelectorReturnsNull_DoesNothingAndThrowsException(ValueRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
            
            var result = FluentActions.Invoking(() =>
                {
                    fixture.AddRangeToUut(testCase.Values, testCase.KeySelector);
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName("keySelector")
                .Which;
            
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have changed");
            
            fixture.AssertUutDidNothing();
        }

        [TestCaseSource(typeof(AddRangeTests), nameof(WhenKeysProducedByKeySelectorForValuesAndDictionaryKeysOverlap_TestCases))]
        public void WhenKeysProducedByKeySelectorForValuesAndDictionaryKeysOverlap_DoesNothingAndThrowsException(ValueRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
            
            var result = FluentActions.Invoking(() =>
                {
                    fixture.AddRangeToUut(testCase.Values, testCase.KeySelector);
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName("keySelector")
                .Which;
            
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have changed");
            
            fixture.AssertUutDidNothing();
        }
        
        [TestCaseSource(typeof(AddRangeTests), nameof(WhenValuesAndDictionaryAreNotEmptyAndKeysDoNotOverlap_TestCases))]
        public void WhenValuesAndDictionaryAreNotEmptyAndKeysDoNotOverlap_AddsItems(ValueRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
                
            fixture.AddRangeToUut(testCase.Values, testCase.KeySelector);
            
            var addedItems = testCase.Values
                .Select(value => new KeyValuePair<string, int>(
                    key:    testCase.KeySelector.Invoke(value),
                    value:  value))
                .ToArray();

            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems.Concat(addedItems), "all given values should have been added to the dictionary");
            
            fixture.AssertItemsWereAdded(addedItems);
        }

        [TestCaseSource(typeof(AddRangeTests), nameof(WhenValuesAndKeySelectorProducesDuplicateKeys_TestCases))]
        public void WhenValuesAndKeySelectorProducesDuplicateKeys_DoesNothingAndThrowsException(ValueRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
            
            var result = FluentActions.Invoking(() =>
                {
                    fixture.AddRangeToUut(testCase.Values, testCase.KeySelector);
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName("keySelector")
                .Which;
            
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have changed");
            
            fixture.AssertUutDidNothing();
        }

        [TestCaseSource(typeof(AddRangeTests), nameof(InitialItems_TestCases))]
        public void WhenValuesIsEmpty_DoesNothing(IReadOnlyList<KeyValuePair<string, int>> initialItems)
        {
            using var fixture = TUutFixture.Create(items: initialItems);
                
            fixture.AddRangeToUut(
                values:         Array.Empty<int>(),
                keySelector:    static value => value.ToString());
            
            fixture.Uut.Should().BeEquivalentTo(initialItems, "the dictionary should have retained its initial items");
            
            fixture.AssertUutDidNothing();
        }
    
        [TestCaseSource(typeof(AddRangeTests), nameof(WhenValuesIsNotEmpty_TestCases))]
        public void WhenValuesIsNotEmptyAndDictionaryIsEmpty_ResetsToItems(IReadOnlyList<int> values)
        {
            using var fixture = TUutFixture.Create();
                
            var keySelector = static (int value) => value.ToString();
                
            fixture.AddRangeToUut(
                values:         values,
                keySelector:    keySelector);
            
            var items = values.Select(value => new KeyValuePair<string, int>(
                    key:    keySelector.Invoke(value),
                    value:  value))
                .ToArray();
            
            fixture.Uut.Should().BeEquivalentTo(items, "the dictionary should have been reset to the given values and keys");
            
            fixture.AssertUutWasReset(
                oldItems:   Array.Empty<KeyValuePair<string, int>>(),
                newItems:   items);
        }

        [Test]
        public void WhenValuesIsNull_ThrowsException()
        {
            using var fixture = TUutFixture.Create();
                
            var result = FluentActions.Invoking(() =>
                {
                    fixture.AddRangeToUut(
                        values:         null!,
                        keySelector:    static value => value.ToString());
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("values")
                .Which;
                
            Console.WriteLine(result);
            
            fixture.AssertUutDidNothing();
        }
    }
}
