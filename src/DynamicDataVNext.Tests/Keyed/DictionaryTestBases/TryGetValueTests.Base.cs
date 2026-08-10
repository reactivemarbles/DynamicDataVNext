namespace DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

public static partial class TryGetValueTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IReadOnlyDictionaryUutFixture<TUutFixture, TUut>
        where TUut : IReadOnlyDictionary<string, int>
    {
        [TestCaseSource(typeof(TryGetValueTests), nameof(WhenKeyIsInDictionary_TestCases))]
        public void WhenKeyIsInDictionary_ReturnsTrueAndMatchingValue(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(testCase.InitialItems);

            var result = fixture.Uut.TryGetValue(testCase.Key, out var resultValue);

            result.Should().BeTrue("the dictionary contains the given key");
            resultValue.Should().Be(testCase.Value, "the value matching the given key should have been retrieved");
        }

        [TestCaseSource(typeof(TryGetValueTests), nameof(WhenKeyIsNotInDictionary_TestCases))]
        public void WhenKeyIsNotInDictionary_ReturnsFalseAndDefaultValue(SingleItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(testCase.InitialItems);

            var result = fixture.Uut.TryGetValue(testCase.Key, out var resultValue);

            result.Should().BeFalse("the dictionary does not contain the given key");
            resultValue.Should().Be(default, "the default value should have been retrieved");
        }

        [Test]
        public void WhenKeyIsNull_ThrowsException()
        {
            using var fixture = TUutFixture.Create(Array.Empty<KeyValuePair<string, int>>());

            var result = FluentActions.Invoking(() =>
                {
                    _ = fixture.Uut.TryGetValue(null!, out _);
                })
                .Should().Throw<ArgumentException>()
                .WithParameterName("key")
                .Which;
            
            Console.WriteLine(result);
        }
    }
}
