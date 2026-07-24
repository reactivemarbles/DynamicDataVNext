using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

public static partial class AddRangeTests
{
    public abstract class ForItemsBase<TUutFixture, TUut>
        where TUutFixture : IDictionaryUutFixture<TUutFixture, TUut>
        where TUut : IDictionary<string, int>
    {
        [TestCaseSource(typeof(AddRangeTests), nameof(WhenItemsAndDictionaryAreNotEmptyAndKeysDoNotOverlap_TestCases))]
        public void WhenItemsAndDictionaryAreNotEmptyAndKeysDoNotOverlap_AddsItems(ItemRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
                
            fixture.AddRangeToUut(testCase.Items);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems.Concat(testCase.Items), "all given items should have been added to the dictionary");
            
            fixture.AssertItemsWereAdded(testCase.Items);
        }
        
        [TestCaseSource(typeof(AddRangeTests), nameof(WhenItemsContainsNullKey_TestCases))]
        public void WhenItemsContainsNullKey_DoesNothingAndThrowsException(ItemRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
            
            var result = FluentActions.Invoking(() =>
                {
                    fixture.AddRangeToUut(testCase.Items);
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName("items")
                .Which;
            
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have changed");
            
            fixture.AssertUutDidNothing();
        }

        [TestCaseSource(typeof(AddRangeTests), nameof(WhenItemsHasDuplicateKeys_TestCases))]
        public void WhenItemsHasDuplicateKeys_DoesNothingAndThrowsException(ItemRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
            
            var result = FluentActions.Invoking(() =>
                {
                    fixture.AddRangeToUut(testCase.Items);
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName("items")
                .Which;
            
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have changed");
            
            fixture.AssertUutDidNothing();
        }

        [TestCaseSource(typeof(AddRangeTests), nameof(InitialItems_TestCases))]
        public void WhenItemsIsEmpty_DoesNothing(IReadOnlyList<KeyValuePair<string, int>> initialItems)
        {
            using var fixture = TUutFixture.Create(items: initialItems);
                
            fixture.AddRangeToUut(Array.Empty<KeyValuePair<string, int>>());
            
            fixture.Uut.Should().BeEquivalentTo(initialItems, "the dictionary should have retained its initial items");
            
            fixture.AssertUutDidNothing();
        }
    
        [TestCaseSource(typeof(AddRangeTests), nameof(WhenItemsIsNotEmpty_TestCases))]
        public void WhenItemsIsNotEmptyAndDictionaryIsEmpty_ResetsToItems(IReadOnlyList<KeyValuePair<string, int>> items)
        {
            using var fixture = TUutFixture.Create();
                
            fixture.AddRangeToUut(items);
            
            fixture.Uut.Should().BeEquivalentTo(items, "the dictionary should have been reset to the given items");
            
            fixture.AssertUutWasReset(
                oldItems:   Array.Empty<KeyValuePair<string, int>>(),
                newItems:   items);
        }

        [Test]
        public void WhenItemsIsNull_ThrowsException()
        {
            using var fixture = TUutFixture.Create();
                
            var result = FluentActions.Invoking(() =>
                {
                    fixture.AddRangeToUut(items: null!);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("items")
                .Which;
                
            Console.WriteLine(result);
            
            fixture.AssertUutDidNothing();
        }

        [TestCaseSource(typeof(AddRangeTests), nameof(WhenItemsKeysAndDictionaryKeysOverlap_TestCases))]
        public void WhenItemsKeysAndDictionaryKeysOverlap_DoesNothingAndThrowsException(ItemRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
            
            var result = FluentActions.Invoking(() =>
                {
                    fixture.AddRangeToUut(testCase.Items);
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName("items")
                .Which;
            
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have changed");
            
            fixture.AssertUutDidNothing();
        }
    }
}
