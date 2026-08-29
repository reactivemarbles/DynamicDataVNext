namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class InsertRangeTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenListAndItemsAreNotEmpty_TestCases
        = new[]
        {
            new TestCaseData(new IndexedItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Index           = 0,
                    Items           = new[] { "2" }
                })
                .SetName("{m}(Single item in list, Unique item to insert, Insertion at front)"),
            new TestCaseData(new IndexedItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Index           = 1,
                    Items           = new[] { "2" }
                })
                .SetName("{m}(Single item in list, Unique item to insert, Insertion at back)"),
            new TestCaseData(new IndexedItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Index           = 0,
                    Items           = new[] { "1" }
                })
                .SetName("{m}(Single item in list, Duplicate item to insert, Insertion at front)"),
            new TestCaseData(new IndexedItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Index           = 1,
                    Items           = new[] { "1" }
                })
                .SetName("{m}(Single item in list, Duplicate item to insert, Insertion at back)"),
            new TestCaseData(new IndexedItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 0,
                    Items           = new[] { "4" }
                })
                .SetName("{m}(Multiple items in list, Unique item to insert, Insertion at front)"),
            new TestCaseData(new IndexedItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 1,
                    Items           = new[] { "4" }
                })
                .SetName("{m}(Multiple items in list, Unique item to insert, Insertion in middle)"),
            new TestCaseData(new IndexedItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 2,
                    Items           = new[] { "4" }
                })
                .SetName("{m}(Multiple items in list, Unique item to insert, Insertion at back)"),
            new TestCaseData(new IndexedItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 0,
                    Items           = new[] { "1" }
                })
                .SetName("{m}(Multiple items in list, Duplicate item to insert, Insertion at front)"),
            new TestCaseData(new IndexedItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 1,
                    Items           = new[] { "2" }
                })
                .SetName("{m}(Multiple items in list, Duplicate item to insert, Insertion in middle)"),
            new TestCaseData(new IndexedItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 2,
                    Items           = new[] { "3" }
                })
                .SetName("{m}(Multiple items in list, Duplicate item to insert, Insertion at back)"),
            new TestCaseData(new IndexedItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 0,
                    Items           = new[] { "4", "5", "6" }
                })
                .SetName("{m}(Multiple items in list, Multiple items to insert, Insertion at front)"),
            new TestCaseData(new IndexedItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 1,
                    Items           = new[] { "4", "5", "6" }
                })
                .SetName("{m}(Multiple items in list, Multiple items to insert, Insertion in middle)"),
            new TestCaseData(new IndexedItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 2,
                    Items           = new[] { "4", "5", "6" }
                })
                .SetName("{m}(Multiple items in list, Multiple items to insert, Insertion at back)"),
            new TestCaseData(new IndexedItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", null, "3" },
                    Index           = 1,
                    Items           = new[] { "4", "5", "6" }
                })
                .SetName("{m}(List contains null)"),
            new TestCaseData(new IndexedItemRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 1,
                    Items           = new[] { "4", null, "6" }
                })
                .SetName("{m}(Items contains null)")
        };
}
