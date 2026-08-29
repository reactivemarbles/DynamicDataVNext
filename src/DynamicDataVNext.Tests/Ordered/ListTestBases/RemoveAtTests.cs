namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class RemoveAtTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenIndexIsInRangeAndListContainsManyItems_TestCases
        = new[]
        {
            new TestCaseData(new SingleIndexOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2" },
                    Index           = 0
                })
                .SetName("{m}(Remove from front)"),
            new TestCaseData(new SingleIndexOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2" },
                    Index           = 1
                })
                .SetName("{m}(Remove from back)"),
            new TestCaseData(new SingleIndexOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 1
                })
                .SetName("{m}(Remove from middle)")
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
