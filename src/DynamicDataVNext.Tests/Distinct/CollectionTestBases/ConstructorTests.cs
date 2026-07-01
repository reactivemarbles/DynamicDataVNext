using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public static partial class ConstructorTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenItemsIsNotNull_TestCases
        = new[]
        {
            new TestCaseData(Array.Empty<int>())    .SetName("{m}(Empty items)"),
            new TestCaseData(new[] { 1 })           .SetName("{m}(Single item)"),
            new TestCaseData(new[] { 1, 2, 3, })    .SetName("{m}(Multiple items)")
        };
}
