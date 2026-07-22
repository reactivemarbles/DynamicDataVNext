using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed;

public static partial class ClearTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IDictionaryUutFixture<TUutFixture, TUut>
        where TUut : IDictionary<string, int>
    {
        [Test]
        public void WhenDictionaryIsEmpty_DoesNothing()
        {
            using var fixture = TUutFixture.Create();

            fixture.Uut.Clear();
                
            fixture.Uut.Should().BeEmpty("the dictionary should not have had any items added to it");
                
            fixture.AssertUutDidNothing();
        }
            
        [TestCaseSource(typeof(ClearTests), nameof(WhenDictionaryIsNotEmpty_TestCases))]
        public void WhenDictionaryIsNotEmpty_ClearsSet(IReadOnlyList<KeyValuePair<string, int>> initialItems)
        {
            using var fixture = TUutFixture.Create(items: initialItems);
                
            fixture.Uut.Clear();
            
            fixture.Uut.Should().BeEmpty("the dictionary should have been cleared");
            
            fixture.AssertUutWasCleared(initialItems);
        }
    }
}
