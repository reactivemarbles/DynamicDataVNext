namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class RemoveRangeTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenCacheContainsAnyOfItems_TestCases
        = new[]
        {
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new TestItem[] { new() { Key = "1" } },
                    Items           = new TestItem[] { new() { Key = "1" } }
                })
                .SetName("{m}(Single item in collection, Single item to remove)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new TestItem[] { new() { Key = "2" } },
                    Items           = new TestItem[]
                    {
                        new() { Key = "1" },
                        new() { Key = "2" },
                        new() { Key = "3" }
                    }
                })
                .SetName("{m}(Single item in collection, Multiple items to remove)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new TestItem[]
                    {
                        new() { Key = "1" },
                        new() { Key = "2" },
                        new() { Key = "3" }
                    },
                    Items           = new TestItem[] { new() { Key = "2" } }
                })
                .SetName("{m}(Multiple items in collection, Single item to remove)"),
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
                        new() { Key = "1" },
                        new() { Key = "2" },
                        new() { Key = "3" }
                    }
                })
                .SetName("{m}(Multiple items in collection, Same items to remove)"),
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
                .SetName("{m}(Multiple items in collection, Overlapping items to remove)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new TestItem[]
                    {
                        new() { Key = "1" },
                        new() { Key = "2" },
                        new() { Key = "3" },
                        new() { Key = "4" },
                        new() { Key = "5" }
                    },
                    Items           = new TestItem[]
                    {
                        new() { Key = "2" },
                        new() { Key = "3" },
                        new() { Key = "4" }
                    }
                })
                .SetName("{m}(Multiple items in collection, Subset of items to remove)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new TestItem[]
                    {
                        new() { Key = "2" },
                        new() { Key = "3" },
                        new() { Key = "4" }
                    },
                    Items           = new TestItem[]
                    {
                        new() { Key = "1" },
                        new() { Key = "2" },
                        new() { Key = "3" },
                        new() { Key = "4" },
                        new() { Key = "5" }
                    }
                })
                .SetName("{m}(Multiple items in collection, Superset of items to remove)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenCacheContainsNoneOfItems_TestCases
        = new[]
        {
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new TestItem[] { new() { Key = "1" } },
                    Items           = new TestItem[] { new() { Key = "2" } }
                })
                .SetName("{m}(Single item in collection, Single item to remove)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new TestItem[] { new() { Key = "1" } },
                    Items           = new TestItem[] { new() { Key = "1", Version = 1 } }
                })
                .SetName("{m}(Single item in collection, Single item to remove, same key)"),
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
                .SetName("{m}(Single item in collection, Multiple items to remove)"),
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
                .SetName("{m}(Multiple items in collection, Single item to remove)"),
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
                .SetName("{m}(Multiple items in collection, Multiple items to remove)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenItemsKeysContainsNull_TestCases
        = new[]
        {
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new TestItem[] { new() { Key = "1" } },
                    Items           = new TestItem[] { new() { Key = null! } }
                })
                .SetName("{m}(Single item in collection, Single item to remove)"),
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
                        new() { Key = "2" },
                        new() { Key = "3" }
                    }
                })
                .SetName("{m}(Multiple items in collection, Multiple items to remove, null is first)"),
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
                        new() { Key = "1" },
                        new() { Key = "2" },
                        new() { Key = null! }
                    }
                })
                .SetName("{m}(Multiple items in collection, Multiple items to remove, null is last)")
        };
}
