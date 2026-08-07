using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class AddTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : ICacheUutFixture<TUutFixture, TUut>
        where TUut : ICache<string, TestItem>
    {
        [Test]
        public void WhenCacheIsEmpty_ResetsCache()
        {
            using var fixture = TUutFixture.Create(TestItem.SelectKey);
            
            var item = new TestItem()
            {
                Key = "1"
            };
            
            fixture.Uut.Add(item);
            
            fixture.Uut.Should().BeEquivalentTo(new[] { item }, "the item should have been added");
            
            fixture.AssertUutWasReset(
                removedItems:   Array.Empty<TestItem>(),
                addedItems:     new[] { item });
        }
        
        [TestCaseSource(typeof(AddTests), nameof(WhenCacheIsNotEmptyAndItemKeyIsNotInCache_TestCases))]
        public void WhenCacheIsNotEmptyAndItemKeyIsNotInDictionary_AddsItem(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems);

            fixture.Uut.Add(testCase.Item);

            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems.Append(testCase.Item), "the given item should have been added to the collection");

            fixture.AssertItemWasAdded(testCase.Item);
        }

        [TestCaseSource(typeof(AddTests), nameof(WhenItemKeyIsInCache_TestCases))]
        public void WhenItemKeyIsInDictionary_DoesNothingAndThrowsException(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems);

            var result = fixture.Uut.Invoking(uut =>
                {
                    uut.Add(testCase.Item);
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName("item")
                .Which;

            Console.WriteLine(result);

            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have been changed");

            fixture.AssertUutDidNothing();
        }
        
        [TestCaseSource(typeof(AddTests), nameof(WhenItemKeyIsNull_TestCases))]
        public void WhenItemKeyIsNull_ThrowsException(IReadOnlyList<TestItem> initialItems)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          initialItems);
            
            var result = fixture.Uut.Invoking(uut =>
                {
                    uut.Add(new TestItem() { Key = null! });
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName("item")
                .Which;
                
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(initialItems, "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }
    }
}
