using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public partial class UnionWithTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : ISetUutFixture<TUutFixture, TUut>
        where TUut : ISet<int>
    {
        [TestCaseSource(typeof(UnionWithTests), nameof(WhenOtherIsNotEmptyAndSetIsEmpty_TestCases))]
        public void WhenOtherIsNotEmptyAndSetIsEmpty_ResetsToOther(IReadOnlyList<int> other)
        {
            using var fixture = TUutFixture.Create();
                
            fixture.Uut.UnionWith(other);
            
            fixture.Uut.Should().BeEquivalentTo(other, "the set should have been reset to the given set");
            
            fixture.AssertUutWasReset(
                oldItems:   Array.Empty<int>(),
                newItems:   other);
        }

        [TestCaseSource(typeof(UnionWithTests), nameof(WhenOtherIsNotEmptyAndNotSubsetOfSetAndSetIsNotEmpty_TestCases))]
        public void WhenOtherIsNotEmptyAndNotSubsetOfSetAndSetIsNotEmpty_AddsNonOverlappingOtherItems(SetOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);
                
            fixture.Uut.UnionWith(testCase.Other);
            
            var addedItems = testCase.Other
                .Except(testCase.Items)
                .ToArray();
            
            var finalItems = Enumerable
                .Union(testCase.Items, testCase.Other)
                .ToArray();

            fixture.Uut.Should().BeEquivalentTo(finalItems, "all non-overlapping items between the two sets should have been added to the set");
            
            fixture.AssertItemsWereAdded(addedItems);
        }

        [TestCaseSource(typeof(UnionWithTests), nameof(WhenOtherIsSubsetOfSet_TestCases))]
        public void WhenOtherIsSubsetOfSet_DoesNothing(SetOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);
                
            fixture.Uut.UnionWith(testCase.Other);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.Items, "there were no items to be added to the set");
            
            fixture.AssertUutDidNothing();
        }
    }
}
