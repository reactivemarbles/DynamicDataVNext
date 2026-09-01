namespace DynamicDataVNext.Tests.ChangeSetBuilderBaseTests;

public static partial class ClearTests
{
    public abstract class Base<TUutAdapter, TChangeSet, TChange, TChangeType>
        where TUutAdapter : IUutAdapter<TChangeSet, TChange, TChangeType>, new()
        where TChangeSet : struct, IChangeSet<TChange, TChangeType>
        where TChange : struct, IChange<TChangeType>
        where TChangeType : Enum
    {
        [TestCaseSource(typeof(ClearTests), nameof(WhenSourceIsInitiallyEmpty_TestCases))]
        [TestCaseSource(typeof(ClearTests), nameof(WhenSourceIsInitiallyNotEmpty_TestCases))]
        public void WhenChangesIsEmpty_BuilderIsReset(
                int initialSourceCount,
                int sourceCount)
            => Always_BuilderIsReset(
                initialSourceCount: initialSourceCount,
                changes:            Array.Empty<TChange>(),
                sourceCount:        sourceCount);
        
        [TestCaseSource(typeof(ClearTests), nameof(WhenSourceIsInitiallyNotEmpty_TestCases))]
        public void WhenChangesDescribeClear_BuilderIsReset(
                int initialSourceCount,
                int sourceCount)
            => Always_BuilderIsReset(
                initialSourceCount: initialSourceCount,
                changes:            new[] { TUutAdapter.CreateRemoval(
                    sourceCount:    initialSourceCount,
                    item:           1) },
                sourceCount:        sourceCount);
        
        [TestCaseSource(typeof(ClearTests), nameof(WhenSourceIsInitiallyEmpty_TestCases))]
        [TestCaseSource(typeof(ClearTests), nameof(WhenSourceIsInitiallyNotEmpty_TestCases))]
        public void WhenChangesDescribeReset_BuilderIsReset(
                int initialSourceCount,
                int sourceCount)
            => Always_BuilderIsReset(
                initialSourceCount: initialSourceCount,
                changes:            new[] { TUutAdapter.CreateAddition(1) },
                sourceCount:        sourceCount);
        
        [TestCaseSource(typeof(ClearTests), nameof(WhenSourceIsInitiallyEmpty_TestCases))]
        [TestCaseSource(typeof(ClearTests), nameof(WhenSourceIsInitiallyNotEmpty_TestCases))]
        public void WhenChangesDescribeUpdate_BuilderIsReset(
                int initialSourceCount,
                int sourceCount)
            => Always_BuilderIsReset(
                initialSourceCount: initialSourceCount,
                changes:            new[]
                {
                    TUutAdapter.CreateAddition(1),
                    TUutAdapter.CreateRemoval(
                        sourceCount:    initialSourceCount + 1,
                        item:           2)
                },
                sourceCount:        sourceCount);

        private static void Always_BuilderIsReset(
            int                     initialSourceCount,
            IReadOnlyList<TChange>  changes,
            int                     sourceCount)
        {
            var uut = TUutAdapter.CreateUut(sourceCount: initialSourceCount);
            
            foreach (var change in changes)
                uut.AddChange(change);

            uut.Clear(sourceCount);

            uut.Changes.Count.Should().Be(0, "all buffered changes should have been removed");
            uut.CurrentType.Should().Be(ChangeSetType.Empty, "there are no buffered changes");
            uut.SourceCount.Should().Be(sourceCount, "the number of items in the source collection should have been reset to the given amount");
        }
    }
}
