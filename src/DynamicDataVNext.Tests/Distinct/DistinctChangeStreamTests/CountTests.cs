using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

[TestFixture]
public partial class CountTests
{
    [TestCase(false,    TestName = "{m}(Source is empty)")]
    [TestCase(true,     TestName = "{m}(Source is not empty)")]
    public void Always_PublishesImmediateNotification(bool isSourceEmpty)
    {
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = isSourceEmpty
                ? Observable.Never<DistinctChangeSet<int>>()
                : Observable.Return(DistinctChangeSet.CreateForReset(new[] { 1, 2, 3 }))
                    .Concat(Observable.Never<DistinctChangeSet<int>>())
        };
        
        using var subscription = stream.Count()
            .RecordValues(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedValues.Should().ContainSingle("an initial value should always be published");
    }

    [Test]
    public void WhenSourceCompletesAsynchronously_CompletionPropagates()
    {
        using var streamSource = new Subject<DistinctChangeSet<int>>();
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = streamSource
        };
        
        using var subscription = stream.Count()
            .RecordValues(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedValues.Should().ContainSingle("an initial value should always be published");
        results.ClearNotifications();
        
        streamSource.OnCompleted();
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeTrue("the source cannot publish any further notifications");
        results.RecordedValues.Should().BeEmpty("no item changes were published");
    }

    [Test]
    public void WhenSourceCompletesImmediately_CompletionPropagates()
    {
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = Observable.Empty<DistinctChangeSet<int>>()
        };
        
        using var subscription = stream.Count()
            .RecordValues(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeTrue("the source cannot publish any further notifications");
        results.RecordedValues.Should().ContainSingle("an initial value should always be published");
    }

    [Test]
    public void WhenSourceFailsAsynchronously_ErrorPropagates()
    {
        using var streamSource = new Subject<DistinctChangeSet<int>>();
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = streamSource
        };
        
        using var subscription = stream.Count()
            .RecordValues(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedValues.Should().ContainSingle("an initial value should always be published");
        results.ClearNotifications();
        
        var error = new TestException();
        
        streamSource.OnError(error);
        
        results.Error.Should().BeSameAs(error, "errors should propagate to subscribers");
        results.RecordedValues.Should().BeEmpty("no item changes were published");
    }

    [Test]
    public void WhenSourceFailsImmediately_ErrorPropagates()
    {
        var error = new TestException();
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = Observable.Throw<DistinctChangeSet<int>>(error)
        };
        
        using var subscription = stream.Count()
            .RecordValues(out var results);
        
        results.Error.Should().BeSameAs(error, "errors should propagate to subscribers");
        results.RecordedValues.Should().BeEmpty("an error occurred during initial subscription");
    }
}
