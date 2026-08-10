namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class ConstructorTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenItemsContainsNullKey_TestCases
        = new[]
        {
            new TestCaseData((object?)new[] { new TestItem() { Key = null! } })
                .SetName("{m}(Single item)"),
            new TestCaseData((object?)new TestItem[]
                {
                    new() { Key = null! },
                    new() { Key = "2" },
                    new() { Key = "3" }
                })
                .SetName("{m}(Multiple items, null is first)"),
            new TestCaseData((object?)new TestItem[]
                {
                    new() { Key = "1" },
                    new() { Key = "2" },
                    new() { Key = null! }
                })
                .SetName("{m}(Multiple items, null is last)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenItemsIsNotNull_TestCases
        = new[]
        {
            new TestCaseData((object?)Array.Empty<TestItem>())
                .SetName("{m}(Empty items)"),
            new TestCaseData((object?)new[] { new TestItem() { Key = "1" } })
                .SetName("{m}(Single item)"),
            new TestCaseData((object?)new TestItem[]
                {
                    new() { Key = "1" },
                    new() { Key = "2" },
                    new() { Key = "3" }
                })
                .SetName("{m}(Multiple items)")
        };
}
