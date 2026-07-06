using System.Collections.Generic;
using System.Reactive.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

public partial class SelectTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenSourceCompletesImmediately_TestCases
        = new[]
        {
            new TestCaseData(new DistinctItemSelectionOptions() { Type = ItemSelectionType.Deterministic })
                .SetName("{m}(Deterministic Selection)"),
            new TestCaseData(new DistinctItemSelectionOptions() { Type = ItemSelectionType.NonDeterministic })
                .SetName("{m}(Non-Deterministic Selection)")
        };

    [TestCaseSource(nameof(WhenSourceCompletesImmediately_TestCases))]
    public void WhenSourceCompletesImmediately_CompletionPropagates(DistinctItemSelectionOptions options)
    {
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = Observable.Empty<DistinctChangeSet<int>>()
        };
        
        using var subscription = stream.Select(
                selector:   static item => item,
                options:    options)
            .ValidateChangeSets()
            .RecordItems(out var results);

        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeTrue("the source cannot publish any further notifications");
        results.RecordedChangeSets.Should().BeEmpty("there were no initial items in the collection");
    }
}
