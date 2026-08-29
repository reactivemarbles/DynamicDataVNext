namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class IndexOfTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenListDoesNotContainItem_TestCases
        = new[]
        {
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = Array.Empty<string?>(),
                    Item            = "1"
                })
                .SetName("{m}(Empty list)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Item            = "2"
                })
                .SetName("{m}(Single item in list)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Item            = "4"
                })
                .SetName("{m}(Multiple items in list)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Item            = null
                })
                .SetName("{m}(Item is null)"),
            new TestCaseData(new SingleItemOperationTestCase()
                {
                    InitialItems    = new[] { "1", null, "3" },
                    Item            = "4"
                })
                .SetName("{m}(List contains null)")
        };
    
    public static readonly IReadOnlyList<TestCaseData> WhenListContainsItem_TestCases
    = new[]
    {
        new TestCaseData(new SingleIndexedItemOperationTestCase()
            {
                InitialItems    = new[] { "1" },
                Index           = 0,
                Item            = "1"
            })
            .SetName("{m}(Single item in list)"),
        new TestCaseData(new SingleIndexedItemOperationTestCase()
            {
                InitialItems    = new[] { "1", "2", "3" },
                Index           = 0,
                Item            = "1"
            })
            .SetName("{m}(Multiple items in list, Target is first)"),
        new TestCaseData(new SingleIndexedItemOperationTestCase()
            {
                InitialItems    = new[] { "1", "2", "3" },
                Index           = 1,
                Item            = "2"
            })
            .SetName("{m}(Multiple items in list, Target is median)"),
        new TestCaseData(new SingleIndexedItemOperationTestCase()
            {
                InitialItems    = new[] { "1", "2", "3" },
                Index           = 2,
                Item            = "3"
            })
            .SetName("{m}(Multiple items in list, Target is last)"),
        new TestCaseData(new SingleIndexedItemOperationTestCase()
            {
                InitialItems    = new[] { "1", "2", "2", "3" },
                Index           = 1,
                Item            = "2"
            })
            .SetName("{m}(Target item is duplicated)"),
        new TestCaseData(new SingleIndexedItemOperationTestCase()
            {
                InitialItems    = new[] { "1", null, "3" },
                Index           = 1,
                Item            = null
            })
            .SetName("{m}(Item is null)")
    };
}
