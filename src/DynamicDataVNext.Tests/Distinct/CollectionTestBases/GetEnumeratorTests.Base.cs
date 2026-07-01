using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public static partial class GetEnumeratorTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IReadOnlySetUutFixture<TUutFixture, TUut>
        where TUut : IReadOnlySet<int>
    {
        [TestCaseSource(typeof(GetEnumeratorTests), nameof(WhenSetIsNotMutatedDuringEnumeration_TestCases))]
        public void Always_EnumerationMatchesItems(IReadOnlyList<int> items)
        {
            using var fixture = TUutFixture.Create(items: items);

            fixture.Uut.Should().BeEquivalentTo(items, "all items in the set should be enumerated");
        }
    }
}
