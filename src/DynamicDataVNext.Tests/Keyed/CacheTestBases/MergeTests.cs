using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class MergeTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenDictionaryContainsKeyWithDifferentItem_TestCases
        = new[]
        {
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "1", Version = 0 } },
                    Item            = new TestItem() { Key = "1", Version = 1 }
                })
                .SetName("{m}(Single item in collection)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new TestItem[]
                    {
                        new() { Key = "1", Version = 0 },
                        new() { Key = "2", Version = 0 },
                        new() { Key = "3", Version = 0 }
                    },
                    Item            = new TestItem() { Key = "2", Version = 1 }
                })
                .SetName("{m}(Multiple items in collection)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenDictionaryContainsKeyWithSameItem_TestCases
        = new[]
        {
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "1" } },
                    Item            = new TestItem() { Key = "1" }
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
                    Item            = new TestItem() { Key = "2" }
                })
                .SetName("{m}(Multiple items in collection)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenDictionaryDoesNotContainKey_TestCases
        = new[]
        {
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = Array.Empty<TestItem>(),
                    Item            = new TestItem() { Key = "1" }
                })
                .SetName("{m}(Empty collection)"),
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
}
