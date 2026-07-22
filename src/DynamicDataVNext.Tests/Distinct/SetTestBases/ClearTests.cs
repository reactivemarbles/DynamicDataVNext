using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public static partial class ClearTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenSetIsNotEmpty_TestCases
        = new[]
        {
            new TestCaseData(new[] { 1 })       .SetName("{m}(Single item in set)"),
            new TestCaseData(new[] { 1, 2, 3 }) .SetName("{m}(Multiple items in set)")
        };
}
