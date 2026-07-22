using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed;

public static partial class GetEnumeratorTests
{
    public static readonly IReadOnlyList<TestCaseData> Always_TestCases
        = new[]
        {
            new TestCaseData(Array.Empty<KeyValuePair<string, int>>())
                .SetName("{m}(Empty dictionary)"),
            new TestCaseData(new[] { new KeyValuePair<string, int>("1", 1) })
                .SetName("{m}(Single item in dictionary)"),
            new TestCaseData(new KeyValuePair<string, int>[]
                {
                    new("1", 1),
                    new("2", 2),
                    new("3", 3)
                })
                .SetName("{m}(Multiple items in dictionary)")
        };
}
