namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class ContainsTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenItemIsInCache_TestCases
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
                    Item            = new() { Key = "2" }
                })
                .SetName("{m}(Multiple items in collection)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenItemIsNotInCache_TestCases
        = new[]
        {
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = Array.Empty<TestItem>(),
                    Item            = new() { Key = "1" }
                })
                .SetName("{m}(Empty collection)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "1" } },
                    Item            = new() { Key = "1", Version = 1 }
                })
                .SetName("{m}(Single item in collection, same key)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "1" } },
                    Item            = new() { Key = "2" }
                })
                .SetName("{m}(Single item in collection, different key)"),
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
}
