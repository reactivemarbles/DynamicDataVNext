namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class GetEnumeratorTests
{
    public static readonly IReadOnlyList<TestCaseData> Always_TestCases
        = new[]
        {
            new TestCaseData((object?)Array.Empty<string?>())
                .SetName("{m}(Empty dictionary)"),
            new TestCaseData((object?)new[] { "1" })
                .SetName("{m}(Single item in dictionary)"),
            new TestCaseData((object?)new[] { "1", "2", "3" })
                .SetName("{m}(Multiple items in dictionary)")
        };
}
