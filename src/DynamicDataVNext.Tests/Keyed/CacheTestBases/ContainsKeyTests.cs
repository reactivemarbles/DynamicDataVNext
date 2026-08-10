namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class ContainsKeyTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenKeyIsInCache_TestCases
        = new[]
        {
            new TestCaseData(new SingleKeyOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "1" } },
                    Key             = "1"
                })
                .SetName("{m}(Single item in collection)"),
            new TestCaseData(new SingleKeyOperationTestCase()
                {
                    InitialItems    = new TestItem[]
                    {
                        new() { Key = "1" },
                        new() { Key = "2" },
                        new() { Key = "3" }
                    },
                    Key             = "2"
                })
                .SetName("{m}(Multiple items in collection)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenKeyIsNotInCache_TestCases
        = new[]
        {
            new TestCaseData(new SingleKeyOperationTestCase()
                {
                    InitialItems    = Array.Empty<TestItem>(),
                    Key             = "1"
                })
                .SetName("{m}(Empty collection)"),
            new TestCaseData(new SingleKeyOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "1" } },
                    Key             = "2"
                })
                .SetName("{m}(Single item in collection)"),
            new TestCaseData(new SingleKeyOperationTestCase()
                {
                    InitialItems    = new TestItem[]
                    {
                        new() { Key = "1" },
                        new() { Key = "2" },
                        new() { Key = "3" }
                    },
                    Key             = "4"
                })
                .SetName("{m}(Multiple items in collection)")
        };
}
