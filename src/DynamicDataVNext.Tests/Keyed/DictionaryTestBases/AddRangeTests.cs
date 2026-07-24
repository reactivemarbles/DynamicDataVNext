using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

public static partial class AddRangeTests
{
    public static readonly IReadOnlyList<TestCaseData> InitialItems_TestCases
        = new[]
        {
            new TestCaseData(Array.Empty<KeyValuePair<string, int>>())
                .SetName("{m}(Empty collection)"),
            new TestCaseData(new[] { new KeyValuePair<string, int>("1", 1) })
                .SetName("{m}(Single item in collection)"),
            new TestCaseData(new KeyValuePair<string, int>[]
                {
                    new("1", 1),
                    new("2", 2),
                    new("3", 3)
                })
                .SetName("{m}(Multiple items in collection)")
        };
    
    public static readonly IReadOnlyList<TestCaseData> WhenItemsAndDictionaryAreNotEmptyAndKeysDoNotOverlap_TestCases
        = new[]
        {
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    Items           = new[] { new KeyValuePair<string, int>("2", 2) }
                })
                .SetName("{m}(Single item in collection, Single item added)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    Items           = new KeyValuePair<string, int>[]
                    {
                        new("2", 2),
                        new("3", 3),
                        new("4", 4)
                    }
                })
                .SetName("{m}(Single item in collection, Multiple items added)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    Items           = new[] { new KeyValuePair<string, int>("4", 4) }
                })
                .SetName("{m}(Multiple items in collection, Single item added)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    Items           = new KeyValuePair<string, int>[]
                    {
                        new("4", 4),
                        new("5", 5),
                        new("6", 6)
                    }
                })
                .SetName("{m}(Multiple items in collection, Multiple items added)")
        };
    
    public static readonly IReadOnlyList<TestCaseData> WhenItemsContainsNullKey_TestCases
        = new[]
        {
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = Array.Empty<KeyValuePair<string, int>>(),
                    Items           = new[] { new KeyValuePair<string, int>(null!, 1) }
                })
                .SetName("{m}(Empty collection, Single added item)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = Array.Empty<KeyValuePair<string, int>>(),
                    Items           = new KeyValuePair<string, int>[]
                    {
                        new(null!,  1),
                        new("2",    2)
                    }
                })
                .SetName("{m}(Empty collection, Multiple added items, null is first)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = Array.Empty<KeyValuePair<string, int>>(),
                    Items           = new KeyValuePair<string, int>[]
                    {
                        new("1",    1),
                        new(null!,  2)
                    }
                })
                .SetName("{m}(Empty collection, Multiple added items, null is last)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    Items           = new[] { new KeyValuePair<string, int>(null!, 2) }
                })
                .SetName("{m}(Single item in collection, Single added item)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    Items           = new KeyValuePair<string, int>[]
                    {
                        new(null!,  2),
                        new("3",    3)
                    }
                })
                .SetName("{m}(Single item in collection, Multiple added items, null is first)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    Items           = new KeyValuePair<string, int>[]
                    {
                        new("2",    2),
                        new(null!,  3)
                    }
                })
                .SetName("{m}(Single item in collection, Multiple added items, null is last)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    Items           = new[] { new KeyValuePair<string, int>(null!, 4) }
                })
                .SetName("{m}(Multiple items in collection, Single added item)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    Items           = new KeyValuePair<string, int>[]
                    {
                        new(null!,  4),
                        new("5",    5)
                    }
                })
                .SetName("{m}(Multiple items in collection, Multiple added items, null is first)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    Items           = new KeyValuePair<string, int>[]
                    {
                        new("4",    4),
                        new(null!,  5)
                    }
                })
                .SetName("{m}(Multiple items in collection, Multiple added items, null is last)"),
        };

    public static readonly IReadOnlyList<TestCaseData> WhenItemsHasDuplicateKeys_TestCases
        = new[]
        {
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = Array.Empty<KeyValuePair<string, int>>(),
                    Items           = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("1", 1)
                    }
                })
                .SetName("{m}(Empty collection)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    Items           = new KeyValuePair<string, int>[]
                    {
                        new("2", 2),
                        new("2", 2)
                    }
                })
                .SetName("{m}(Single item in collection)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    Items           = new KeyValuePair<string, int>[]
                    {
                        new("4", 4),
                        new("4", 4)
                    }
                })
                .SetName("{m}(Multiple items in collection)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    Items           = new KeyValuePair<string, int>[]
                    {
                        new("2", 2),
                        new("2", 3)
                    }
                })
                .SetName("{m}(Duplicate keys have different values)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    Items           = new KeyValuePair<string, int>[]
                    {
                        new("2", 2),
                        new("3", 3),
                        new("4", 4),
                        new("2", 2)
                    }
                })
                .SetName("{m}(Duplicate item is last)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenItemsIsNotEmpty_TestCases
        = new[]
        {
            new TestCaseData(new[] { new KeyValuePair<string, int>("1", 1) })
                .SetName("{m}(Single item)"),
            new TestCaseData(new KeyValuePair<string, int>[]
                {
                    new("1", 1),
                    new("2", 2),
                    new("3", 3)
                })
                .SetName("{m}(Multiple items)")
        };
    
    public static readonly IReadOnlyList<TestCaseData> WhenItemsKeysAndDictionaryKeysOverlap_TestCases
        = new[]
        {
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    Items           = new[] { new KeyValuePair<string, int>("1", 1) }
                })
                .SetName("{m}(Single item in collection, Single item added, same value)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    Items           = new[] { new KeyValuePair<string, int>("1", 2) }
                })
                .SetName("{m}(Single item in collection, Single item added, different value)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    Items           = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    }
                })
                .SetName("{m}(Single item in collection, Multiple items added, first is duplicated)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("3", 3) },
                    Items           = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    }
                })
                .SetName("{m}(Single item in collection, Multiple items added, last is duplicated)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    Items           = new[] { new KeyValuePair<string, int>("2", 2) }
                })
                .SetName("{m}(Multiple items in collection, Single item added)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenKeySelectorReturnsNull_TestCases
        = new[]
        {
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = Array.Empty<KeyValuePair<string, int>>(),
                    KeySelector     = static _ => null!, 
                    Values          = new[] { 1 }
                })
                .SetName("{m}(Empty collection, Single added item)"),
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = Array.Empty<KeyValuePair<string, int>>(),
                    KeySelector     = static value => (value is 1) ? null! : value.ToString(), 
                    Values          = new[] { 1, 2 }
                })
                .SetName("{m}(Empty collection, Multiple added items, null is first)"),
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = Array.Empty<KeyValuePair<string, int>>(),
                    KeySelector     = static value => (value is 2) ? null! : value.ToString(), 
                    Values          = new[] { 1, 2 }
                })
                .SetName("{m}(Empty collection, Multiple added items, null is last)"),
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    KeySelector     = static _ => null!,
                    Values          = new[] { 2 }
                })
                .SetName("{m}(Single item in collection, Single added item)"),
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    KeySelector     = static value => (value is 2) ? null! : value.ToString(), 
                    Values          = new[] { 2, 3 }
                })
                .SetName("{m}(Single item in collection, Multiple added items, null is first)"),
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    KeySelector     = static value => (value is 3) ? null! : value.ToString(), 
                    Values          = new[] { 2, 3 }
                })
                .SetName("{m}(Single item in collection, Multiple added items, null is last)"),
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
                .SetName("{m}(Multiple items in collection, Single added item)"),
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
                .SetName("{m}(Multiple items in collection, Multiple added items, null is first)"),
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
                .SetName("{m}(Multiple items in collection, Multiple added items, null is last)"),
        };

    public static readonly IReadOnlyList<TestCaseData> WhenKeysProducedByKeySelectorForValuesAndDictionaryKeysOverlap_TestCases
        = new[]
        {
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    KeySelector     = static value => value.ToString(),
                    Values          = new[] { 1 }
                })
                .SetName("{m}(Single item in collection, Single item added)"),
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    KeySelector     = static value => value.ToString(),
                    Values          = new[] { 1, 2, 3 }
                })
                .SetName("{m}(Single item in collection, Multiple items added, first is duplicated)"),
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("3", 3) },
                    KeySelector     = static value => value.ToString(),
                    Values          = new[] { 1, 2, 3 }
                })
                .SetName("{m}(Single item in collection, Multiple items added, last is duplicated)"),
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    KeySelector     = static value => value.ToString(),
                    Values          = new[] { 2 }
                })
                .SetName("{m}(Multiple items in collection, Single item added)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenValuesAndDictionaryAreNotEmptyAndKeysDoNotOverlap_TestCases
        = new[]
        {
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    KeySelector     = static value => value.ToString(),
                    Values          = new[] { 2 }
                })
                .SetName("{m}(Single item in collection, Single item added)"),
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    KeySelector     = static value => value.ToString(),
                    Values          = new[] { 2, 3, 4 }
                })
                .SetName("{m}(Single item in collection, Multiple items added)"),
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    KeySelector     = static value => value.ToString(),
                    Values          = new[] { 4 }
                })
                .SetName("{m}(Multiple items in collection, Single item added)"),
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
                .SetName("{m}(Multiple items in collection, Multiple items added)")
        };
    
    public static readonly IReadOnlyList<TestCaseData> WhenValuesAndKeySelectorProducesDuplicateKeys_TestCases
        = new[]
        {
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = Array.Empty<KeyValuePair<string, int>>(),
                    KeySelector     = static value => value.ToString(),
                    Values          = new[] { 1, 1 }
                })
                .SetName("{m}(Empty collection)"),
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    KeySelector     = static value => value.ToString(),
                    Values          = new[] { 2, 2 }
                })
                .SetName("{m}(Single item in collection)"),
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    KeySelector     = static value => value.ToString(),
                    Values          = new[] { 4, 4 }
                })
                .SetName("{m}(Multiple items in collection)"),
            new TestCaseData(new ValueRangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    KeySelector     = static value => value.ToString(),
                    Values          = new[] { 2, 3, 4, 2 }
                })
                .SetName("{m}(Duplicate item is last)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenValuesIsNotEmpty_TestCases
        = new[]
        {
            new TestCaseData(new[] { 1 })       .SetName("{m}(Single value)"),
            new TestCaseData(new[] { 1, 2, 3 }) .SetName("{m}(Multiple values)")
        };
}
