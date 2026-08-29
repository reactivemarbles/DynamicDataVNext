namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class RemoveTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenListContainsItemAndOtherItems_TestCases
        = new[]
        {
            new TestCaseData(new SingleIndexedItemOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2" },
                    Index           = 0,
                    Item            = "1"
                })
                .SetName("{m}(Remove non-null from front)"),
            new TestCaseData(new SingleIndexedItemOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2" },
                    Index           = 1,
                    Item            = "2"
                })
                .SetName("{m}(Remove non-null from back)"),
            new TestCaseData(new SingleIndexedItemOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 1,
                    Item            = "2"
                })
                .SetName("{m}(Remove non-null from middle)"),
            new TestCaseData(new SingleIndexedItemOperationTestCase()
                {
                    InitialItems    = new[] { "1", "1", "1" },
                    Index           = 0,
                    Item            = "1"
                })
                .SetName("{m}(Remove non-null duplicate)"),
            new TestCaseData(new SingleIndexedItemOperationTestCase()
                {
                    InitialItems    = new[] { null, "2" },
                    Index           = 0,
                    Item            = null
                })
                .SetName("{m}(Remove null from front)"),
            new TestCaseData(new SingleIndexedItemOperationTestCase()
                {
                    InitialItems    = new[] { "1", null },
                    Index           = 1,
                    Item            = null
                })
                .SetName("{m}(Remove null from back)"),
            new TestCaseData(new SingleIndexedItemOperationTestCase()
                {
                    InitialItems    = new[] { "1", null, "3" },
                    Index           = 1,
                    Item            = null
                })
                .SetName("{m}(Remove null from middle)"),
            new TestCaseData(new SingleIndexedItemOperationTestCase()
                {
                    InitialItems    = new string?[] { null, null, null },
                    Index           = 0,
                    Item            = null
                })
                .SetName("{m}(Remove null duplicate)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenListDoesNotContainItem_TestCases
        = new[]
        {
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Item            = null
                })
                .SetName("{m}(Single item in list, attempt to remove null)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Item            = "2"
                })
                .SetName("{m}(Single item in list, attempt to remove non-null)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new string?[] { null },
                    Item            = "1"
                })
                .SetName("{m}(Single null in list)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Item            = null
                })
                .SetName("{m}(Multiple items in list, attempt to remove null)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Item            = "4"
                })
                .SetName("{m}(Multiple items in list, attempt to remove non-null)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new string?[] { null, null, null },
                    Item            = "1"
                })
                .SetName("{m}(Multiple nulls in list)")
        };
}
