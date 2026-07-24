using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

public static partial class AddTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IDictionaryUutFixture<TUutFixture, TUut>
        where TUut : IDictionary<string, int>
    {
        [Test]
        public void WhenDictionaryIsEmpty_ResetsDictionary()
        {
            using var fixture = TUutFixture.Create();

            const string    key   = "1";
            const int       value = 1;
            
            AddItem(fixture.Uut, key, value);
            
            fixture.Uut.Should().ContainSingle("the set should have been reset to the given item");
            fixture.Uut.Keys.Should().Contain(key, "the set should have been reset to the given item");
            fixture.Uut.Values.Should().Contain(value, "the set should have been reset to the given item");

            fixture.AssertUutWasReset(
                oldItems: Array.Empty<KeyValuePair<string, int>>(),
                newItems: new[] { new KeyValuePair<string, int>(key, value) });
        }
        
        protected abstract void AddItem(
            TUut    uut,
            string  key,
            int     value);
    }
}
