namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class ContainsTests
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
        new TestCaseData(new SingleItemOperationTestCase()
            {
                InitialItems    = new[] { "1" },
                Item            = "1"
            })
            .SetName("{m}(Single item in list)"),
        new TestCaseData(new SingleItemOperationTestCase()
            {
                InitialItems    = new[] { "1", "2", "3" },
                Item            = "2"
            })
            .SetName("{m}(Multiple items in list)"),
        new TestCaseData(new SingleItemOperationTestCase()
            {
                InitialItems    = new[] { "1", "2", "2", "3" },
                Item            = "2"
            })
            .SetName("{m}(Duplicate items in list)"),
        new TestCaseData(new SingleItemOperationTestCase()
            {
                InitialItems    = new[] { "1", null, "3" },
                Item            = null
            })
            .SetName("{m}(Item is null)")
    };
}
