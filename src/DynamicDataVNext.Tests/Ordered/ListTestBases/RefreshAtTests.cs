namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class RefreshAtTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenIndexIsInRange_TestCases
        = new[]
        {
            new TestCaseData(new SingleIndexOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Index           = 0
                })
                .SetName("{m}(Single item in list)"),
            new TestCaseData(new SingleIndexOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 0
                })
                .SetName("{m}(Refresh front item)"),
            new TestCaseData(new SingleIndexOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 2
                })
                .SetName("{m}(Refresh back item)"),
            new TestCaseData(new SingleIndexOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 1
                })
                .SetName("{m}(Refresh middle item)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenIndexIsNotInRange_TestCases
        = new[]
        {
            new TestCaseData(new SingleIndexOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Index           = int.MinValue
                })
                .SetName("{m}(Min negative value)"),
            new TestCaseData(new SingleIndexOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Index           = -1
                })
                .SetName("{m}(Max negative value)"),
            new TestCaseData(new SingleIndexOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Index           = int.MaxValue
                })
                .SetName("{m}(Max positive value)"),
            new TestCaseData(new SingleIndexOperationTestCase()
                {
                    InitialItems    = Array.Empty<string?>(),
                    Index           = 0
                })
                .SetName("{m}(Empty list)"),
            new TestCaseData(new SingleIndexOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Index           = 1
                })
                .SetName("{m}(Single item in list)"),
            new TestCaseData(new SingleIndexOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 3
                })
                .SetName("{m}(Multiple items in list)"),
        };
}
