using DynamicDataVNext.Tests.Keyed.CacheTestBases;

namespace DynamicDataVNext.Tests.Keyed.ObservableCacheTests;

public static partial class ChangeStreamTests
{
    public static partial class SourceTests
    {
        public partial class SubscribeTests
        {
            public static readonly IReadOnlyList<TestCaseData> WhenSetIsNotEmpty_TestCases
                = new[]
                {
                    new TestCaseData((object?)new[] { new TestItem() { Key = "1" } })
                        .SetName("{m}(Single item in set)"),
                    new TestCaseData((object?)new TestItem[]
                        {
                            new() { Key = "1" },
                            new() { Key = "2" },
                            new() { Key = "3" }
                        })
                        .SetName("{m}(Multiple items in set)")
                };
            [TestCaseSource(nameof(WhenSetIsNotEmpty_TestCases))]
            public void WhenSetIsNotEmpty_PublishesReset(IReadOnlyList<TestItem> items)
            {
                using var uut = new ObservableCache<string, TestItem>(
                    items:          items,
                    keySelector:    TestItem.SelectKey);
                
                var observer = new KeyedItemRecordingObserver<string, TestItem>(Sequencer.Default);

                using var result = uut.ChangeStream.Source.Subscribe(observer);

                result.Should().NotBeNull();

                var additions = items
                    .Select(static item => new KeyedItem<string, TestItem>()
                    {
                        Key     = item.Key,
                        Item    = item
                    })
                    .ToArray();

                observer.Error.Should().BeNull("no error should have occurred");
                observer.RecordedChangeSets.Count.Should().Be(1, "an initial reset should have been published");
                observer.RecordedChangeSets[0].Type.Should().Be(ChangeSetType.Reset, "an initial reset should have been published");
                observer.RecordedChangeSets[0].AsReset().Removals.Should().BeEmpty("the initial reset should contain only initial items");
                observer.RecordedChangeSets[0].AsReset().Additions.Should().BeEquivalentTo(additions, "the initial reset should contain all initial items");
                observer.HasCompleted.Should().BeFalse("the set can still be changed");
                
                uut.Should().BeEquivalentTo(items, "the set should not have been changed");
            }
        }
    }
}

