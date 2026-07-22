using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public static partial class IsProperSubsetOfTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenSetIsNotProperSubsetOfOther_TestCases
        = new[]
        {
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the first set contains items not in the second set", 
                    Items   = new[] { 1 },
                    Other   = Array.Empty<int>()
                })
                .SetName("{m}(Empty other set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the two sets are equal",
                    Items   = new[] { 1 },
                    Other   = new[] { 1 }
                })
                .SetName("{m}(Sets are equal)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the first set contains an item not in the second set",
                    Items   = new[] { 1 },
                    Other   = new[] { 2 }
                })
                .SetName("{m}(Sets do not overlap)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the first set contains an item not in the second set",
                    Items   = new[] { 1, 2, 3 },
                    Other   = new[] { 2, 3, 4 }
                })
                .SetName("{m}(Set partially overlaps other set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the first set contains items not in the second set",
                    Items   = new[] { 1, 2, 3 },
                    Other   = new[] { 2 }
                })
                .SetName("{m}(Set is superset of other set)")
        };
    
    public static readonly IReadOnlyList<TestCaseData> WhenSetIsProperSubsetOfOther_TestCases
        = new[]
        {
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the first set is empty and the second set is not", 
                    Items   = Array.Empty<int>(),
                    Other   = new[] { 1 }
                })
                .SetName("{m}(Empty set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the first set contains only an item in the second set, and is not equal to the second set", 
                    Items   = new[] { 2 },
                    Other   = new[] { 1, 2, 3 }
                })
                .SetName("{m}(Single item in set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the first set contains only items in the second set, and is not equal to the second set", 
                    Items   = new[] { 2, 3, 4 },
                    Other   = new[] { 1, 2, 3, 4, 5 }
                })
                .SetName("{m}(Multiple items in set)")
        };
}
