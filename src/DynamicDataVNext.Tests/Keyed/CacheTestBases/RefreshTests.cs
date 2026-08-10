namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class RefreshTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenCacheContainsItem_TestCases
        = new[]
        {
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "1" } },
                    Item            = new() { Key = "1" }
                })
                .SetName("{m}(Single item in dictionary)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new TestItem[]
                    {
                        new() { Key = "1" },
                        new() { Key = "2" },
                        new() { Key = "3" }
                    },
                    Item            = new() { Key = "2" }
                })
                .SetName("{m}(Multiple items in dictionary)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenCacheDoesNotContainItem_TestCases
        = new[]
        {
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = Array.Empty<TestItem>(),
                    Item            = new() { Key = "1" }
                })
                .SetName("{m}(Empty dictionary)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "1" } },
                    Item            = new() { Key = "1", Version = 1 }
                })
                .SetName("{m}(Single item in dictionary, same key)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "1" } },
                    Item            = new() { Key = "2" }
                })
                .SetName("{m}(Single item in dictionary, different key)"),
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
                .SetName("{m}(Multiple items in dictionary)")
        };
}
