namespace DynamicDataVNext.Tests.Distinct.ReactiveHashSetTests;

public static partial class ChangeStreamTests
{
    public static partial class SourceTests
    {
        public partial class SubscribeTests
        {
            public static readonly IReadOnlyList<TestCaseData> WhenSetIsNotEmpty_TestCases
                = new[]
                {
                    new TestCaseData(new[] { 1 })       .SetName("{m}(Single item in set)"),
                    new TestCaseData(new[] { 1, 2, 3 }) .SetName("{m}(Multiple items in set)")
                };
            [TestCaseSource(nameof(WhenSetIsNotEmpty_TestCases))]
            public void WhenSetIsNotEmpty_PublishesReset(IReadOnlyList<int> items)
            {
                using var uut = new ReactiveHashSet<int>(source: Signal.Chain(
                    Signal.Emit(DistinctChangeSet.CreateForReset(addedItems: items)),
                    Signal.Silent<DistinctChangeSet<int>>()));
                
                var observer = new DistinctItemRecordingObserver<int>(Sequencer.Default);

                using var result = uut.ChangeStream.Source.Subscribe(observer);

                result.Should().NotBeNull();

                observer.Error.Should().BeNull("no error should have occurred");
                observer.RecordedChangeSets.Count.Should().Be(1, "an initial reset should have been published");
                observer.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Reset, "an initial reset should have been published");
                observer.RecordedChangeSets[0].AsReset().Removals.Should().BeEmpty("the initial reset should contain only initial items");
                observer.RecordedChangeSets[0].AsReset().Additions.Should().BeEquivalentTo(items, "the initial reset should contain all initial items");
                observer.HasCompleted.Should().BeFalse("the set can still be changed");
                
                uut.Should().BeEquivalentTo(items, "the set should not have been changed");
            }
        }
    }
}

