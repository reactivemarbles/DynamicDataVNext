using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public static partial class OverlapsTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenSetDoesNotOverlapOther_TestCases
        = new[]
        {
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the first set is empty",
                    Items   = Array.Empty<int>(),
                    Other   = new[] { 1 }
                })
                .SetName("{m}(Empty set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the second set is empty",
                    Items   = new[] { 1 },
                    Other   = Array.Empty<int>()
                })
                .SetName("{m}(Empty other set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "each set's only item is not in the other set",
                    Items = new[] { 1 },
                    Other = new[] { 2 }
                })
                .SetName("{m}(Sets do not overlap, single item)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "each set contains only items not in the other set",
                    Items = new[] { 1, 2, 3 },
                    Other = new[] { 4, 5, 6 }
                })
                .SetName("{m}(Sets do not overlap, multiple items)")
        };
    
    public static readonly IReadOnlyList<TestCaseData> WhenSetOverlapsOther_TestCases
        = new[]
        {
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
                    Because = "the first set contains only an item in the second set",
                    Items   = new[] { 2 },
                    Other   = new[] { 1, 2, 3 }
                })
                .SetName("{m}(Set is subset of other set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the first set contains the only item in the second set",
                    Items = new[] { 1, 2, 3 },
                    Other = new[] { 2 }
                })
                .SetName("{m}(Set is superset of other set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Because = "the first set contains some of the items in the second set",
                    Items = new[] { 1, 2, 3 },
                    Other = new[] { 2, 3, 4 }
                })
                .SetName("{m}(Set partially overlaps other set)")
        };
}
