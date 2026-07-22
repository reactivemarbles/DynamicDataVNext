using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public partial class ExceptWithTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : ISetUutFixture<TUutFixture, TUut>
        where TUut : ISet<int>
    {
        [TestCaseSource(typeof(ExceptWithTests), nameof(WhenOtherDoesNotOverlapSet_TestCases))]
        public void WhenOtherDoesNotOverlapSet_DoesNothing(SetOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);

            fixture.Uut.ExceptWith(testCase.Other);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.Items, "the set should not have changed");
            
            fixture.AssertUutDidNothing();
        }

        [TestCaseSource(typeof(ExceptWithTests), nameof(WhenOtherIsSupersetOfSetAndSetIsNotEmpty_TestCases))]
        public void WhenOtherIsSupersetOfSetAndSetIsNotEmpty_ClearsSet(SetOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);
                
            fixture.Uut.ExceptWith(testCase.Other);
            
            fixture.Uut.Should().BeEmpty("the set should have been cleared");
            
            fixture.AssertUutWasCleared(testCase.Items);
        }

        [TestCaseSource(typeof(ExceptWithTests), nameof(WhenOtherOverlapsSetAndIsNotSupersetOfSet_TestCases))]
        public void WhenOtherOverlapsSetAndIsNotSupersetOfSet_RemovesOverlappingItems(SetOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);
                
            fixture.Uut.ExceptWith(testCase.Other);
            
            var removedItems = testCase.Items.Intersect(testCase.Other).ToArray();
            var finalItems = testCase.Items.Except(testCase.Other).ToArray();
            
            fixture.Uut.Should().BeEquivalentTo(finalItems, "the overlapping items between the two sets should have been removed");
            
            fixture.AssertItemsWereRemoved(
                removedItems:   removedItems,
                because:        "the overlapping items between the two sets should have been removed");
        }
    }
}
