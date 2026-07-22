using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed;

public static partial class RemoveTests
{
    public abstract class ForKeyBase<TUutFixture, TUut>
        where TUutFixture : IDictionaryUutFixture<TUutFixture, TUut>
        where TUut : IDictionary<string, int>
    {
        [TestCaseSource(typeof(RemoveTests), nameof(WhenDictionaryContainsKey_TestCases))]
        public void WhenDictionaryContainsKey_RemovesItemAndReturnsTrue(SingleKeyOperationTestCase testCase)
        {
            var fixture = TUutFixture.Create(items: testCase.InitialItems);
            
            var result = fixture.Uut.Remove(testCase.Key);
            
            result.Should().BeTrue("the collection contained the given key");
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems.Where(item => item.Key != testCase.Key), "the item with the given key should have been removed from the collection");

            if (testCase.InitialItems.Count is 1)
                fixture.AssertUutWasCleared(testCase.InitialItems);
            else
                fixture.AssertItemWasRemoved(
                    removedKey:     testCase.Key,
                    removedValue:   testCase.InitialItems.First(item => item.Key == testCase.Key).Value);
        }
        
        [TestCaseSource(typeof(RemoveTests), nameof(WhenDictionaryDoesNotContainKey_TestCases))]
        public void WhenDictionaryDoesNotContainKey_DoesNothingAndReturnsFalse(SingleKeyOperationTestCase testCase)
        {
            var fixture = TUutFixture.Create(items: testCase.InitialItems);
            
            var result = fixture.Uut.Remove(testCase.Key);
            
            result.Should().BeFalse("the collection did not contain the given key");
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }
        
        [Test]
        public void WhenKeyIsNull_DoesNothingAndThrowsException()
        {
            using var fixture = TUutFixture.Create(new[] { new KeyValuePair<string, int>("1", 1) });

            var result = FluentActions.Invoking(() =>
                {
                    _ = fixture.Uut.Remove(null!);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("key")
                .Which;
            
            Console.WriteLine(result);
            
            fixture.AssertUutDidNothing();
        }
    }
}
