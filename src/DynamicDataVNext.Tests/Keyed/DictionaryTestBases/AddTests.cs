using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.DictionaryTestBases;

public static partial class AddTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenKeyIsInDictionary_TestCases
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
                    Key             = "1",
                    Value           = 1
                })
                .SetName("{m}(Multiple items in collection)"),
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
                .SetName("{m}(Key exists with different value)"),
        };

    public static readonly IReadOnlyList<TestCaseData> WhenKeyIsNotInDictionary_TestCases
        = new[]
        {
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

    public static readonly IReadOnlyList<TestCaseData> WhenKeyIsNull_TestCases
        = new[]
        {
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
}
