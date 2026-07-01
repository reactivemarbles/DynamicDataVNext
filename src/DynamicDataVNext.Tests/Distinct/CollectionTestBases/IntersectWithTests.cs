using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public partial class IntersectWithTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenOtherDoesNotOverlapSetAndSetIsNotEmpty_TestCases
        = new[]
        {
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 1 },
                    Other = Array.Empty<int>() 
                })
                .SetName("{m}(No other items)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 1 },
                    Other = new[] { 2 } 
                })
                .SetName("{m}(Single item)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 1, 2, 3 },
                    Other = new[] { 4, 5, 6 } 
                })
                .SetName("{m}(Multiple items)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenOtherIsSupersetOfSet_TestCases
        = new[]
        {
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 1 },
                    Other = new[] { 1 } 
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
                    Items = new[] { 1, 2, 3, 4, 5 },
                    Other = new[] { 1, 2, 3, 4, 5 } 
                })
                .SetName("{m}(Exact match, multiple items)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 2, 3, 4 },
                    Other = new[] { 1, 2, 3, 4, 5 } 
                })
                .SetName("{m}(Other is proper superset of set)")
        };
    
    public static readonly IReadOnlyList<TestCaseData> WhenOtherOverlapsSetAndIsNotSupersetOfSet_TestCases
        = new[]
        {
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 1, 2, 3 },
                    Other = new[] { 2, 3, 4 } 
                })
                .SetName("{m}(Mutually-exclusive overlap)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 1, 2, 3 },
                    Other = new[] { 2 } 
                })
                .SetName("{m}(Other is proper subset of set, single item)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 1, 2, 3, 4, 5 },
                    Other = new[] { 2, 3, 4 } 
                })
                .SetName("{m}(Other is proper subset of set, multiple items)")
        };
}
