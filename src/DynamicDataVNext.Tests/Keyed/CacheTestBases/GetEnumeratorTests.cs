namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class GetEnumeratorTests
{
    public static readonly IReadOnlyList<TestCaseData> Always_TestCases
        = new[]
        {
            new TestCaseData((object?)Array.Empty<TestItem>())
                .SetName("{m}(Empty dictionary)"),
            new TestCaseData((object?)new[] { new TestItem() { Key = "1" } })
                .SetName("{m}(Single item in dictionary)"),
            new TestCaseData((object?)new TestItem[]
                {
                    new() { Key = "1" },
                    new() { Key = "2" },
                    new() { Key = "3" }
                })
                .SetName("{m}(Multiple items in dictionary)")
        };
}
