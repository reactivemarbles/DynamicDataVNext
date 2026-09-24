namespace DynamicDataVNext.Tests.Keyed.ReactiveDictionaryTests;

public static partial class ChangeStreamTests
{
    public static partial class SourceTests
    {
        public partial class SubscribeTests
        {
            public static readonly IReadOnlyList<TestCaseData> WhenDictionaryIsNotEmpty_TestCases
                = new[]
                {
                    new TestCaseData(new[] { new KeyValuePair<string, int>("1", 1) })
                        .SetName("{m}(Single item in set)"),
                    new TestCaseData(new KeyValuePair<string, int>[]
                        {
                            new("1", 1),
                            new("2", 2),
                            new("3", 3)
                        })
                        .SetName("{m}(Multiple items in set)")
                };
            [TestCaseSource(nameof(WhenDictionaryIsNotEmpty_TestCases))]
            public void WhenSetIsNotEmpty_PublishesReset(IReadOnlyList<KeyValuePair<string, int>> items)
            {
                using var uut = new ReactiveDictionary<string, int>(source: Signal.Chain(
                    Signal.Emit(KeyedChangeSet.CreateForReset(additions: items)),
                    Signal.Silent<KeyedChangeSet<string, int>>()));
                
                var observer = new KeyedItemRecordingObserver<string, int>(Sequencer.Default);

                using var result = uut.ChangeStream.Source.Subscribe(observer);

                result.Should().NotBeNull();

                observer.Error.Should().BeNull("no error should have occurred");
                observer.RecordedChangeSets.Count.Should().Be(1, "an initial reset should have been published");
                observer.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Reset, "an initial reset should have been published");
                observer.RecordedChangeSets[0].AsReset().Removals.Should().BeEmpty("the initial reset should contain only initial items");
                observer.RecordedChangeSets[0].AsReset().AdditionPairs.Should().BeEquivalentTo(items, "the initial reset should contain all initial items");
                observer.HasCompleted.Should().BeFalse("the set can still be changed");
                
                uut.Should().BeEquivalentTo(items, "the set should not have been changed");
            }
        }
    }
}

