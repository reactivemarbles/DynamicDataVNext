using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class MergeRangeTests
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

    public static readonly IReadOnlyList<TestCaseData> WhenCacheIsEmptyAndItemsIsNot_TestCases
        = new[]
        {
            new TestCaseData((object?)new[] { new TestItem() { Key = "1" } })
                .SetName("{m}(Single item to add)"),
            new TestCaseData((object?)new TestItem[]
                {
                    new() { Key = "1" },
                    new() { Key = "2" },
                    new() { Key = "3" }
                })
                .SetName("{m}(Multiple items to add)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenItemsIsNotSubsetOfCache_TestCases
        = new[]
        {
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "1" } },
                    Items           = new[] { new TestItem() { Key = "1", Version = 1 } }
                })
                .SetName("{m}(Single item in collection, Different item to add with same key)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "1" } },
                    Items           = new[] { new TestItem() { Key = "2" } }
                })
                .SetName("{m}(Single item in collection, Different item to add with different key)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "2" } },
                    Items           = new TestItem[]
                    {
                        new() { Key = "1" },
                        new() { Key = "2" },
                        new() { Key = "3" }
                    }
                })
                .SetName("{m}(Single item in collection, Multiple items to add, with overlap)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "1" } },
                    Items           = new TestItem[]
                    {
                        new() { Key = "2" },
                        new() { Key = "3" },
                        new() { Key = "4" }
                    }
                })
                .SetName("{m}(Single item in collection, Multiple items to add, without overlap)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new TestItem[]
                    {
                        new() { Key = "1" },
                        new() { Key = "2" },
                        new() { Key = "3" }
                    },
                    Items           = new[] { new TestItem() { Key = "4" } }
                })
                .SetName("{m}(Multiple items in collection, Single item to add, without overlap)"),
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
                        new() { Key = "3" },
                        new() { Key = "4" },
                        new() { Key = "5" }
                    }
                })
                .SetName("{m}(Multiple items in collection, Multiple items to add, with overlap)"),
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
                .SetName("{m}(Multiple items in collection, Multiple items to add, without overlap)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenItemsIsSubsetOfCache_TestCases
        = new[]
        {
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = Array.Empty<TestItem>(),
                    Items           = Array.Empty<TestItem>()
                })
                .SetName("{m}(Empty collection, no items to add)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "1" } },
                    Items           = Array.Empty<TestItem>()
                })
                .SetName("{m}(Single item in collection, no items to add)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "1" } },
                    Items           = new[] { new TestItem() { Key = "1" } }
                })
                .SetName("{m}(Single item in collection, Same item to add)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new TestItem[]
                    {
                        new() { Key = "1" },
                        new() { Key = "2" },
                        new() { Key = "3" }
                    },
                    Items           = Array.Empty<TestItem>()
                })
                .SetName("{m}(Multiple items in collection, no items to add)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new TestItem[]
                    {
                        new() { Key = "1" },
                        new() { Key = "2" },
                        new() { Key = "3" }
                    },
                    Items           = new[] { new TestItem() { Key = "2" } }
                })
                .SetName("{m}(Multiple items in collection, Single item to add)"),
        };

    public static readonly IReadOnlyList<TestCaseData> WhenItemsKeysContainsNull_TestCases
        = new[]
        {
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "1" } },
                    Items           = new[] { new TestItem() { Key = null! } }
                })
                .SetName("{m}(Single item to add)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "1" } },
                    Items           = new TestItem[]
                    {
                        new() { Key = null! },
                        new() { Key = "3" },
                        new() { Key = "4" }
                    }
                })
                .SetName("{m}(Multiple items to add, null key is first)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "1" } },
                    Items           = new TestItem[]
                    {
                        new() { Key = "2" },
                        new() { Key = "3" },
                        new() { Key = null! }
                    }
                })
                .SetName("{m}(Multiple items to add, null key is last)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new TestItem[]
                    {
                        new() { Key = "1" },
                        new() { Key = "2" },
                        new() { Key = "3" }
                    },
                    Items           = new[] { new TestItem() { Key = null! } }
                })
                .SetName("{m}(Multiple items in collection)")
        };
}
