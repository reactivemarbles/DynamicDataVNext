using System.Collections.Generic;

using ReactiveUI.Primitives.Signals;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

public partial class WhereTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenSourceFailsImmediately_TestCases
        = new[]
        {
            new TestCaseData(new DistinctItemOptions() { ItemsAreMutable = false})
                .SetName("{m}(Immutable Items)"),
            new TestCaseData(new DistinctItemOptions() { ItemsAreMutable = true })
                .SetName("{m}(Mutable Items)")
        };
    
    [TestCaseSource(nameof(WhenSourceFailsImmediately_TestCases))]
    public void WhenSourceFailsImmediately_ErrorPropagates(DistinctItemOptions options)
    {
        var error = new TestException();
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Options     = options,
            Source      = Signal.Throw<DistinctChangeSet<int>>(error)
        };
        
        using var subscription = stream.Where(IsEven)
            .ValidateChangeSets()
            .RecordItems(out var results);
        
        results.Error.Should().BeSameAs(error, "errors should propagate to subscribers");
        results.RecordedChangeSets.Should().BeEmpty("an error occurred during initial subscription");
    }
}
