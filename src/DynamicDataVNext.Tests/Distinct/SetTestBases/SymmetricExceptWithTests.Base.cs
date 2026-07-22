using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public partial class SymmetricExceptWithTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : ISetUutFixture<TUutFixture, TUut>
        where TUut : ISet<int>
    {
        [TestCaseSource(typeof(SymmetricExceptWithTests), nameof(WhenOtherDoesNotOverlapSetAndSetIsEmpty_TestCases))]
        public void WhenOtherDoesNotOverlapSetAndSetIsEmpty_ResetsToOther(IReadOnlyList<int> other)
        {
            using var fixture = TUutFixture.Create();
                
            fixture.Uut.SymmetricExceptWith(other);
            
            fixture.Uut.Should().BeEquivalentTo(other, "the set should have been reset to the given other set");
            
            fixture.AssertUutWasReset(
                oldItems:   Array.Empty<int>(),
                newItems:   other);
        }

        [TestCaseSource(typeof(SymmetricExceptWithTests), nameof(WhenOtherEqualsSetAndSetIsNotEmpty_TestCases))]
        public void WhenOtherEqualsSetAndSetIsNotEmpty_ClearsSet(IReadOnlyList<int> items)
        {
            using var fixture = TUutFixture.Create(items);
                
            fixture.Uut.SymmetricExceptWith(items);
            
            fixture.Uut.Should().BeEmpty("the set should have been cleared");
            
            fixture.AssertUutWasCleared(items);
        }

        [TestCaseSource(typeof(SymmetricExceptWithTests), nameof(WhenOtherIsEmpty_TestCases))]
        public void WhenOtherIsEmpty_DoesNothing(IReadOnlyList<int> items)
        {
            using var fixture = TUutFixture.Create(items);
                
            fixture.Uut.SymmetricExceptWith(Array.Empty<int>());
            
            fixture.Uut.Should().BeEquivalentTo(items, "the set should not have changed");
            
            fixture.AssertUutDidNothing();
        }

        [TestCaseSource(typeof(SymmetricExceptWithTests), nameof(WhenOtherIsNotEmptyAndDoesNotOverlapSetAndSetIsNotEmpty_TestCases))]
        public void WhenOtherIsNotEmptyAndDoesNotOverlapSetAndSetIsNotEmpty_AddsOtherItems(SetOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);
                
            fixture.Uut.SymmetricExceptWith(testCase.Other);
            
            var addedItems = testCase.Other.Except(testCase.Items).ToArray();
            
            var finalItems = Enumerable
                .Concat(testCase.Items, testCase.Other)
                .ToArray();
            
            fixture.Uut.Should().BeEquivalentTo(finalItems, "all items not already in the set should have been added");
            
            fixture.AssertItemsWereAdded(addedItems);
        }

        [TestCaseSource(typeof(SymmetricExceptWithTests), nameof(WhenOtherIsProperSupersetOfSetAndSetIsNotEmpty_TestCases))]
        public void WhenOtherIsProperSupersetOfSetAndSetIsNotEmpty_ResetsToNonOverlappingOtherItems(SetOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);
                
            fixture.Uut.SymmetricExceptWith(testCase.Other);
            
            var finalItems = testCase.Other
                .Except(testCase.Items)
                .ToArray();

            fixture.Uut.Should().BeEquivalentTo(finalItems, "all items in the set should have been removed and replaced with the non-overlapping items from the other set");
            
            fixture.AssertUutWasReset(
                oldItems:   testCase.Items,
                newItems:   finalItems);
        }

        [TestCaseSource(typeof(SymmetricExceptWithTests), nameof(WhenOtherOverlapsAndIsNotSupersetOfSet_TestCases))]
        public void WhenOtherOverlapsAndIsNotSupersetOfSet_RemovesOverlappingItemsAndAddsNonOverlappingOtherItems(SetOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);
                
            fixture.Uut.SymmetricExceptWith(testCase.Other);
            
            var removedItems = Enumerable.Intersect(testCase.Items, testCase.Other)
                .ToArray();
            
            var addedItems = testCase.Other
                .Except(testCase.Items)
                .ToArray();
            
            var finalItems = Enumerable.Union(testCase.Items, testCase.Other)
                .Except(Enumerable.Intersect(testCase.Items, testCase.Other))
                .ToArray();
            
            fixture.Uut.Should().BeEquivalentTo(finalItems, "all overlapping items between the sets should have been removed, and all non-overlapping items in the other set should have been added to the set");
            
            fixture.AssertUutWasUpdated(
                removedItems:           removedItems,
                addedItems:             addedItems,
                itemsRemovedBecause:    "all overlapping items between the sets should have been removed");
        }
    }
}
