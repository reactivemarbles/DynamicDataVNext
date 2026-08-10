namespace DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

public static partial class ConstructorTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenItemsContainsNullKey_TestCases
        = new[]
        {
            new TestCaseData(new[] { new KeyValuePair<string, int>(null!, 1) })
                .SetName("{m}(Single item)"),
            new TestCaseData(new KeyValuePair<string, int>[]
                {
                    new(null!,  1),
                    new("2",    2),
                    new("3",    3)
                })
                .SetName("{m}(Multiple items, null is first)"),
            new TestCaseData(new KeyValuePair<string, int>[]
                {
                    new("1",    1),
                    new("2",    2),
                    new(null!,  3)
                })
                .SetName("{m}(Multiple items, null is last)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenItemsIsNotNull_TestCases
        = new[]
        {
            new TestCaseData(Array.Empty<KeyValuePair<string, int>>())
                .SetName("{m}(Empty items)"),
            new TestCaseData(new[] { new KeyValuePair<string, int>("1", 1) })
                .SetName("{m}(Single item)"),
            new TestCaseData(new KeyValuePair<string, int>[]
                {
                    new("1", 1),
                    new("2", 2),
                    new("3", 3)
                })
                .SetName("{m}(Multiple items)")
        };
}
