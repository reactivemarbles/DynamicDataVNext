namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class AddRangeTests
{
    public static readonly IReadOnlyList<TestCaseData> InitialITems_TestCases
        = new[]
        {
            new TestCaseData((object?)Array.Empty<string>())    .SetName("{m}(Empty list)"),
            new TestCaseData((object?)new[] { "1" })            .SetName("{m}(Single item in list)"),
            new TestCaseData((object?)new[] { "1", "2", "3" })  .SetName("{m}(Multiple items in list)")
        };
        
    public static readonly IReadOnlyList<TestCaseData> WhenListAndItemsAreNotEmpty_TestCases
        = new[]
        {
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Items           = new[] { "1" }
                })
                .SetName("{m}(Single item in list, same item to add)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Items           = new[] { "2" }
                })
                .SetName("{m}(Single item in list, different item to add)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Items           = new[] { "2", "3", "4" }
                })
                .SetName("{m}(Single item in list, Multiple items to add)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Items           = new[] { "2" }
                })
                .SetName("{m}(Multiple items in list, Duplicate item to add)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Items           = new[] { "4" }
                })
                .SetName("{m}(Multiple items in list, Unique item to add)"),
            new TestCaseData(new ItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Items           = new[] { "4", "5", "6" }
                })
                .SetName("{m}(Multiple items in list, Multiple items to add)")
        };
}
