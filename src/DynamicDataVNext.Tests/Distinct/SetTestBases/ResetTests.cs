using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.SetTestBases;

public static partial class ResetTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenItemsIsNotEmpty_TestCases
        = new[]
        {
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = Array.Empty<int>(),
                    Other = new[] { 1 }
                })
                .SetName("{m}(Empty set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 1 },
                    Other = new[] { 2 }
                })
                .SetName("{m}(Single item in set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 1, 2, 3 },
                    Other = new[] { 2, 3, 4 }
                })
                .SetName("{m}(Multiple items in set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 1 },
                    Other = new[] { 1 }
                })
                .SetName("{m}(Redundant reset)")
        };
    
    public static readonly IReadOnlyList<TestCaseData> WhenItemsIsEmptyAndSetIsNot_TestCases
        = new[]
        {
            new TestCaseData(new[] { 1 })       .SetName("{m}(Single item in reset)"),
            new TestCaseData(new[] { 1, 2, 3 }) .SetName("{m}(Multiple items in reset)")
        };
}
