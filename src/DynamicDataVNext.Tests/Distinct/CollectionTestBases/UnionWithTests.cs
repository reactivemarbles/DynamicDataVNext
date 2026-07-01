using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public static partial class UnionWithTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenOtherIsNotEmptyAndSetIsEmpty_TestCases
        = new[]
        {
            new TestCaseData(new[] { 1 })       .SetName("{m}(Single other item)"),
            new TestCaseData(new[] { 1, 2, 3 }) .SetName("{m}(Multiple other items)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenOtherIsNotEmptyAndNotSubsetOfSetAndSetIsNotEmpty_TestCases
        = new[]
        {
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 2 },
                    Other = new[] { 1, 2, 3 } 
                })
                .SetName("{m}(Single item in set, other is proper superset)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 2, 3, 4 },
                    Other = new[] { 1, 2, 3, 4, 5 } 
                })
                .SetName("{m}(Multiple items in set, other is proper superset)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 1, 2, 3 },
                    Other = new[] { 2, 3, 4 } 
                })
                .SetName("{m}(Mutually-exclusive overlap)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenOtherIsSubsetOfSet_TestCases
        = new[]
        {
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = Array.Empty<int>(),
                    Other = Array.Empty<int>() 
                })
                .SetName("{m}(Exact match, no items)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 1 },
                    Other = new[] { 1 } 
                })
                .SetName("{m}(Exact match, single item)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 1, 2, 3 },
                    Other = new[] { 1, 2, 3 } 
                })
                .SetName("{m}(Exact match, multiple items)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 1 },
                    Other = Array.Empty<int>() 
                })
                .SetName("{m}(Proper subset, no other items)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 1, 2, 3 },
                    Other = new[] { 2 } 
                })
                .SetName("{m}(Proper subset, single other item)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 1, 2, 3, 4, 5 },
                    Other = new[] { 2, 3, 4 } 
                })
                .SetName("{m}(Proper subset, multiple other items)"),
        };
}
