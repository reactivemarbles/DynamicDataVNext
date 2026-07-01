using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public partial class SymmetricExceptWithTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenOtherDoesNotOverlapSetAndSetIsEmpty_TestCases
        = new[]
        {
            new TestCaseData(new[] { 1 })       .SetName("{m}(Single item)"),
            new TestCaseData(new[] { 1, 2, 3 }) .SetName("{m}(Multiple items)")
        };
    
    public static readonly IReadOnlyList<TestCaseData> WhenOtherEqualsSetAndSetIsNotEmpty_TestCases
        = new[]
        {
            new TestCaseData(new[] { 1 })       .SetName("{m}(Single item in set)"),
            new TestCaseData(new[] { 1, 2, 3 }) .SetName("{m}(Multiple items in set)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenOtherIsEmpty_TestCases
        = new[]
        {
            new TestCaseData(Array.Empty<int>())    .SetName("{m}(Set is empty)"),
            new TestCaseData(new[] { 1 })           .SetName("{m}(Single item in set)"),
            new TestCaseData(new[] { 1, 2, 3 })     .SetName("{m}(Multiple items in set)")
        };
    
    public static readonly IReadOnlyList<TestCaseData> WhenOtherIsNotEmptyAndDoesNotOverlapSetAndSetIsNotEmpty_TestCases
        = new[]
        {
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
    
    public static readonly IReadOnlyList<TestCaseData> WhenOtherIsProperSupersetOfSetAndSetIsNotEmpty_TestCases
        = new[]
        {
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 2 },
                    Other = new[] { 1, 2, 3 } 
                })
                .SetName("{m}(Single item in set)"),
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 2, 3, 4 },
                    Other = new[] { 1, 2, 3, 4, 5 } 
                })
                .SetName("{m}(Multiple items in set)")
        };
    
    public static readonly IReadOnlyList<TestCaseData> WhenOtherOverlapsAndIsNotSupersetOfSet_TestCases
        = new[]
        {
            new TestCaseData(new SetOperationTestCase()
                {
                    Items = new[] { 1, 2, 3 },
                    Other = new[] { 2, 3, 4 } 
                })
                .SetName("{m}(Mutually-exclusive overlap)")
        };
}
