namespace DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

public static partial class BuildAndClearTests
{
    public abstract class Base<TUutAdapter, TChangeSet, TChange, TChangeType>
            : ChangeSetBuilderBaseTests.Base<TUutAdapter, TChangeSet, TChange, TChangeType>
        where TUutAdapter : IUutAdapter<TChangeSet, TChange, TChangeType>, new()
        where TChangeSet : struct, IChangeSet<TChange, TChangeType>
        where TChange : struct, IChange<TChangeType>
        where TChangeType : Enum
    {
        [TestCaseSource(typeof(BuildAndClearTests), nameof(WhenAdditionsDoNotFollowClearOrInitiallyEmptySource_TestCases))]
        public void WhenAdditionsDoNotFollowClearOrInitiallyEmptySource_ResultIsUpdateAndBuilderIsReset(SetupTestCase testCase)
            => Always_ResultIsExpectedAndBuilderIsReset(
                testCase:           testCase,
                expectedResultType: ChangeSetType.Update,
                because:            (testCase.ChangesCategories
                            .Reverse()
                            .TakeWhile(static category => category is ChangeCategory.Addition)
                            .Count()
                        is 1)
                    ? "an item was added to a non-empty source"
                    : "items were added to a non-empty source");
        
        [TestCaseSource(typeof(BuildAndClearTests), nameof(WhenAdditionsFollowClearOrInitiallyEmptySource_TestCases))]
        public void WhenAdditionsFollowClearOrInitiallyEmptySource_ResultIsResetAndBuilderIsReset(SetupTestCase testCase)
            => Always_ResultIsExpectedAndBuilderIsReset(
                testCase:           testCase,
                expectedResultType: ChangeSetType.Reset,
                because:            (
                    testCase.ChangesCategories.Count(static change => change is ChangeCategory.Removal),
                    testCase.ChangesCategories.Count(static change => change is ChangeCategory.Addition))
                        switch
                        {
                            (0, 1)  => "an item was added to an empty source",
                            (0, >1) => "items were added to an empty source",
                            (>0, 1) => "all items were removed from the source, then a new item was added to it",
                            _       => "all items were removed from the source, then new items were added to it" 
                        });

        [TestCaseSource(typeof(BuildAndClearTests), nameof(WhenChangesIsEmpty_TestCases))]
        public void WhenChangesIsEmpty_ResultIsEmpty(SetupTestCase testCase)
            => Always_ResultIsExpectedAndBuilderIsReset(
                testCase:           testCase,
                expectedResultType: ChangeSetType.Empty,
                because:            "there were no buffered changes to be captured");

        [TestCaseSource(typeof(BuildAndClearTests), nameof(WhenRemovalsAloneDoNotLeaveSourceEmpty_TestCases))]
        public void WhenRemovalsAloneDoNotLeaveSourceEmpty_ResultIsUpdateAndBuilderIsReset(SetupTestCase testCase)
            => Always_ResultIsExpectedAndBuilderIsReset(
                testCase:           testCase,
                expectedResultType: ChangeSetType.Update,
                because:            (testCase.ChangesCategories.Count is 1)
                    ? "the only item in the source was removed"
                    : "all items in the source were removed");

        [TestCaseSource(typeof(BuildAndClearTests), nameof(WhenRemovalsAloneLeaveSourceEmpty_TestCases))]
        public void WhenRemovalsAloneLeaveSourceEmpty_ResultIsClearAndBuilderIsReset(SetupTestCase testCase)
            => Always_ResultIsExpectedAndBuilderIsReset(
                testCase:           testCase,
                expectedResultType: ChangeSetType.Clear,
                because:            "all items were removed from the source");

        [TestCaseSource(typeof(BuildAndClearTests), nameof(WhenRemovalsFollowReset_TestCases))]
        public void WhenRemovalsFollowReset_ResultIsUpdateAndBuilderIsReset(SetupTestCase testCase)
            => Always_ResultIsExpectedAndBuilderIsReset(
                testCase:           testCase,
                expectedResultType: ChangeSetType.Update,
                because:            (testCase.ChangesCategories
                        .Reverse()
                        .TakeWhile(static category => category is ChangeCategory.Removal)
                        .Count()
                    is 1)
                    ? "a reset was performed, but then an item was removed"
                    : "a reset was performed, but then items were removed");

        [TestCaseSource(typeof(BuildAndClearTests), nameof(WhenRemovalsLeaveSourceEmptyAfterAdditions_TestCases))]
        public void WhenRemovalsLeaveSourceEmptyAfterAdditions_ResultIsUpdateAndBuilderIsReset(SetupTestCase testCase)
            => Always_ResultIsExpectedAndBuilderIsReset(
                testCase:           testCase,
                expectedResultType: ChangeSetType.Update,
                because:            "items were added before the source was cleared");

        private static void Always_ResultIsExpectedAndBuilderIsReset(
            SetupTestCase   testCase,
            ChangeSetType   expectedResultType,
            string          because)
        {
            var uut = PerformSetup(testCase);

            uut.Changes.Count.Should().Be(testCase.ChangesCategories.Count, "{0} changes were added", testCase.ChangesCategories.Count);
            uut.CurrentType.Should().Be(expectedResultType, because);
            
            var priorChanges        = uut.Changes.ToArray();
            var priorSourceCount    = uut.SourceCount;
            
            var result = uut.BuildAndClear();

            TUutAdapter.AssertShouldBeValid(result);
            result.Changes.Should().BeEquivalentTo(priorChanges, static options => options.WithStrictOrdering(), "all buffered changes should have been captured");
            result.Type.Should().Be(expectedResultType);

            uut.Changes.Count.Should().Be(0, "all buffered changes should have been consumed");
            uut.CurrentType.Should().Be(ChangeSetType.Empty, "there are no buffered changes");
            uut.SourceCount.Should().Be(priorSourceCount, "the state of the source collection should not have been changed");
        }
    }
}
