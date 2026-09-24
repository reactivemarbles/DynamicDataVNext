namespace DynamicDataVNext.Tests.Keyed.ReactiveDictionaryTests;

[TestFixture]
public partial class ConstructorTests
{
    [Test]
    public void WhenSourceChangeSetIsEmpty_ChangeSetIsIgnored()
    {
        var initialItems = new KeyValuePair<string, int>[]
        {
            new("1", 1),
            new("2", 2),
            new("3", 3),
        };
        
        using var sourceSource = new Signal<KeyedChangeSet<string, int>>();

        using var uut = new ReactiveDictionary<string, int>(
            source:     Signal
                .Return(KeyedChangeSet.CreateForReset(additions: initialItems))
                .Concat(sourceSource),
            comparer:   EqualityComparer<string>.Default,
            options:    default);

        uut.Should().BeEquivalentTo(initialItems, "an initial set of items was given");
        uut.Keys.Should().BeEquivalentTo(initialItems.Select(static item => item.Key), "an initial set of items was given");
        uut.Values.Should().BeEquivalentTo(initialItems.Select(static item => item.Value), "an initial set of items was given");
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
        changeStreamResults.RecordedItems.Should().BeEquivalentTo(initialItems, options => options.WithoutStrictOrdering(), "subscribers should be initialized to match the set");
        changeStreamResults.ClearNotifications();
        
        sourceSource.OnNext(KeyedChangeSet.Empty<string, int>());
        
        uut.Should().BeEquivalentTo(initialItems, "no changes should have been made");
        uut.Keys.Should().BeEquivalentTo(initialItems.Select(static item => item.Key), "no changes should have been made");
        uut.Values.Should().BeEquivalentTo(initialItems.Select(static item => item.Value), "no changes should have been made");
        
        collectionChangedResults.Error.Should().BeNull("no errors should have occurred");
        collectionChangedResults.HasCompleted.Should().BeFalse("the source can still publish notifications");
        collectionChangedResults.RecordedValues.Should().BeEmpty("no changes should have been made");
        
        changeStreamResults.Error.Should().BeNull("no errors should have occurred");
        changeStreamResults.HasCompleted.Should().BeFalse("the source can still publish notifications");
        changeStreamResults.RecordedChangeSets.Should().BeEmpty("no changes should have been made");
    }
    
    [Test]
    public void WhenSourceCompletesAsynchronously_CompletionPropagates()
    {
        using var source = new Signal<KeyedChangeSet<string, int>>();

        using var uut = new ReactiveDictionary<string, int>(
            source:     source,
            comparer:   EqualityComparer<string>.Default,
            options:    default);
        
        using var changeStreamSourceSubscription = uut.ChangeStream.Source
            .RecordValues(out var changeStreamSourceResults);
        
        using var collectionChangedSubscription = uut.CollectionChanged
            .RecordValues(out var collectionChangedResults);

        changeStreamSourceResults.HasCompleted.Should().BeFalse("the source can still publish notifications");
        collectionChangedResults.HasCompleted.Should().BeFalse("the source can still publish notifications");

        source.OnCompleted();
        
        changeStreamSourceResults.HasCompleted.Should().BeTrue("no further notifications should occur");
        collectionChangedResults.HasCompleted.Should().BeTrue("no further notifications should occur");
    }

    [Test]
    public void WhenSourceCompletesImmediately_CompletionPropagates()
    {
        var source = Signal.Empty<KeyedChangeSet<string, int>>();

        using var uut = new ReactiveDictionary<string, int>(
            source:     source,
            comparer:   EqualityComparer<string>.Default,
            options:    default);
        
        using var changeStreamSourceSubscription = uut.ChangeStream.Source
            .RecordValues(out var changeStreamSourceResults);
        
        using var collectionChangedSubscription = uut.CollectionChanged
            .RecordValues(out var collectionChangedResults);

        changeStreamSourceResults.HasCompleted.Should().BeTrue("no further notifications should occur");
        collectionChangedResults.HasCompleted.Should().BeTrue("no further notifications should occur");
    }

    [Test]
    public void WhenSourceFailsAsynchronously_ErrorPropagates()
    {
        using var source = new Signal<KeyedChangeSet<string, int>>();

        using var uut = new ReactiveDictionary<string, int>(
            source:     source,
            comparer:   EqualityComparer<string>.Default,
            options:    default);
        
        using var changeStreamSourceSubscription = uut.ChangeStream.Source
            .RecordValues(out var changeStreamSourceResults);
        
        using var collectionChangedSubscription = uut.CollectionChanged
            .RecordValues(out var collectionChangedResults);

        changeStreamSourceResults.Error.Should().BeNull("no errors should have occurred");
        changeStreamSourceResults.HasCompleted.Should().BeFalse("the source can still publish notifications");
        collectionChangedResults.Error.Should().BeNull("no errors should have occurred");
        collectionChangedResults.HasCompleted.Should().BeFalse("the source can still publish notifications");

        var error = new TestException();
        source.OnError(error);
        
        changeStreamSourceResults.Error.Should().Be(error, "errors should propagate downstream");
        collectionChangedResults.Error.Should().Be(error, "errors should propagate downstream");
    }

    [Test]
    public void WhenSourceFailsImmediately_ErrorPropagates()
    {
        var error = new TestException();
        var source = Signal.Throw<KeyedChangeSet<string, int>>(error);

        using var uut = new ReactiveDictionary<string, int>(
            source:     source,
            comparer:   EqualityComparer<string>.Default,
            options:    default);
        
        using var changeStreamSourceSubscription = uut.ChangeStream.Source
            .RecordValues(out var changeStreamSourceResults);
        
        using var collectionChangedSubscription = uut.CollectionChanged
            .RecordValues(out var collectionChangedResults);

        changeStreamSourceResults.Error.Should().Be(error, "errors should propagate downstream");
        collectionChangedResults.Error.Should().Be(error, "errors should propagate downstream");
    }

    [Test]
    public void WhenSourceIsNull_ThrowsException()
    {
        var result = FluentActions.Invoking(() =>
            {
                using var uut = new ReactiveDictionary<string, int>(
                    source:     null!,
                    comparer:   EqualityComparer<string>.Default,
                    options:    default);
            })
            .Should().Throw<ArgumentNullException>()
            .WithParameterName("source")
            .Which;
        
        Console.WriteLine(result);
    }

    [Test]
    public void WhenSourceIsEmpty_ResultIsEmpty()
    {
        using var uut = new ReactiveDictionary<string, int>(
            source:     Signal.Empty<KeyedChangeSet<string, int>>(),
            comparer:   EqualityComparer<string>.Default,
            options:    default);

        uut.Should().BeEmpty("no initial items were given");
        uut.Keys.Should().BeEmpty("no initial items were given");
        uut.Values.Should().BeEmpty("no initial items were given");
        uut.ChangeStream.KeyComparer.Should().BeSameAs(EqualityComparer<string>.Default, "no equality comparer was specified");
        uut.ChangeStream.Options.Should().Be(default(KeyedItemOptions), "no change tracking options were specified");
    }

    [Test]
    public void WhenComparerIsGiven_ResultUsesComparer()
    {
        var comparer = EqualityComparer<string>.Create(static (x, y) => x == y);
        
        using var uut = new ReactiveDictionary<string, int>(
            source:     Signal.Empty<KeyedChangeSet<string, int>>(),
            comparer:   comparer,
            options:    default);

        uut.ChangeStream.KeyComparer.Should().BeSameAs(comparer, "a non-default equality comparer was given");
    }

    [Test]
    public void WhenOptionsIsGiven_ResultUsesOptions()
    {
        var options = new KeyedItemOptions()
        {
            ItemsAreMutable = true
        };
        
        using var uut = new ReactiveDictionary<string, int>(
            source:     Signal.Empty<KeyedChangeSet<string, int>>(),
            comparer:   EqualityComparer<string>.Default,
            options:    options);

        uut.ChangeStream.Options.Should().Be(options, "a non-default set of options was given");
    }
}
