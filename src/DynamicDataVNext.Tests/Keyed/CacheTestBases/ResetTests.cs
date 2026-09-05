namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class ResetTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenCacheIsNotEmpty_TestCases
        = new[]
        {
            new TestCaseData((object?)new TestItem[] { new() { Key = "1" } })
                .SetName("{m}(Single item in collection)"),
            new TestCaseData((object?)new TestItem[]
                {
                    new() { Key = "1" },
                    new() { Key = "2" },
                    new() { Key = "3" }
                })
                .SetName("{m}(Multiple items in collection)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenItemsIsNotEmpty_TestCases
        = new[]
        {
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = Array.Empty<TestItem>(),
                    Items           = new TestItem[] { new() { Key = "1" } } 
                })
                .SetName("{m}(Collection is empty, Single item in reset)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = Array.Empty<TestItem>(),
                    Items           = new TestItem[]
                    {
                        new() { Key = "1" },
                        new() { Key = "2" },
                        new() { Key = "3" }
                    } 
                })
                .SetName("{m}(Collection is empty, Multiple items in reset)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new TestItem[] { new() { Key = "1" } },
                    Items           = new TestItem[] { new() { Key = "2" } } 
                })
                .SetName("{m}(Single item in collection, Single item in reset)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new TestItem[] { new() { Key = "1" } },
                    Items           = new TestItem[]
                    {
                        new() { Key = "2" },
                        new() { Key = "3" },
                        new() { Key = "4" }
                    } 
                })
                .SetName("{m}(Single item in collection, Multiple items in reset)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new TestItem[]
                    {
                        new() { Key = "1" },
                        new() { Key = "2" },
                        new() { Key = "3" }
                    },
                    Items           = new TestItem[] { new() { Key = "4" } } 
                })
                .SetName("{m}(Multiple items in collection, Single item in reset)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new TestItem[]
                    {
                        new() { Key = "1" },
                        new() { Key = "2" },
                        new() { Key = "3" }
                    },
                    Items           = new TestItem[]
                    {
                        new() { Key = "4" },
                        new() { Key = "5" },
                        new() { Key = "6" }
                    } 
                })
                .SetName("{m}(Multiple items in collection, Multiple items in reset)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new TestItem[]
                    {
                        new() { Key = "1" },
                        new() { Key = "2" },
                        new() { Key = "3" }
                    },
                    Items           = new TestItem[]
                    {
                        new() { Key = "2" },
                        new() { Key = "3" },
                        new() { Key = "4" }
                    } 
                })
                .SetName("{m}(Multiple items in collection, Overlapping items in reset)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenItemsKeysContainsNull_TestCases
        = new[]
        {
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = Array.Empty<TestItem>(),
                    Items           = new TestItem[] { new() { Key = null! } } 
                })
                .SetName("{m}(Collection is empty, Single item in reset)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new TestItem[] { new() { Key = "1" } },
                    Items           = new TestItem[] { new() { Key = null! } } 
                })
                .SetName("{m}(Single item in collection, Single item in reset)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new TestItem[]
                    {
                        new() { Key = "1" },
                        new() { Key = "2" },
                        new() { Key = "3" }
                    },
                    Items           = new TestItem[]
                    {
                        new() { Key = null! },
                        new() { Key = "5" },
                        new() { Key = "6" }
                    }
                })
                .SetName("{m}(Multiple items in collection, Multiple items in reset, null is first)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new TestItem[]
                    {
                        new() { Key = "1" },
                        new() { Key = "2" },
                        new() { Key = "3" }
                    },
                    Items           = new TestItem[]
                    {
                        new() { Key = "4" },
                        new() { Key = "5" },
                        new() { Key = null! }
                    }
                })
                .SetName("{m}(Multiple items in collection, Multiple items in reset, null is last)")
        };
}
