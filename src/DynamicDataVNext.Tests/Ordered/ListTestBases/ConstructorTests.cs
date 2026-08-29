namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class ConstructorTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenItemsIsNotNull_TestCases
        = new[]
        {
            new TestCaseData((object?)Array.Empty<string?>())   .SetName("{m}(Empty items)"),
            new TestCaseData((object?)new[] { "1" })            .SetName("{m}(Single item)"),
            new TestCaseData((object?)new[] { "1", "2", "3", }) .SetName("{m}(Multiple items)")
        };
}
