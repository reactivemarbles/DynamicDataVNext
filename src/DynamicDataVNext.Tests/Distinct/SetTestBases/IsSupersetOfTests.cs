using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public static partial class IsSupersetOfTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenSetIsNotSupersetOfOther_TestCases
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
                    Because = "the first set contains no items in the second set", 
                    Items   = new[] { 1 },
                    Other   = new[] { 2 }
                })
                .SetName("{m}(Sets do not overlap)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the first set contains only some items in the second set", 
                    Items = new[] { 1, 2, 3 },
                    Other = new[] { 2, 3, 4 }
                })
                .SetName("{m}(Set partially overlaps other set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the first set contains only one item in the second set", 
                    Items = new[] { 2 },
                    Other = new[] { 1, 2, 3 }
                })
                .SetName("{m}(Set is subset of other set)")
        };
    
    public static readonly IReadOnlyList<TestCaseData> WhenSetIsSupersetOfOther_TestCases
        = new[]
        {
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the first set is not empty and the second set is", 
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
                .SetName("{m}(Sets are equal, single item)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the two sets are equal", 
                    Items   = new[] { 1, 2, 3 },
                    Other   = new[] { 1, 2, 3 }
                })
                .SetName("{m}(Sets are equal, multiple items)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the first set contains the only item in the second set", 
                    Items = new[] { 1, 2, 3 },
                    Other = new[] { 2 }
                })
                .SetName("{m}(Single item in other set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the first set contains all items in the second set", 
                    Items = new[] { 1, 2, 3, 4, 5 },
                    Other = new[] { 2, 3, 4 }
                })
                .SetName("{m}(Multiple items in other set)")
        };
}
