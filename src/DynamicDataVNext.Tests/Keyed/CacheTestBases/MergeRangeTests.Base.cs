using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class MergeRangeTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : ICacheUutFixture<TUutFixture, TUut>
        where TUut : ICache<string, TestItem>
    {
        [TestCaseSource(typeof(MergeRangeTests), nameof(WhenCacheIsEmptyAndItemsIsNot_TestCases))]
        public void WhenCacheIsEmptyAndItemsIsNot_ResetsCache(IReadOnlyList<TestItem> items)
        {
            using var fixture = TUutFixture.Create(TestItem.SelectKey);
            
            fixture.MergeRangeIntoUut(items);
            
            fixture.Uut.Should().BeEquivalentTo(items, "the collection should not have been changed");
            
            fixture.AssertUutWasReset(
                removedItems:   Array.Empty<TestItem>(),
                addedItems:     items);
        }
        
        [TestCaseSource(typeof(MergeRangeTests), nameof(WhenItemsIsNotSubsetOfCache_TestCases))]
        public void WhenItemsIsNotSubsetOfCache_MergesItems(ItemRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems);
            
            fixture.MergeRangeIntoUut(testCase.Items);
    
            var finalItems = testCase.InitialItems
                .Where(item => !testCase.Items.Select(TestItem.SelectKey).Contains(item.Key))
                .Concat(testCase.Items)
                .ToArray();
            
            fixture.Uut.Should().BeEquivalentTo(finalItems, "the given items not already in the collection should have been merged into it");
            
            fixture.AssertItemsWereMerged(
                addedItems:     testCase.Items
                    .Where(item => !testCase.InitialItems.Select(TestItem.SelectKey).Contains(item.Key))
                    .ToArray(),
                replacements:   Enumerable.Join(
                        outer:              testCase.InitialItems,
                        inner:              testCase.Items,
                        outerKeySelector:   TestItem.SelectKey,
                        innerKeySelector:   TestItem.SelectKey,
                        resultSelector:     (initialItem, item) => new KeyedReplacement<string, TestItem>()
                        {
                            Key     = initialItem.Key,
                            OldItem = initialItem,
                            NewItem = item
                        })
                    .Where(static replacement => replacement.OldItem != replacement.NewItem)
                    .ToArray()
                );
        }

        [TestCaseSource(typeof(MergeRangeTests), nameof(WhenItemsIsSubsetOfCache_TestCases))]
        public void WhenItemsIsSubsetOfCache_DoesNothing(ItemRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems);
            
            fixture.MergeRangeIntoUut(testCase.Items);
        
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }

        [TestCaseSource(typeof(MergeRangeTests), nameof(InitialItems_TestCases))]
        public void WhenItemsIsNull_DoesNothingAndThrowsException(IReadOnlyList<TestItem> initialItems)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          initialItems);
            
            var result = fixture.Invoking(fixture =>
                {
                    fixture.MergeRangeIntoUut(items: null!);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("items")
                .Which;
                
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(initialItems, "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }

        [TestCaseSource(typeof(MergeRangeTests), nameof(WhenItemsKeysContainsNull_TestCases))]
        public void WhenItemsKeysContainsNull_DoesNothingAndThrowsException(ItemRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems);
            
            var result = fixture.Invoking(fixture =>
                {
                    fixture.MergeRangeIntoUut(testCase.Items);
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName("items")
                .Which;
                
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }
    }
}
