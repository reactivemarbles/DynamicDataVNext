namespace DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

public static partial class IndexerTests
{
    public static partial class GetTests
    {
        public abstract class Base<TUutFixture, TUut>
            where TUutFixture : IReadOnlyDictionaryUutFixture<TUutFixture, TUut>
            where TUut : class, IReadOnlyDictionary<string, int>
        {
            [TestCaseSource(typeof(IndexerTests), nameof(WhenKeyIsNull_TestCases))]
            public void WhenKeyIsNull_ThrowsException(IReadOnlyList<KeyValuePair<string, int>> initialItems)
            {
                using var fixture = TUutFixture.Create(initialItems);
                
                var result = fixture.Uut.Invoking(uut =>
                    {
                        _ = uut[null!];
                    })
                    .Should().Throw<ArgumentNullException>()
                    .WithParameterName("key")
                    .Which;
                    
                Console.WriteLine(result);
            }
            
            [TestCaseSource(typeof(GetTests), nameof(WhenDictionaryContainsKey_TestCases))]
            public void WhenDictionaryContainsKey_ReturnsMatchingValue(SingleKeyOperationTestCase testCase)
            {
                using var fixture = TUutFixture.Create(testCase.InitialItems);
                
                var result = fixture.Uut[testCase.Key];
                
                result.Should().Be(testCase.InitialItems.First(item => item.Key == testCase.Key).Value, "the value in the collection for the given key should have been retrieved");
            }

            [TestCaseSource(typeof(GetTests), nameof(WhenDictionaryDoesNotContainKey_TestCases))]
            public void WhenDictionaryDoesNotContainKey_ThrowsException(SingleKeyOperationTestCase testCase)
            {
                using var fixture = TUutFixture.Create(testCase.InitialItems);
                
                var result = fixture.Uut.Invoking(uut =>
                    {
                        _ = uut[testCase.Key];
                    })
                    .Should().Throw<KeyNotFoundException>()
                    .Which;

                Console.WriteLine(result);
            }
        }
    }
}
