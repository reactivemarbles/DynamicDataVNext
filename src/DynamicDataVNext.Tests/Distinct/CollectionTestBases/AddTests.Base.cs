using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public static partial class AddTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : ISetUutFixture<TUutFixture, TUut>
        where TUut : ISet<int>
    {
        [TestCaseSource(typeof(AddTests), nameof(WhenItemIsInSet_TestCases))]
        public void WhenItemIsInSet_DoesNothingAndReturnsFalse(ItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);
                
            var result = fixture.Uut.Add(testCase.Item);
            
            result.Should().BeFalse("the given item was already present in the set");
            
            fixture.Uut.Should().Contain(testCase.Items, "the set should not have changed");
            
            fixture.AssertUutDidNothing();
        }
        
        [TestCaseSource(typeof(AddTests), nameof(WhenItemIsNotInSet_TestCases))]
        public void WhenItemIsNotInSet_AddsItemAndReturnsTrue(ItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);
                
            var result = fixture.Uut.Add(testCase.Item);
            
            result.Should().BeTrue("the given item was not present in the set");
            
            fixture.Uut.Should().Contain(testCase.Item, "the given item should have been added to the set");

            fixture.AssertItemWasAdded(testCase.Item);
        }
        
        [Test]
        public void WhenSetIsEmpty_ResetsSetAndReturnsTrue()
        {
            using var fixture = TUutFixture.Create();

            const int item = 1;
            
            var result = fixture.Uut.Add(item);
            
            result.Should().BeTrue("the item was not present in the set");
            
            fixture.Uut.Should().ContainSingle("the set should have been reset to the given item");
            fixture.Uut.Should().Contain(item, "the set should have been reset to the given item");

            fixture.AssertUutWasReset(
                oldItems: Array.Empty<int>(),
                newItems: new[] { item });
        }
    }
}
