using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public static partial class SetEqualsTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenSetDoesNotEqualOther_TestCases
        = new[]
        {
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "one set is empty and the other is not",
                    Items   = Array.Empty<int>(),
                    Other   = new[] { 1 }
                })
                .SetName("{m}(Empty set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "one set is empty and the other is not",
                    Items   = new[] { 1 },
                    Other   = Array.Empty<int>()
                })
                .SetName("{m}(Empty other set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "each set contains only an item not in the other set",
                    Items   = new[] { 1 },
                    Other   = new[] { 2 }
                })
                .SetName("{m}(Sets do not overlap)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the first set is missing items in the second set",
                    Items   = new[] { 2 },
                    Other   = new[] { 1, 2, 3 }
                })
                .SetName("{m}(Set is subset of other set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the first contains items not in the second set",
                    Items = new[] { 1, 2, 3 },
                    Other = new[] { 2 }
                })
                .SetName("{m}(Set is superset of other set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "each set contains an item not in the other set",
                    Items = new[] { 1, 2, 3 },
                    Other = new[] { 2, 3, 4 }
                })
                .SetName("{m}(Set partially overlaps other set)")
        };
    
    public static readonly IReadOnlyList<TestCaseData> WhenSetEqualsOther_TestCases
        = new[]
        {
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "twe two sets are empty",
                    Items   = Array.Empty<int>(),
                    Other   = Array.Empty<int>()
                })
                .SetName("{m}(Single item in set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "twe two sets are equal",
                    Items   = new[] { 1 },
                    Other   = new[] { 1 }
                })
                .SetName("{m}(Single item in set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "twe two sets are equal",
                    Items   = new[] { 1, 2, 3, 4, 5 },
                    Other   = new[] { 1, 2, 3, 4, 5 }
                })
                .SetName("{m}(Multiple items in set)")
        };
}
