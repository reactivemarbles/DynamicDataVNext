using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

public static partial class RefreshTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IDictionaryUutFixture<TUutFixture, TUut>
        where TUut : IDictionary<string, int>
    {
        [TestCaseSource(typeof(RefreshTests), nameof(WhenDictionaryContainsKey_TestCases))]
        public void WhenDictionaryContainsKey_RefreshesItemAndReturnsTrue(SingleKeyOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                items:      testCase.InitialItems,
                options:    new()
                {
                    ItemsAreMutable = true
                });
            
            var result = fixture.RefreshUut(testCase.Key);
            
            result.Should().BeTrue("the collection contains the given key");
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have changed");
            
            fixture.AssertItemWasRefreshed(
                refreshedKey:   testCase.Key,
                refreshedValue: fixture.Uut[testCase.Key]);
        }
        
        [TestCaseSource(typeof(RefreshTests), nameof(WhenDictionaryDoesNotContainKey_TestCases))]
        public void WhenDictionaryDoesNotContainKey_DoesNothingAndReturnsFalse(SingleKeyOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(
                items:      testCase.InitialItems,
                options:    new()
                {
                    ItemsAreMutable = true
                });
            
            var result = fixture.RefreshUut(testCase.Key);
            
            result.Should().BeFalse("the collection does not contain the given key");
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have changed");

            fixture.AssertUutDidNothing();
        }

        [Test]
        public void WhenItemKeyIsNull_DoesNothingAndThrowsException()
        {
            using var fixture = TUutFixture.Create(
                items:      new[] { new KeyValuePair<string, int>("1", 1) },
                options:    new()
                {
                    ItemsAreMutable = true
                });

            var result = FluentActions.Invoking(() =>
                {
                    _ = fixture.RefreshUut(null!);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("key")
                .Which;
            
            Console.WriteLine(result);
            
            fixture.AssertUutDidNothing();
        }

        [Test]
        public void WhenItemsAreNotMutable_DoesNothingAndThrowsException()
        {
            using var fixture = TUutFixture.Create(
                items:      new[] { new KeyValuePair<string, int>("1", 1) },
                options:    new()
                {
                    ItemsAreMutable = false
                });

            var result = FluentActions.Invoking(() =>
                {
                    _ = fixture.RefreshUut("1");
                })
                .Should().Throw<ImmutableRefreshException>()
                .Which;
            
            Console.WriteLine(result);
            
            fixture.AssertUutDidNothing();
        }
    }
}
