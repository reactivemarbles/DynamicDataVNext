using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public partial class RemoveTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : ISetUutFixture<TUutFixture, TUut>
        where TUut : ISet<int>
    {
        [Test]
        public void WhenItemIsOnlyItemInSet_ClearsSetAndReturnsTrue()
        {
            const int removedItem = 1;
            
            using var fixture = TUutFixture.Create(items: new[] { removedItem });
                
            var result = fixture.Uut.Remove(1);
            
            result.Should().BeTrue("the item was present in the set");
            
            fixture.Uut.Should().BeEmpty("the item should have been removed from the set");
            
            fixture.AssertUutWasCleared(new[] { removedItem });
        }
            
        [Test]
        public void WhenItemIsInSetAndNotOnlyItemInSet_RemovesItemAndReturnsTrue()
        {
            const int item = 2;

            using var fixture = TUutFixture.Create(items: new[] { 1, 2, 3 });
                
            var result = fixture.Uut.Remove(item);
            
            result.Should().BeTrue("the item was present in the set");
            
            fixture.Uut.Should().NotContain(2, "the item should have been removed from the set");
            fixture.Uut.Should().Contain(new[] { 1, 3 }, "other items should not have been removed from the set");
            
            fixture.AssertItemWasRemoved(item);
        }

        [TestCaseSource(typeof(RemoveTests), nameof(WhenItemIsNotInSet_TestCases))]
        public void WhenItemIsNotInSet_DoesNothingAndReturnsFalse(ItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);
                
            var result = fixture.Uut.Remove(testCase.Item);
            
            result.Should().BeFalse("the item was not present in the set");
            
            fixture.Uut.Should().Contain(testCase.Items, "the set should not have changed");
            
            fixture.AssertUutDidNothing();
        }
    }
}
