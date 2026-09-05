namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class RemoveRangeTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenCountIsNotInRange_TestCases
        = new[]
        {
            new TestCaseData(new NumericRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Index           = 0,
                    Count           = int.MinValue
                })
                .SetName("{m}(Min negative value)"),
            new TestCaseData(new NumericRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Index           = 0,
                    Count           = -1
                })
                .SetName("{m}(Max negative value)"),
            new TestCaseData(new NumericRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1" },
                    Index           = 0,
                    Count           = 2
                })
                .SetName("{m}(Single item in list)"),
            new TestCaseData(new NumericRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 0,
                    Count           = 4
                })
                .SetName("{m}(From front of list)"),
            new TestCaseData(new NumericRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 2,
                    Count           = 2
                })
                .SetName("{m}(From back of list)"),
            new TestCaseData(new NumericRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 1,
                    Count           = 3
                })
                .SetName("{m}(From middle of list)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenIndexAndCountArePartOfList_TestCases
        = new[]
        {
            new TestCaseData(new NumericRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 0,
                    Count           = 1
                })
                .SetName("{m}(Front item of list)"),
            new TestCaseData(new NumericRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 0,
                    Count           = 2
                })
                .SetName("{m}(Front chunk of list)"),
            new TestCaseData(new NumericRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 2,
                    Count           = 1
                })
                .SetName("{m}(Back item of list)"),
            new TestCaseData(new NumericRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 1,
                    Count           = 2
                })
                .SetName("{m}(Back chunk of list)"),
            new TestCaseData(new NumericRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 1,
                    Count           = 1
                })
                .SetName("{m}(Middle item of list)"),
            new TestCaseData(new NumericRangeOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3", "4", "5" },
                    Index           = 1,
                    Count           = 3
                })
                .SetName("{m}(Middle chunk of list)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenIndexAndCountAreWholeList_TestCases
        = new[]
        {
            new TestCaseData((object)new[] { "1" })
                .SetName("{m}(Single item in list)"),
            new TestCaseData((object)new[] { "1", "2", "3" })
                .SetName("{m}(Multiple items in list)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenIndexIsInRangeAndCountIsZero_TestCases
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
                .SetName("{m}(Front item of list)"),
            new TestCaseData(new SingleIndexOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 2
                })
                .SetName("{m}(Back item of list)"),
            new TestCaseData(new SingleIndexOperationTestCase()
                {
                    InitialItems    = new[] { "1", "2", "3" },
                    Index           = 1
                })
                .SetName("{m}(Middle item of list)")
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
                .SetName("{m}(Multiple items in list)")
        };
}
