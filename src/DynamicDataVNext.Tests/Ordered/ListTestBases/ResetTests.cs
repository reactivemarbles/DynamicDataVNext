namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class ResetTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenItemsIsEmptyAndListIsNot_TestCases
        = new[]
        {
            new TestCaseData((object)new[] { "1" })                         .SetName("{m}(Single non-null item in list)"),
            new TestCaseData((object)new string?[] { null })                .SetName("{m}(Single null item in list)"),
            new TestCaseData((object)new[] { "1", "2", "3" })               .SetName("{m}(Multiple non-null items in list)"),
            new TestCaseData((object)new string?[] { null, null, null })    .SetName("{m}(Multiple null items in list)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenItemsIsNotEmpty_TestCases
        = new[]
        {
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = Array.Empty<string?>(),
                    Items           = new[] { "1" }
                })
                .SetName("{m}(Empty list, Single non-null item reset)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = Array.Empty<string?>(),
                    Items           = new string?[] { null }
                })
                .SetName("{m}(Empty list, Single null item reset)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Items           = new[] { "1" }
                })
                .SetName("{m}(Single non-null item in list, Redundant reset)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Items           = new[] { "2" }
                })
                .SetName("{m}(Single non-null item in list, Single non-null item reset)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Items           = new string?[] { null }
                })
                .SetName("{m}(Single non-null item in list, Single null item reset)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new string?[] { null },
                    Items           = new string?[] { null }
                })
                .SetName("{m}(Single null item in list, Redundant reset)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new string?[] { null },
                    Items           = new[] { "2" }
                })
                .SetName("{m}(Single null item in list, Single non-null item reset)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Items           = new[] { "1" }
                })
                .SetName("{m}(Multiple non-null items in list, Single non-null item reset)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Items           = new string?[] { null }
                })
                .SetName("{m}(Multiple non-null items in list, Single null item reset)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Items           = new[] { "1", "2", "3" }
                })
                .SetName("{m}(Multiple non-null items in list, Redundant reset)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Items           = new[] { "4", "5", "6" }
                })
                .SetName("{m}(Multiple non-null items in list, Multiple non-null item reset)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new string?[] { null, null, null },
                    Items           = new[] { "1" }
                })
                .SetName("{m}(Multiple null items in list, Single non-null item reset)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new string?[] { null, null, null },
                    Items           = new string?[] { null }
                })
                .SetName("{m}(Multiple null items in list, Single null item reset)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new string?[] { null, null, null },
                    Items           = new[] { "1", "2", "3" }
                })
                .SetName("{m}(Multiple null items in list, Multiple non-null item reset)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new string?[] { null, null, null },
                    Items           = new string?[] { null, null, null }
                })
                .SetName("{m}(Multiple null items in list, Redundant reset)")
        };
}
