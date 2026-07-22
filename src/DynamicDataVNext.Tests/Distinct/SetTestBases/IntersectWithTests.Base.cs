using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public partial class IntersectWithTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : ISetUutFixture<TUutFixture, TUut>
        where TUut : ISet<int>
    {
        [TestCaseSource(typeof(IntersectWithTests), nameof(WhenOtherDoesNotOverlapSetAndSetIsNotEmpty_TestCases))]
        public void WhenOtherDoesNotOverlapSetAndSetIsNotEmpty_ClearsSet(SetOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);
                
            fixture.Uut.IntersectWith(testCase.Other);
            
            fixture.Uut.Should().BeEmpty("the set should have been cleared");
            
            fixture.AssertUutWasCleared(testCase.Items);
        }

        [TestCaseSource(typeof(IntersectWithTests), nameof(WhenOtherIsSupersetOfSet_TestCases))]
        public void WhenOtherIsSupersetOfSet_DoesNothing(SetOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);
                
            fixture.Uut.IntersectWith(testCase.Other);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.Items, "the set should not have changed");
            
            fixture.AssertUutDidNothing();
        }
        
        [TestCaseSource(typeof(IntersectWithTests), nameof(WhenOtherOverlapsSetAndIsNotSupersetOfSet_TestCases))]
        public void WhenOtherOverlapsSetAndIsNotSupersetOfSet_RemovesNonOverlappingItems(SetOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);
                
            fixture.Uut.IntersectWith(testCase.Other);
            
            var removedItems = testCase.Items.Except(testCase.Other).ToArray();
            var finalItems = testCase.Items.Intersect(testCase.Other).ToArray();
            
            fixture.Uut.Should().BeEquivalentTo(finalItems, "the non-overlapping items between the two sets should have been removed");
            
            fixture.AssertItemsWereRemoved(
                removedItems:   removedItems,
                because:        "the non-overlapping items between the two sets should have been removed");
        }
    }
}
