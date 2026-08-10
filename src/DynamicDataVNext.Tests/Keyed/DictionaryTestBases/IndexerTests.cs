namespace DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

public static partial class IndexerTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenKeyIsNull_TestCases
        = new[]
        {
            new TestCaseData(new[] { new KeyValuePair<string, int>("1", 1) })
                .SetName("{m}(Single item in collection)"),
            new TestCaseData(new KeyValuePair<string, int>[]
                {
                    new("1", 1),
                    new("2", 2),
                    new("3", 3)
                })
                .SetName("{m}(Multiple items in collection)")
        };
}
