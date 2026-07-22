using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public static partial class IsProperSupersetOfTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenSetIsNotProperSupersetOfOther_TestCases
        = new[]
        {
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the first set does not contain the second set's item", 
                    Items   = Array.Empty<int>(),
                    Other   = new[] { 1 }
                })
                .SetName("{m}(Empty set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the two sets are equal",
                    Items   = new[] { 1 },
                    Other   = new[] { 1 }
                })
                .SetName("{m}(Sets are equal)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the first set does not contain the second set's item",
                    Items   = new[] { 1 },
                    Other   = new[] { 2 }
                })
                .SetName("{m}(Sets do not overlap)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the first set does not contain an item in the second set",
                    Items   = new[] { 1, 2, 3 },
                    Other   = new[] { 2, 3, 4 }
                })
                .SetName("{m}(Set partially overlaps other set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the first set is missing some items from the second set", 
                    Items   = new[] { 2 },
                    Other   = new[] { 1, 2, 3 }
                })
                .SetName("{m}(Set is proper subset of other set)")
        };
    
    public static readonly IReadOnlyList<TestCaseData> WhenSetIsProperSupersetOfOther_TestCases
        = new[]
        {
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the second set is empty", 
                    Items   = new[] { 1 },
                    Other   = Array.Empty<int>()
                })
                .SetName("{m}(Empty other set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the first set contains the only item in the second set, along with other items",
                    Items   = new[] { 1, 2, 3 },
                    Other   = new[] { 2 }
                })
                .SetName("{m}(Single item in other set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the first set contains all items in the second set, along with other items",
                    Items   = new[] { 1, 2, 3, 4, 5 },
                    Other   = new[] { 2, 3, 4 }
                })
                .SetName("{m}(Single item in other set)")
        };
}
