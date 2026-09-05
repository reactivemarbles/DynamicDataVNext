namespace DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

public static partial class ResetTests
{
    public abstract class ForItemsBase<TUutFixture, TUut>
        where TUutFixture : IDictionaryUutFixture<TUutFixture, TUut>
        where TUut : IDictionary<string, int>,
            IRangeAwareDictionary<string, int>
    {
        [Test]
        public void WhenItemsAndDictionaryAreEmpty_DoesNothing()
        {
            using var fixture = TUutFixture.Create();
                
            fixture.Uut.Reset(Array.Empty<KeyValuePair<string, int>>());
            
            fixture.Uut.Should().BeEmpty("the dictionary should remain empty");
            
            fixture.AssertUutDidNothing();
        }

        [TestCaseSource(typeof(ResetTests), nameof(WhenItemsContainsNullKey_TestCases))]
        public void WhenItemsContainsNullKey_DoesNothingAndThrowsException(ItemRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
                
            var result = FluentActions.Invoking(() =>
                {
                    fixture.Uut.Reset(testCase.Items);
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName("items")
                .Which;
                
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }
    
        [TestCaseSource(typeof(ResetTests), nameof(WhenDictionaryIsNotEmpty_TestCases))]
        public void WhenItemsIsEmptyAndDictionaryIsNot_ClearsDictionary(IReadOnlyList<KeyValuePair<string, int>> initialItems)
        {
            using var fixture = TUutFixture.Create(initialItems);
                
            fixture.Uut.Reset(Array.Empty<KeyValuePair<string, int>>());
            
            fixture.Uut.Should().BeEmpty("the dictionary should have been cleared");
            
            fixture.AssertUutWasCleared(initialItems);
        }

        [TestCaseSource(typeof(ResetTests), nameof(WhenItemsIsNotEmpty_TestCases))]
        public void WhenItemsIsNotEmpty_ResetsDictionaryToItems(ItemRangeOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.InitialItems);
                
            fixture.Uut.Reset(testCase.Items);
            
            fixture.Uut.Should().BeEquivalentTo(testCase.Items, "the dictionary should have been reset to the given items");
            
            fixture.AssertUutWasReset(
                removedItems:   testCase.InitialItems,
                addedItems:     testCase.Items);
        }

        [TestCaseSource(typeof(ResetTests), nameof(WhenDictionaryIsNotEmpty_TestCases))]
        public void WhenItemsIsNull_DoesNothingAndThrowsException(IReadOnlyList<KeyValuePair<string, int>> initialItems)
        {
            using var fixture = TUutFixture.Create(items: initialItems);
                
            var result = FluentActions.Invoking(() =>
                {
                    fixture.Uut.Reset<IEnumerable<KeyValuePair<string, int>>>(null!);
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName("items")
                .Which;
                
            Console.WriteLine(result);
            
            fixture.Uut.Should().BeEquivalentTo(initialItems, "the collection should not have been changed");
            
            fixture.AssertUutDidNothing();
        }
    }
}
