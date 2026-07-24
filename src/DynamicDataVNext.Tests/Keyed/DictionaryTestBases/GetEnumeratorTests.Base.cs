using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

public static partial class GetEnumeratorTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IReadOnlyDictionaryUutFixture<TUutFixture, TUut>
        where TUut : IReadOnlyDictionary<string, int>
    {
        [TestCaseSource(typeof(GetEnumeratorTests), nameof(Always_TestCases))]
        public void Always_EnumerationMatchesItems(IReadOnlyList<KeyValuePair<string, int>> items)
        {
            using var fixture = TUutFixture.Create(items: items);

            fixture.Uut.Should().BeEquivalentTo(items, "all items in the set should be enumerated");
        }
    }
}
