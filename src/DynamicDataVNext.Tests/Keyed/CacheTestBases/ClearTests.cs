using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.CacheTestBases;

public static partial class ClearTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenCacheIsNotEmpty_TestCases
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
