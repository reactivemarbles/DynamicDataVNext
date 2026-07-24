using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

public static partial class RemoveTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenDictionaryContainsItem_TestCases
        = new[]
        {
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    Key             = "1",
                    Value           = 1
                })
                .SetName("{m}(Single item in dictionary)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    Key             = "2",
                    Value           = 2
                })
                .SetName("{m}(Multiple items in dictionary)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenDictionaryContainsKey_TestCases
        = new[]
        {
            new TestCaseData(new SingleKeyOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    Key             = "1"
                })
                .SetName("{m}(Single item in dictionary)"),
            new TestCaseData(new SingleKeyOperationTestCase()
                {
                    InitialItems    = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    Key             = "2"
                })
                .SetName("{m}(Multiple items in dictionary)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenDictionaryDoesNotContainItem_TestCases
        = new[]
        {
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = Array.Empty<KeyValuePair<string, int>>(),
                    Key             = "1",
                    Value           = 1
                })
                .SetName("{m}(Empty dictionary)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    Key             = "2",
                    Value           = 1
                })
                .SetName("{m}(Single item in dictionary, key does not match)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    Key             = "1",
                    Value           = 2
                })
                .SetName("{m}(Single item in dictionary, value does not match)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    Key             = "4",
                    Value           = 4
                })
                .SetName("{m}(Multiple items in dictionary)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenDictionaryDoesNotContainKey_TestCases
        = new[]
        {
            new TestCaseData(new SingleKeyOperationTestCase()
                {
                    InitialItems    = Array.Empty<KeyValuePair<string, int>>(),
                    Key             = "1"
                })
                .SetName("{m}(Empty dictionary)"),
            new TestCaseData(new SingleKeyOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    Key             = "2"
                })
                .SetName("{m}(Single item in dictionary)"),
            new TestCaseData(new SingleKeyOperationTestCase()
                {
                    InitialItems    = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    Key             = "4"
                })
                .SetName("{m}(Multiple items in dictionary)")
        };
}
