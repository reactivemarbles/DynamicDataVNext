using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class RemoveTests
{
    public abstract class ForItemBase<TUutFixture, TUut>
        where TUutFixture : ICacheUutFixture<TUutFixture, TUut>
        where TUut : ICache<string, TestItem>
    {
        [TestCaseSource(typeof(RemoveTests), nameof(WhenCacheContainsItem_TestCases))]
        public void WhenCacheContainsItem_ReturnsTrueAndRemovesItem(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems);
                
            var result = fixture.Uut.Remove(testCase.Item);
            
            result.Should().BeTrue("the collection contained the given item");
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems.Except(new[] { testCase.Item }), "the given item should have been removed");
            
            if (testCase.InitialItems.Count is 1)
                fixture.AssertUutWasCleared(new[] { testCase.Item });
            else
                fixture.AssertItemWasRemoved(testCase.Item);
        }

        [TestCaseSource(typeof(RemoveTests), nameof(WhenCacheDoesNotContainItem_TestCases))]
        public void WhenCacheDoesNotContainItem_ReturnsFalseAndDoesNothing(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          testCase.InitialItems);
                
            var result = fixture.Uut.Remove(testCase.Item);
            
            result.Should().BeFalse("the collection does not contain the given item");
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have been changed");

            fixture.AssertUutDidNothing();
        }
        
        [TestCaseSource(typeof(RemoveTests), nameof(InitialItems_TestCases))]
        public void WhenItemKeyIsNull_ThrowsExceptionAndDoesNothing(IReadOnlyList<TestItem> initialItems)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey,
                items:          initialItems);

            var result = fixture.Uut.Invoking(uut =>
                {
                    uut.Remove(new TestItem() { Key = null! });
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName("item")
                .Which;
                
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(initialItems, "the collection should not have changed");
            
            fixture.AssertUutDidNothing();
        }
    }
}
