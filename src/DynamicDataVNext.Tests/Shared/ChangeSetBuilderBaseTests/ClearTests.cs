namespace DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

public static partial class ClearTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenSourceIsInitiallyEmpty_TestCases
        = new[]
        {
            //                  initialSourceCount, sourceCount
            new TestCaseData(   0,                  0)          .SetName("{m}(Empty source after Clear)"),
            new TestCaseData(   0,                  1)          .SetName("{m}(Single item in source after Clear)"),
            new TestCaseData(   0,                  3)          .SetName("{m}(Multiple items in source after Clear)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenSourceIsInitiallyNotEmpty_TestCases
        = new[]
        {
            //                  initialSourceCount, sourceCount
            new TestCaseData(   1,                  0)          .SetName("{m}(Single item initially in source, Empty source after Clear)"),
            new TestCaseData(   1,                  1)          .SetName("{m}(Single item initially in source, Single item in source after Clear)"),
            new TestCaseData(   1,                  3)          .SetName("{m}(Single item initially in source, Multiple items in source after Clear)"),
            new TestCaseData(   3,                  0)          .SetName("{m}(Multiple items initially in source, Empty source after Clear)"),
            new TestCaseData(   3,                  1)          .SetName("{m}(Multiple items initially in source, Single item in source after Clear)"),
            new TestCaseData(   3,                  3)          .SetName("{m}(Multiple items initially in source, Multiple items in source after Clear)")
        };
}
