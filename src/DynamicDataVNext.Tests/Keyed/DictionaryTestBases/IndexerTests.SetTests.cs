using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

public static partial class IndexerTests
{
    public static partial class SetTests
    {
        public static readonly IReadOnlyList<TestCaseData> WhenDictionaryContainsKeyWithDifferentValue_TestCases
            = new[]
            {
                new TestCaseData(new SingleItemOperationTestCase()
                    {
                        InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                        Key             = "1",
                        Value           = 2
                    })
                    .SetName("{m}(Single item in collection)"),
                new TestCaseData(new SingleItemOperationTestCase()
                    {
                        InitialItems    = new KeyValuePair<string, int>[]
                        {
                            new("1", 1),
                            new("2", 2),
                            new("3", 3)
                        },
                        Key             = "2",
                        Value           = 4
                    })
                    .SetName("{m}(Multiple items in collection)")
            };

        public static readonly IReadOnlyList<TestCaseData> WhenDictionaryContainsKeyWithSameValue_TestCases
            = new[]
            {
                new TestCaseData(new SingleItemOperationTestCase()
                    {
                        InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                        Key             = "1",
                        Value           = 1
                    })
                    .SetName("{m}(Single item in collection)"),
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
                    .SetName("{m}(Multiple items in collection)")
            };

        public static readonly IReadOnlyList<TestCaseData> WhenDictionaryDoesNotContainKey_TestCases
            = new[]
            {
                new TestCaseData(new SingleItemOperationTestCase()
                    {
                        InitialItems    = Array.Empty<KeyValuePair<string, int>>(),
                        Key             = "1",
                        Value           = 1
                    })
                    .SetName("{m}(Empty collection)"),
                new TestCaseData(new SingleItemOperationTestCase()
                    {
                        InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                        Key             = "2",
                        Value           = 2
                    })
                    .SetName("{m}(Single item in collection)"),
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
                    .SetName("{m}(Multiple items in collection)")
            };
    }
}
