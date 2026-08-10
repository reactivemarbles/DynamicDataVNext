namespace DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

public static partial class IndexerTests
{
    public static partial class SetTests
    {
        public abstract class Base<TUutFixture, TUut>
            where TUutFixture : IDictionaryUutFixture<TUutFixture, TUut>
            where TUut : class, IDictionary<string, int>
        {
            [TestCaseSource(typeof(IndexerTests), nameof(WhenKeyIsNull_TestCases))]
            public void WhenKeyIsNull_ThrowsException(IReadOnlyList<KeyValuePair<string, int>> initialItems)
            {
                using var fixture = TUutFixture.Create(initialItems);
                
                var result = fixture.Uut.Invoking(uut =>
                    {
                        uut[null!] = 0;
                    })
                    .Should().Throw<ArgumentNullException>()
                    .WithParameterName("key")
                    .Which;
                    
                Console.WriteLine(result);
            }
            
            [TestCaseSource(typeof(SetTests), nameof(WhenDictionaryContainsKeyWithDifferentValue_TestCases))]
            public void WhenDictionaryContainsKeyWithDifferentValue_ReplacesItem(SingleItemOperationTestCase testCase)
            {
                using var fixture = TUutFixture.Create(testCase.InitialItems);
                
                fixture.Uut[testCase.Key] = testCase.Value;
                
                var finalItems = testCase.InitialItems
                    .Select(item => (item.Key == testCase.Key)
                        ? new KeyValuePair<string, int>(testCase.Key, testCase.Value)
                        : item)
                    .ToArray();

                fixture.Uut.Should().BeEquivalentTo(finalItems, "the value for the given key should have been replaced");
                
                fixture.AssertItemWasReplaced(
                    replacementKey:     testCase.Key,
                    replacedValue:      testCase.InitialItems.First(item => item.Key == testCase.Key).Value,
                    replacementValue:   testCase.Value);
            }

            [TestCaseSource(typeof(SetTests), nameof(WhenDictionaryContainsKeyWithSameValue_TestCases))]
            public void WhenDictionaryContainsKeyWithSameValue_DoesNothing(SingleItemOperationTestCase testCase)
            {
                using var fixture = TUutFixture.Create(testCase.InitialItems);
                
                fixture.Uut[testCase.Key] = testCase.Value;
                
                fixture.Uut.Should().BeEquivalentTo(testCase.InitialItems, "the collection should not have been changed");
                
                fixture.AssertUutDidNothing();
            }

            [TestCaseSource(typeof(SetTests), nameof(WhenDictionaryDoesNotContainKey_TestCases))]
            public void WhenDictionaryDoesNotContainKey_AddsItem(SingleItemOperationTestCase testCase)
            {
                using var fixture = TUutFixture.Create(testCase.InitialItems);
                
                fixture.Uut[testCase.Key] = testCase.Value;
                
                var finalItems = testCase.InitialItems
                    .Append(new KeyValuePair<string, int>(testCase.Key, testCase.Value))
                    .ToArray();

                fixture.Uut.Should().BeEquivalentTo(finalItems, "the given key and value should have been added to the collection");
                
                if (testCase.InitialItems.Count is 0)
                    fixture.AssertUutWasReset(
                        removedItems:   Array.Empty<KeyValuePair<string, int>>(),
                        addedItems:     new[] { new KeyValuePair<string, int>(testCase.Key, testCase.Value) });
                else
                    fixture.AssertItemWasAdded(
                        addedKey:   testCase.Key,
                        addedValue: testCase.Value);
            }
        }
    }
}
