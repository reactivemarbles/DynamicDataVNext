using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ReactiveHashSetTests;

[TestFixture]
public partial class ConstructorTests
{
    [Test]
    public void WhenSourceChangeSetIsEmpty_ChangeSetIsIgnored()
    {
        var initialItems = new[] { 1, 2, 3 };
        
        using var sourceSource = new Subject<DistinctChangeSet<int>>();

        using var uut = new ReactiveHashSet<int>(
            source:     Observable
                .Return(DistinctChangeSet.CreateForReset(addedItems: initialItems))
                .Concat(sourceSource),
            comparer:   EqualityComparer<int>.Default,
            options:    default);

        uut.Should().BeEquivalentTo(initialItems, options => options.WithoutStrictOrdering(), "an initial set of items was given");
        uut.ChangeStream.Comparer.Should().BeSameAs(EqualityComparer<int>.Default, "no equality comparer was specified");
        uut.ChangeStream.Options.Should().Be(default(DistinctItemOptions), "no change tracking options were specified");
        
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
        
        sourceSource.OnNext(DistinctChangeSet.Empty<int>());
        
        uut.Should().BeEquivalentTo(initialItems, options => options.WithoutStrictOrdering(), "no changes should have been made");
        
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
        using var source = new Subject<DistinctChangeSet<int>>();

        using var uut = new ReactiveHashSet<int>(
            source:     source,
            comparer:   EqualityComparer<int>.Default,
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
        var source = Observable.Empty<DistinctChangeSet<int>>();

        using var uut = new ReactiveHashSet<int>(
            source:     source,
            comparer:   EqualityComparer<int>.Default,
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
        using var source = new Subject<DistinctChangeSet<int>>();

        using var uut = new ReactiveHashSet<int>(
            source:     source,
            comparer:   EqualityComparer<int>.Default,
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
        var source = Observable.Throw<DistinctChangeSet<int>>(error);

        using var uut = new ReactiveHashSet<int>(
            source:     source,
            comparer:   EqualityComparer<int>.Default,
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
                using var uut = new ReactiveHashSet<int>(
                    source:     null!,
                    comparer:   EqualityComparer<int>.Default,
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
        using var uut = new ReactiveHashSet<int>(
            source:     Observable.Empty<DistinctChangeSet<int>>(),
            comparer:   EqualityComparer<int>.Default,
            options:    default);

        uut.Should().BeEmpty("no initial items were given");
        uut.ChangeStream.Comparer.Should().BeSameAs(EqualityComparer<int>.Default, "no equality comparer was specified");
        uut.ChangeStream.Options.Should().Be(default(DistinctItemOptions), "no change tracking options were specified");
    }

    [Test]
    public void WhenComparerIsGiven_ResultUsesComparer()
    {
        var comparer = EqualityComparer<int>.Create(static (x, y) => x == y);
        
        using var uut = new ReactiveHashSet<int>(
            source:     Observable.Empty<DistinctChangeSet<int>>(),
            comparer:   comparer,
            options:    default);

        uut.ChangeStream.Comparer.Should().BeSameAs(comparer, "a non-default equality comparer was given");
    }

    [Test]
    public void WhenOptionsIsGiven_ResultUsesOptions()
    {
        var options = new DistinctItemOptions()
        {
            ItemsAreMutable = true,
        };
        
        using var uut = new ReactiveHashSet<int>(
            source:     Observable.Empty<DistinctChangeSet<int>>(),
            comparer:   EqualityComparer<int>.Default,
            options:    options);

        uut.ChangeStream.Options.Should().Be(options, "a non-default set of options was given");
    }
}
