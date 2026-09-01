namespace DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

public static partial class AddChangeTests
{
    public abstract class Base<TUutAdapter, TChangeSet, TChange, TChangeType>
            : ChangeSetBuilderBaseTests.Base<TUutAdapter, TChangeSet, TChange, TChangeType>
        where TUutAdapter : IUutAdapter<TChangeSet, TChange, TChangeType>
        where TChangeSet : struct, IChangeSet<TChange, TChangeType>
        where TChange : struct, IChange<TChangeType>
        where TChangeType : Enum
    {
        [TestCaseSource(typeof(AddChangeTests), nameof(WhenSourceIsEmpty_TestCases))]
        [TestCaseSource(typeof(AddChangeTests), nameof(WhenSourceIsNotEmpty_TestCases))]
        public void WhenChangeIsAddition_BuffersChangeAndIncrementsSourceCount(SetupTestCase testCase)
        {
            var uut = PerformSetup(testCase);
            
            var priorChanges        = uut.Changes.ToArray();
            var priorSourceCount    = uut.SourceCount;
            
            var change = TUutAdapter.CreateAddition(testCase.ChangesCategories.Count);
            
            uut.AddChange(change);
            
            uut.Changes.Should().BeEquivalentTo(priorChanges.Append(change), static options => options.WithStrictOrdering(), "the given change should have been buffered.");
            WhenChangeIsAddition_AssertCurrentTypeIsCorrect(
                testCase:       testCase,
                uut:            uut,
                priorChanges:   priorChanges);
            uut.SourceCount.Should().Be(priorSourceCount + 1,   "the given change should have added an item to the collection");
        }

        [TestCaseSource(typeof(AddChangeTests), nameof(WhenSourceIsEmpty_TestCases))]
        [TestCaseSource(typeof(AddChangeTests), nameof(WhenSourceIsNotEmpty_TestCases))]
        public void WhenChangeIsNone_ThrowsException(SetupTestCase testCase)
            => WhenChangeIsInvalidOrIncoherent_ThrowsException(
                testCase:   testCase,
                change:     TUutAdapter.CreateNone(),
                because:    "changes of type None are not supported");

        [TestCaseSource(typeof(AddChangeTests), nameof(WhenSourceIsEmpty_TestCases))]
        public void WhenChangeIsRemovalAndSourceIsEmpty_ThrowsException(SetupTestCase testCase)
            => WhenChangeIsInvalidOrIncoherent_ThrowsException(
                testCase:   testCase,
                change:     TUutAdapter.CreateRemoval(
                    sourceCount:    1,
                    item:           testCase.ChangesCategories.Count),
                because:    "an item cannot be removed from an empty collection");

        [TestCaseSource(typeof(AddChangeTests), nameof(WhenSourceIsNotEmpty_TestCases))]
        public void WhenChangeIsRemovalAndSourceIsNotEmpty_BuffersChangeAndDecrementsSourceCount(SetupTestCase testCase)
        {
            var uut = PerformSetup(testCase);
            
            var priorChanges        = uut.Changes.ToArray();
            var priorSourceCount    = uut.SourceCount;
            
            var change = TUutAdapter.CreateRemoval(
                sourceCount:    uut.SourceCount,
                item:           testCase.ChangesCategories.Count);
            
            uut.AddChange(change);
            
            uut.Changes.Should().BeEquivalentTo(priorChanges.Append(change), static options => options.WithStrictOrdering(), "the given change should have been buffered.");
            WhenChangeIsRemoval_AssertCurrentTypeIsCorrect(
                uut:                uut,
                priorSourceCount:   priorSourceCount,
                priorChanges:       priorChanges,
                change:             change);
            uut.SourceCount.Should().Be(priorSourceCount - 1,   "the given change should have removed an item from the collection");
        }
        
        protected virtual void WhenChangeIsAddition_AssertCurrentTypeIsCorrect(
            SetupTestCase                                           testCase,
            ChangeSetBuilderBase<TChangeSet, TChange, TChangeType>  uut,
            IReadOnlyList<TChange>                                  priorChanges)
        {
            if (        (testCase.SourceCount is 0)
                    &&  priorChanges.All(static change => change.Category is ChangeCategory.Addition))
                uut.CurrentType.Should().Be(ChangeSetType.Reset, "additions to an empty collection should be considered a reset operation");
            else if (   (testCase.SourceCount == priorChanges.Count(static change => change.Category is ChangeCategory.Removal))
                    &&  priorChanges
                            .SkipWhile(static change => change.Category is ChangeCategory.Removal)
                            .All(static change => change.Category is ChangeCategory.Addition))
                uut.CurrentType.Should().Be(ChangeSetType.Reset, "a sequence of removals, which removes every item in a collection, followed by a sequence of additions, should be considered a reset operation");
            else
                uut.CurrentType.Should().Be(ChangeSetType.Update, "an addition that is not part of a reset operation should be considered part of an update");
        }

        protected virtual void WhenChangeIsRemoval_AssertCurrentTypeIsCorrect(
                ChangeSetBuilderBase<TChangeSet, TChange, TChangeType>  uut,
                int                                                     priorSourceCount,
                IReadOnlyList<TChange>                                  priorChanges,
                TChange                                                 change)
        {
            if (        (priorSourceCount is 1)
                    &&  priorChanges.All(static change => change.Category is ChangeCategory.Removal))
                uut.CurrentType.Should().Be(ChangeSetType.Clear, "a sequence of changes consisting of only removals, that leaves the collection empty, should be considered a clear operation");
            else
                uut.CurrentType.Should().Be(ChangeSetType.Update, "a removal that does not clear a collection should be considered part of an update operation");
        }
        
        private static void WhenChangeIsInvalidOrIncoherent_ThrowsException(
            SetupTestCase   testCase,
            TChange         change,
            string          because)
        {
            var uut = PerformSetup(testCase);
            
            var priorChangeCount    = uut.Changes.Count;
            var priorType           = uut.CurrentType;
            var priorSourceCount    = uut.SourceCount;
            
            var result = uut.Invoking(uut => uut.AddChange(change))
                .Should().Throw<ArgumentException>(because)
                .Which;
            
            Console.WriteLine(result);

            uut.Changes.Count.Should().Be(priorChangeCount, "a rejected change should restore the builder's prior state");
            uut.CurrentType.Should().Be(priorType,          "a rejected change should restore the builder's prior state");
            uut.SourceCount.Should().Be(priorSourceCount,   "a rejected change should restore the builder's prior state");
        }
    }
}
