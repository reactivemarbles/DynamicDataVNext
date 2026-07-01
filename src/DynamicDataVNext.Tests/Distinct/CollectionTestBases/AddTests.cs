using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public static partial class AddTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenItemIsInSet_TestCases
        = new[]
        {
            new TestCaseData(new ItemOperationTestCase()
                {
                    Item    = 1,
                    Items   = new[] { 1 }
                })
                .SetName("{m}(Single item in collection)"),
            new TestCaseData(new ItemOperationTestCase()
                {
                    Item    = 1,
                    Items   = new[] { 1, 2, 3 }
                })
                .SetName("{m}(Multiple items in collection)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenItemIsNotInSet_TestCases
        = new[]
        {
            new TestCaseData(new ItemOperationTestCase()
                {
                    Item    = 2,
                    Items   = new[] { 1 }
                })
                .SetName("{m}(Single item in collection)"),
            new TestCaseData(new ItemOperationTestCase()
                {
                    Item    = 4,
                    Items   = new[] { 1, 2, 3 }
                })
                .SetName("{m}(Multiple items in collection)")
        };
}
