using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public static partial class ResetTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : ISetUutFixture<TUutFixture, TUut>
        where TUut : ISet<int>
    {
        [Test]
        public void WhenItemsAndSetAreEmpty_DoesNothing()
        {
            using var fixture = TUutFixture.Create();
                
            fixture.ResetUut(Array.Empty<int>());
            
            fixture.Uut.Should().BeEmpty("the set should remain empty");
            
            fixture.AssertUutDidNothing();
        }

        [TestCaseSource(typeof(ResetTests), nameof(WhenItemsAndSetAreNotEmpty_TestCases))]
        public void WhenItemsAndSetAreNotEmpty_ResetsSetToItems(SetOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);
                
            fixture.ResetUut(testCase.Other);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.Other, "the set should have been reset to the given set");
            
            fixture.AssertUutWasReset(
                oldItems: testCase.Items,
                newItems: testCase.Other);
        }

        [TestCaseSource(typeof(ResetTests), nameof(WhenItemsIsEmptyAndSetIsNot_TestCases))]
        public void WhenItemsIsEmptyAndSetIsNot_ClearsSet(IReadOnlyList<int> items)
        {
            using var fixture = TUutFixture.Create(items);
                
            fixture.ResetUut(Array.Empty<int>());
            
            fixture.Uut.Should().BeEmpty("the set should have been cleared");
            
            fixture.AssertUutWasCleared(items);
        }
    }
}
