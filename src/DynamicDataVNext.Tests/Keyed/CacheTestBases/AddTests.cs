namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class AddTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenItemKeyIsInCache_TestCases
        = new[]
        {
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "1" } },
                    Item            = new TestItem()
                    {
                        Key     = "1",
                        Version = 1
                    }
                })
                .SetName("{m}(Single item in collection)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new TestItem[]
                    {
                        new() { Key = "1" },
                        new() { Key = "2" },
                        new() { Key = "3" }
                    },
                    Item            = new TestItem()
                    {
                        Key     = "1",
                        Version = 1
                    }
                })
                .SetName("{m}(Multiple items in collection)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenCacheIsNotEmptyAndItemKeyIsNotInCache_TestCases
        = new[]
        {
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "1" } },
                    Item            = new TestItem() { Key = "2" }
                })
                .SetName("{m}(Single item in collection)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new TestItem[]
                    {
                        new() { Key = "1" },
                        new() { Key = "2" },
                        new() { Key = "3" }
                    },
                    Item            = new TestItem() { Key = "4" }
                })
                .SetName("{m}(Multiple items in collection)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenItemKeyIsNull_TestCases
        = new[]
        {
            new TestCaseData((object?)new[] { new TestItem() { Key = "1" } })
                .SetName("{m}(Single item in collection)"),
            new TestCaseData((object?)new TestItem[]
                {
                    new() { Key = "1" },
                    new() { Key = "2" },
                    new() { Key = "3" }
                })
                .SetName("{m}(Multiple items in collection)")
        };
}
