using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed;

public static partial class ResetTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenKeySelectorReturnsNull_TestCases
        = new[]
        {
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = Array.Empty<KeyValuePair<string, int>>(),
                    KeySelector     = static _ => null!, 
                    Values          = new[] { 1 }
                })
                .SetName("{m}(Empty collection, Single item reset)"),
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = Array.Empty<KeyValuePair<string, int>>(),
                    KeySelector     = static value => (value is 1) ? null! : value.ToString(), 
                    Values          = new[] { 1, 2 }
                })
                .SetName("{m}(Empty collection, Multiple item reset, null is first)"),
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = Array.Empty<KeyValuePair<string, int>>(),
                    KeySelector     = static value => (value is 2) ? null! : value.ToString(), 
                    Values          = new[] { 1, 2 }
                })
                .SetName("{m}(Empty collection, Multiple item reset, null is last)"),
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    KeySelector     = static _ => null!,
                    Values          = new[] { 2 }
                })
                .SetName("{m}(Single item in collection, Single item reset)"),
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    KeySelector     = static value => (value is 2) ? null! : value.ToString(), 
                    Values          = new[] { 2, 3 }
                })
                .SetName("{m}(Single item in collection, Multiple item reset, null is first)"),
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    KeySelector     = static value => (value is 3) ? null! : value.ToString(), 
                    Values          = new[] { 2, 3 }
                })
                .SetName("{m}(Single item in collection, Multiple item reset, null is last)"),
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    KeySelector     = static _ => null!,
                    Values          = new[] { 4 }
                })
                .SetName("{m}(Multiple items in collection, Single item reset)"),
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    KeySelector     = static value => (value is 4) ? null! : value.ToString(), 
                    Values          = new[] { 4, 5 }
                })
                .SetName("{m}(Multiple items in collection, Multiple item reset, null is first)"),
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    KeySelector     = static value => (value is 5) ? null! : value.ToString(), 
                    Values          = new[] { 4, 5 }
                })
                .SetName("{m}(Multiple items in collection, Multiple item reset, null is last)"),
        };

    public static readonly IReadOnlyList<TestCaseData> WhenValuesIsEmptyAndDictionaryIsNot_TestCases
        = new[]
        {
            new TestCaseData(new[] { new KeyValuePair<string, int>("1", 1) })
                .SetName("{m}(Single item in reset)"),
            new TestCaseData(new KeyValuePair<string, int>[]
                {
                    new("1", 1),
                    new("2", 2),
                    new("3", 3)
                })
                .SetName("{m}(Multiple items in reset)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenValuesIsNotEmpty_TestCases
        = new[]
        {
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = Array.Empty<KeyValuePair<string, int>>(),
                    KeySelector     = static value => value.ToString(),
                    Values          = new[] { 1 }
                })
                .SetName("{m}(Empty dictionary)"),
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    KeySelector     = static value => value.ToString(),
                    Values          = new[] { 2 }
                })
                .SetName("{m}(Single item in dictionary)"),
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    KeySelector     = static value => value.ToString(),
                    Values          = new[] { 4, 5, 6 }
                })
                .SetName("{m}(Multiple items in dictionary)"),
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    KeySelector     = static value => value.ToString(),
                    Values          = new[] { 1 }
                })
                .SetName("{m}(Redundant reset)")
        };
}
