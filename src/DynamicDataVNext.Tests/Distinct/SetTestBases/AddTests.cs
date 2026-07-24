using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.SetTestBases;

public static partial class AddTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenItemIsInSet_TestCases
        = new[]
        {
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    Item    = 1,
                    Items   = new[] { 1 }
                })
                .SetName("{m}(Single item in collection)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    Item    = 1,
                    Items   = new[] { 1, 2, 3 }
                })
                .SetName("{m}(Multiple items in collection)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenItemIsNotInSet_TestCases
        = new[]
        {
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    Item    = 2,
                    Items   = new[] { 1 }
                })
                .SetName("{m}(Single item in collection)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    Item    = 4,
                    Items   = new[] { 1, 2, 3 }
                })
                .SetName("{m}(Multiple items in collection)")
        };
}
