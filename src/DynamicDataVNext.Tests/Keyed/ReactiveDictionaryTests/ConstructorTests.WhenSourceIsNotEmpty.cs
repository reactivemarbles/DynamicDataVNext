namespace DynamicDataVNext.Tests.Keyed.ReactiveDictionaryTests;

public partial class ConstructorTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenSourceIsNotEmpty_TestCases
        = new[]
        {
            new TestCaseData(new ChangeOperationTestCase()
                {
                    InitialItems    = Array.Empty<KeyValuePair<string, int>>(),
                    ChangeSet       = KeyedChangeSet.CreateForReset(additions: new[] { new KeyValuePair<string, int>("1", 1) }),
                    ExpectedItems   = new[] { new KeyValuePair<string, int>("1", 1) },
                    Because         = "a single item was added to the dictionary"
                })
                .SetName("{m}(Single item added, Empty dictionary)"),
            new TestCaseData(new ChangeOperationTestCase()
                {
                    InitialItems    = Array.Empty<KeyValuePair<string, int>>(),
                    ChangeSet       = KeyedChangeSet.CreateForReset(additions: new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    }),
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    Because         = "multiple items were added to the dictionary"
                })
                .SetName("{m}(Multiple items added, Empty dictionary)"),
            new TestCaseData(new ChangeOperationTestCase()
                {
                    InitialItems    = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(KeyedChange.CreateAddition(key: "4", item: 4)),
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3),
                        new("4", 4)
                    },
                    Because         = "a single item was added to the dictionary"
                })
                .SetName("{m}(Single item added, Non-empty dictionary)"),
            new TestCaseData(new ChangeOperationTestCase()
                {
                    InitialItems    = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateAddition(key: "4", item: 4),
                        KeyedChange.CreateAddition(key: "5", item: 5),
                        KeyedChange.CreateAddition(key: "6", item: 6)
                    }),
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3),
                        new("4", 4),
                        new("5", 5),
                        new("6", 6)
                    },
                    Because         = "multiple items were added to the dictionary"
                })
                .SetName("{m}(Multiple items added, Empty dictionary)"),
            new TestCaseData(new ChangeOperationTestCase()
                {
                    InitialItems    = new[] { new KeyValuePair<string, int>("1", 1) },
                    ChangeSet       = KeyedChangeSet.CreateForClear(new[] { new KeyValuePair<string, int>("1", 1) }),
                    ExpectedItems   = Array.Empty<KeyValuePair<string, int>>(),
                    Because         = "the dictionary's only item was removed"
                })
                .SetName("{m}(Single item removed, Leaving empty dictionary)"),
            new TestCaseData(new ChangeOperationTestCase()
                {
                    InitialItems    = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    ChangeSet       = KeyedChangeSet.CreateForClear(new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    }),
                    ExpectedItems   = Array.Empty<KeyValuePair<string, int>>(),
                    Because         = "all items in the dictionary were removed"
                })
                .SetName("{m}(Multiple items removed, Leaving empty dictionary)"),
            new TestCaseData(new ChangeOperationTestCase()
                {
                    InitialItems    = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3),
                        new("4", 4)
                    },
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(KeyedChange.CreateRemoval(key: "1", item: 1)),
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("2", 2),
                        new("3", 3),
                        new("4", 4)
                    },
                    Because         = "a single item was removed from the dictionary"
                })
                .SetName("{m}(Single item removed, Leaving non-empty dictionary)"),
            new TestCaseData(new ChangeOperationTestCase()
                {
                    InitialItems    = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3),
                        new("4", 4),
                        new("5", 5),
                        new("6", 6)
                    },
                    ChangeSet       = KeyedChangeSet.CreateForUpdate(new[]
                    {
                        KeyedChange.CreateRemoval(key: "1", item: 1),
                        KeyedChange.CreateRemoval(key: "2", item: 2),
                        KeyedChange.CreateRemoval(key: "3", item: 3)
                    }),
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("4", 4),
                        new("5", 5),
                        new("6", 6)
                    },
                    Because         = "multiple items were removed from the dictionary"
                })
                .SetName("{m}(Multiple items removed, Leaving non-empty dictionary)"),
            new TestCaseData(new ChangeOperationTestCase()
                {
                    InitialItems    = new KeyValuePair<string, int>[]
                    {
                        new("1", 1),
                        new("2", 2),
                        new("3", 3)
                    },
                    ChangeSet       = KeyedChangeSet.CreateForReset(
                        removals:   new KeyValuePair<string, int>[]
                        {
                            new("1", 1),
                            new("2", 2),
                            new("3", 3)
                        },
                        additions:  new KeyValuePair<string, int>[]
                        {
                            new("4", 4),
                            new("5", 5),
                            new("6", 6)
                        }),
                    ExpectedItems   = new KeyValuePair<string, int>[]
                    {
                        new("4", 4),
                        new("5", 5),
                        new("6", 6)
                    },
                    Because         = "all items were removed from the dictionary, then new items were added"
                })
                .SetName("{m}(Multiple item reset)")
        };
    [TestCaseSource(nameof(WhenSourceIsNotEmpty_TestCases))]
    public void WhenSourceIsNotEmpty_ResultMatchesSource(ChangeOperationTestCase testCase)
    {
        using var sourceSource = new Signal<KeyedChangeSet<string, int>>();

        using var uut = new ReactiveDictionary<string, int>(
            source:     Signal
                .Return(KeyedChangeSet.CreateForReset(additions: testCase.InitialItems))
                .Concat(sourceSource),
            comparer:   EqualityComparer<string>.Default,
            options:    default);

        uut.Should().BeEquivalentTo(testCase.InitialItems, "an initial set of items was given");
        uut.Keys.Should().BeEquivalentTo(testCase.InitialItems.Select(static item => item.Key), "an initial set of items was given");
        uut.Values.Should().BeEquivalentTo(testCase.InitialItems.Select(static item => item.Value), "an initial set of items was given");
        uut.ChangeStream.KeyComparer.Should().BeSameAs(EqualityComparer<string>.Default, "no equality comparer was specified");
        uut.ChangeStream.Options.Should().Be(default(KeyedItemOptions), "no change tracking options were specified");
        
        using var collectionChangeSubscription = uut.CollectionChanged
            .RecordValues(out var collectionChangedResults);

        collectionChangedResults.Error.Should().BeNull("no errors should have occurred");
        collectionChangedResults.HasCompleted.Should().BeFalse("the source can still publish notifications");
        collectionChangedResults.RecordedValues.Should().BeEmpty("change events cannot occur during subscription");
        
        using var changeStreamSubscription = uut.ChangeStream
            .RecordItems(out var changeStreamResults);
        
        changeStreamResults.Error.Should().BeNull("no errors should have occurred");
        changeStreamResults.HasCompleted.Should().BeFalse("the source can still publish notifications");
        changeStreamResults.RecordedItems.Should().BeEquivalentTo(testCase.InitialItems, "subscribers should be initialized to match the set");
        
        sourceSource.OnNext(testCase.ChangeSet);
        
        uut.Should().BeEquivalentTo(testCase.ExpectedItems, testCase.Because);
        uut.Keys.Should().BeEquivalentTo(testCase.ExpectedItems.Select(static item => item.Key), testCase.Because);
        uut.Values.Should().BeEquivalentTo(testCase.ExpectedItems.Select(static item => item.Value), testCase.Because);
        
        collectionChangedResults.Error.Should().BeNull("no errors should have occurred");
        collectionChangedResults.HasCompleted.Should().BeFalse("the source can still publish notifications");
        collectionChangedResults.RecordedValues.Should().ContainSingle("the published changeset should have mutated the set");
        
        changeStreamResults.Error.Should().BeNull("no errors should have occurred");
        changeStreamResults.HasCompleted.Should().BeFalse("the source can still publish notifications");
        changeStreamResults.RecordedChangeSets.Should().NotBeEmpty("the published changeset should have propagated to subscribers");
        changeStreamResults.RecordedItems.Should().BeEquivalentTo(testCase.ExpectedItems, testCase.Because);
    }
}
