using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

public static partial class ContainsKeyTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenKeyIsInDictionary_TestCases
        = new[]
        {
            new TestCaseData(new SingleKeyOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    Key             = "1"
                })
                .SetName("{m}(Single item in collection)"),
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
                .SetName("{m}(Multiple items in collection)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenKeyIsNotInDictionary_TestCases
        = new[]
        {
            new TestCaseData(new SingleKeyOperationTestCase()
                {
                    InitialItems    = Array.Empty<KeyValuePair<string, int>>(),
                    Key             = "1"
                })
                .SetName("{m}(Empty collection)"),
            new TestCaseData(new SingleKeyOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    Key             = "2"
                })
                .SetName("{m}(Single item in collection)"),
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
                .SetName("{m}(Multiple items in collection)")
        };
}
