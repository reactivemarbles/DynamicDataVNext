using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.SetTestBases;

public static partial class GetEnumeratorTests
{
    public static readonly IReadOnlyList<TestCaseData> Always_TestCases
        = new[]
        {
            new TestCaseData(Array.Empty<int>()).SetName("{m}(Empty set)"),
            new TestCaseData(new[] { 1 })       .SetName("{m}(Single item in set)"),
            new TestCaseData(new[] { 1, 2, 3 }) .SetName("{m}(Multiple items in set)")
        };
}
