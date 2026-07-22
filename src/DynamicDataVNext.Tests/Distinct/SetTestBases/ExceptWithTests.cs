using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public partial class ExceptWithTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenOtherDoesNotOverlapSet_TestCases
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
    
    public static readonly IReadOnlyList<TestCaseData> WhenOtherIsSupersetOfSetAndSetIsNotEmpty_TestCases
        = new[]
        {
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 1 },
                    Other = new[] { 1 } 
                })
                .SetName("{m}(Other equals set, single item)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 1, 2, 3 },
                    Other = new[] { 1, 2, 3 } 
                })
                .SetName("{m}(Other equals set, multiple items)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 2 },
                    Other = new[] { 1, 2, 3 } 
                })
                .SetName("{m}(Proper superset, single item)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 2, 3, 4 },
                    Other = new[] { 1, 2, 3, 4, 5 } 
                })
                .SetName("{m}(Proper superset, multiple items)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenOtherOverlapsSetAndIsNotSupersetOfSet_TestCases
        = new[]
        {
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 1, 2, 3, 4, 5 },
                    Other = new[] { 2, 3, 4 } 
                })
                .SetName("{m}(Other is proper subset of set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 1, 2, 3 },
                    Other = new[] { 2, 3, 4 } 
                })
                .SetName("{m}(Mutually-exclusive overlap)")
        };
}
