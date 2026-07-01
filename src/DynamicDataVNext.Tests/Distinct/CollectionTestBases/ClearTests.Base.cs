using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public static partial class ClearTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : ISetUutFixture<TUutFixture, TUut>
        where TUut : ISet<int>
    {
        [Test]
        public void WhenSetIsEmpty_DoesNothing()
        {
            using var fixture = TUutFixture.Create();

            fixture.Uut.Clear();
                
            fixture.Uut.Should().BeEmpty("the set should not have had any items added to it");
                
            fixture.AssertUutDidNothing();
        }
            
        [TestCaseSource(typeof(ClearTests), nameof(WhenSetIsNotEmpty_TestCases))]
        public void WhenSetIsNotEmpty_ClearsSet(IReadOnlyList<int> items)
        {
            using var fixture = TUutFixture.Create(items);
                
            fixture.Uut.Clear();
            
            fixture.Uut.Should().BeEmpty("the set should have been cleared");
            
            fixture.AssertUutWasCleared(items);
        }
    }
}
