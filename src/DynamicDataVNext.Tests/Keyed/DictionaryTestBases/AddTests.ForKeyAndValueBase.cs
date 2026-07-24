using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

public static partial class AddTests
{
    public abstract class ForKeyAndValueBase<TUutFixture, TUut>
            : Base<TUutFixture, TUut>
        where TUutFixture : IDictionaryUutFixture<TUutFixture, TUut>
        where TUut : IDictionary<string, int>
    {
        [TestCaseSource(typeof(AddTests), nameof(WhenKeyIsInDictionary_TestCases))]
        public void WhenKeyIsInDictionary_ThrowsException(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
                
            var result = FluentActions.Invoking(() =>
                {
                    fixture.Uut.Add(testCase.Key, testCase.Value);
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName("key")
                .Which;

            Console.WriteLine(result);

            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have been changed");

            fixture.AssertUutDidNothing();
        }
        
        [TestCaseSource(typeof(AddTests), nameof(WhenKeyIsNotInDictionary_TestCases))]
        public void WhenKeyIsNotInDictionary_AddsItem(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
                
            fixture.Uut.Add(testCase.Key, testCase.Value);
            
            fixture.Uut.Keys.Should().Contain(testCase.Key, "the given item should have been added to the set");
            fixture.Uut.Values.Should().Contain(testCase.Value, "the given item should have been added to the set");

            fixture.AssertItemWasAdded(testCase.Key, testCase.Value);
        }
        
        [TestCaseSource(typeof(AddTests), nameof(WhenKeyIsNull_TestCases))]
        public void WhenKeyIsNull_ThrowsException(IReadOnlyList<KeyValuePair<string, int>> initialItems)
        {
            using var fixture = TUutFixture.Create(items: initialItems);
                
            var result = FluentActions.Invoking(() =>
                {
                    fixture.Uut.Add(null!, 1);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("key")
                .Which;

            Console.WriteLine(result);

            fixture.Uut.Should().BeEquivalentTo(initialItems, "the collection should not have been changed");

            fixture.AssertUutDidNothing();
        }

        protected override void AddItem(
                TUut    uut,
                string  key,
                int     value)
            => uut.Add(key, value);
    }
}
