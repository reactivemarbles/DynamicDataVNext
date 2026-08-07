using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class RefreshKeyTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenCacheContainsKey_TestCases
        = new[]
        {
            new TestCaseData(new SingleKeyOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "1" } },
                    Key             = "1"
                })
                .SetName("{m}(Single item in dictionary)"),
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
                .SetName("{m}(Multiple items in dictionary)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenCacheDoesNotContainKey_TestCases
        = new[]
        {
            new TestCaseData(new SingleKeyOperationTestCase()
                {
                    InitialItems    = Array.Empty<TestItem>(),
                    Key             = "1"
                })
                .SetName("{m}(Empty dictionary)"),
            new TestCaseData(new SingleKeyOperationTestCase()
                {
                    InitialItems    = new[] { new TestItem() { Key = "1" } },
                    Key             = "2"
                })
                .SetName("{m}(Single item in dictionary)"),
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
                .SetName("{m}(Multiple items in dictionary)")
        };
}
