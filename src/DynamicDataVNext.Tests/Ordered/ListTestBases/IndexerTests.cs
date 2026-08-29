namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public static partial class IndexerTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenIndexIsOutOfRange_TestCases
        = new[]
        {
            /*                  index,          itemCount   */
            new TestCaseData(   -1,             0           ).SetName("{m}(Max negative value)"),
            new TestCaseData(   int.MinValue,   0           ).SetName("{m}(Min negative value)"),
            new TestCaseData(   0,              0           ).SetName("{m}(Index exceeds bounds, Empty list)"),
            new TestCaseData(   1,              1           ).SetName("{m}(Index exceeds bounds, Single item in list)"),
            new TestCaseData(   3,              3           ).SetName("{m}(Index exceeds bounds, Multiple items in list)"),
            new TestCaseData(   int.MaxValue,   0           ).SetName("{m}(Max positive value)")
        };

    public static readonly IReadOnlyList<TestCaseData> WhenIndexIsInRange_TestCases
        = new[]
        {
            /*                  index,  itemCount   */
            new TestCaseData(   0,      1           ).SetName("{m}(Single item in list)"),
            new TestCaseData(   0,      3           ).SetName("{m}(Min index, Multiple items in list)"),
            new TestCaseData(   2,      3           ).SetName("{m}(Median index, Multiple items in list)"),
            new TestCaseData(   2,      3           ).SetName("{m}(Max index, Multiple items in list)")
        };
}
