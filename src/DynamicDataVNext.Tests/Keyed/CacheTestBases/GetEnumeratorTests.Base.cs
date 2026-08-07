using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class GetEnumeratorTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IReadOnlyCacheUutFixture<TUutFixture, TUut>
        where TUut : IReadOnlyCache<string, TestItem>
    {
        [TestCaseSource(typeof(GetEnumeratorTests), nameof(Always_TestCases))]
        public void Always_EnumerationMatchesItems(IReadOnlyList<TestItem> items)
        {
            using var fixture = TUutFixture.Create(
                keySelector:    TestItem.SelectKey, 
                items:          items);

            fixture.Uut.Should().BeEquivalentTo(items, "all items in the set should be enumerated");
        }
    }
}
