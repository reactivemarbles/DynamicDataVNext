using System.Collections.Generic;

using ReactiveUI.Primitives.Signals;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

public partial class WhereTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenSourceCompletesImmediately_TestCases
        = new[]
        {
            new TestCaseData(new DistinctItemOptions() { ItemsAreMutable = false})
                .SetName("{m}(Immutable Items)"),
            new TestCaseData(new DistinctItemOptions() { ItemsAreMutable = true })
                .SetName("{m}(Mutable Items)")
        };
    
    [TestCaseSource(nameof(WhenSourceCompletesImmediately_TestCases))]
    public void WhenSourceCompletesImmediately_CompletionPropagates(DistinctItemOptions options)
    {
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Options     = options,
            Source      = Signal.Empty<DistinctChangeSet<int>>()
        };
        
        using var subscription = stream.Where(IsEven)
            .ValidateChangeSets()
            .RecordItems(out var results);

        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeTrue("the source cannot publish any further notifications");
        results.RecordedChangeSets.Should().BeEmpty("there were no initial items in the collection");
    }
}
