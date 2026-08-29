namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class InsertTests
{
    public static IReadOnlyList<TestCaseData> WhenListIsNotEmpty_TestCases
        = new[]
        {
            new TestCaseData(new SingleIndexedItemOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Index           = 0,
                    Item            = "2"
                })
                .SetName("{m}(Single item in list, Unique item to insert, Insertion at front)"),
            new TestCaseData(new SingleIndexedItemOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Index           = 1,
                    Item            = "2"
                })
                .SetName("{m}(Single item in list, Unique item to insert, Insertion at end)"),
            new TestCaseData(new SingleIndexedItemOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Index           = 0,
                    Item            = "1"
                })
                .SetName("{m}(Single item in list, Duplicate item to insert, Insertion at front)"),
            new TestCaseData(new SingleIndexedItemOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Index           = 1,
                    Item            = "1"
                })
                .SetName("{m}(Single item in list, Duplicate item to insert, Insertion at end)"),
            new TestCaseData(new SingleIndexedItemOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 0,
                    Item            = "4"
                })
                .SetName("{m}(Multiple items in list, Unique item to insert, Insertion at front)"),
            new TestCaseData(new SingleIndexedItemOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 1,
                    Item            = "4"
                })
                .SetName("{m}(Multiple items in list, Unique item to insert, Insertion in middle)"),
            new TestCaseData(new SingleIndexedItemOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 2,
                    Item            = "4"
                })
                .SetName("{m}(Multiple items in list, Unique item to insert, Insertion at end)"),
            new TestCaseData(new SingleIndexedItemOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 0,
                    Item            = "1"
                })
                .SetName("{m}(Multiple items in list, Duplicate item to insert, Insertion at front)"),
            new TestCaseData(new SingleIndexedItemOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 1,
                    Item            = "2"
                })
                .SetName("{m}(Multiple items in list, Duplicate item to insert, Insertion in middle)"),
            new TestCaseData(new SingleIndexedItemOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 2,
                    Item            = "3"
                })
                .SetName("{m}(Multiple items in list, Duplicate item to insert, Insertion at end)"),
            new TestCaseData(new SingleIndexedItemOperationTestCase()
                {
                    InitialItems    = new[] { "1", null, "3" },
                    Index           = 1,
                    Item            = "2"
                })
                .SetName("{m}(List contains null)"),
            new TestCaseData(new SingleIndexedItemOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 1,
                    Item            = null
                })
                .SetName("{m}(Item is null)")
        };
}
