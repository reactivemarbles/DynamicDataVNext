using System.Collections.Generic;
using System.Reactive.Subjects;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

public partial class WhereTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenSourceCompletesAsynchronously_TestCases
        = new[]
        {
            new TestCaseData(new DistinctItemOptions() { ItemsAreMutable = false})
                .SetName("{m}(Immutable Items)"),
            new TestCaseData(new DistinctItemOptions() { ItemsAreMutable = true })
                .SetName("{m}(Mutable Items)")
        };
    
    [TestCaseSource(nameof(WhenSourceCompletesAsynchronously_TestCases))]
    public void WhenSourceCompletesAsynchronously_CompletionPropagates(DistinctItemOptions options)
    {
        using var streamSource = new Subject<DistinctChangeSet<int>>();
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Options     = options,
            Source      = streamSource
        };
        
        using var subscription = stream.Where(IsEven)
            .ValidateChangeSets()
            .RecordItems(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().BeEmpty("there were no initial items in the collection");
        
        streamSource.OnCompleted();
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeTrue("the source cannot publish any further notifications");
        results.RecordedChangeSets.Should().BeEmpty("no change operations were performed");
    }
}
