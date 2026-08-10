namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class RemoveTests
{
    public static readonly IReadOnlyList<TestCaseData> InitialItems_TestCases
        = new[]
        {
            new TestCaseData((object?)Array.Empty<TestItem>())
                .SetName("{m}(Empty collection)"),
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

    public static readonly IReadOnlyList<TestCaseData> WhenCacheContainsItem_TestCases
        = new[]
        {
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "1" } },
                    Item            = new() { Key = "1" }
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
                    Item            = new() { Key = "1" }
                })
                .SetName("{m}(Multiple items in collection)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenCacheContainsKey_TestCases
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

    public static readonly IReadOnlyList<TestCaseData> WhenCacheDoesNotContainItem_TestCases
        = new[]
        {
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "1" } },
                    Item            = new() { Key = "2" }
                })
                .SetName("{m}(Single item in collection)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "1" } },
                    Item            = new()
                    {
                        Key     = "1",
                        Version = 1
                    }
                })
                .SetName("{m}(Single item in collection, same key)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new TestItem[]
                    {
                        new() { Key = "1" },
                        new() { Key = "2" },
                        new() { Key = "3" }
                    },
                    Item            = new() { Key = "4" }
                })
                .SetName("{m}(Multiple items in collection)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenCacheDoesNotContainKey_TestCases
        = new[]
        {
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
