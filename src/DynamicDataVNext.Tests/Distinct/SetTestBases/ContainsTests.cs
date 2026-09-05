namespace DynamicDataVNext.Tests.Distinct.SetTestBases;

public static partial class ContainsTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenItemIsInSet_TestCases
        = new[]
        {
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    Items   = new[] { 1 },
                    Item    = 1
                })
                .SetName("{m}(Single item in collection)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    Items   = new[] { 1, 2, 3 },
                    Item    = 2
                })
                .SetName("{m}(Multiple items in collection)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenItemIsNotInSet_TestCases
        = new[]
        {
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    Items   = Array.Empty<int>(),
                    Item    = 1 
                })
                .SetName("{m}(Empty set)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    Items   = new[] { 1 },
                    Item    = 2
                })
                .SetName("{m}(Single item in set)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    Items   = new[] { 1, 2, 3 },
                    Item    = 4
                })
                .SetName("{m}(Multiple items in set)")
        };
}
