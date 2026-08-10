using System.Collections.Generic;

using ReactiveUI.Primitives;
using ReactiveUI.Primitives.Signals;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

public partial class SelectTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenSelectionIsNotDeterministic_TestCases
        = new[]
        {
            new TestCaseData(new DistinctItemSelectionOptions() { Type = ItemSelectionType.NonDeterministic })
                .SetName("{m}(Non-Deterministic Selection)"),
            new TestCaseData(new DistinctItemSelectionOptions() { Type = ItemSelectionType.Mutable })
                .SetName("{m}(Mutable Selection)")
        };

    [TestCaseSource(nameof(WhenSelectionIsNotDeterministic_TestCases))]
    public void WhenSelectionIsNotDeterministic_SelectorIsOnlyInvokedOncePerItem(DistinctItemSelectionOptions options)
    {
        using var source = new Signal<DistinctChangeSet<int>>(); 
        
        var items = new[]
        {
            1, 2, 3
        };
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = Signal.Concat(
                Signal.Return(DistinctChangeSet.CreateForReset(items)),
                source)
        };
        
        var selectorInvocations = new List<int>();

        using var subscription = stream.Select(
                selector:   item =>
                {
                    selectorInvocations.Add(item);
                    return item;
                },
                options:    options)
            .ValidateChangeSets()
            .RecordItems(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().ContainSingle("there were initial items in the collection");
        results.RecordedItems.Should().BeEquivalentTo(
            expectation:    items,
            config:         options => options.WithoutStrictOrdering(),
            because:        "all changes should propagate downstream, after having the selector applied");
        results.ClearNotifications();

        selectorInvocations.Should().BeEquivalentTo(
            expectation:    items,
            config:         options => options.WithoutStrictOrdering(),
            because:        "the selector should have been invoked for each added item");
        selectorInvocations.Clear();
        
        source.OnNext(DistinctChangeSet.CreateForClear(items));

        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().ContainSingle("a single change operation was performed");
        results.RecordedItems.Should().BeEmpty("all changes should propagate downstream, after having the selector applied");
        
        selectorInvocations.Should().BeEmpty("non-deterministic selectors should only be invoked once, per-item");
    }
}
