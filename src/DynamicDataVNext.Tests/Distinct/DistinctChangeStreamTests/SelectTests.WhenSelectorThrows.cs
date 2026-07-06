using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

public partial class SelectTests
{
    public class WhenSelectorThrows_Item
    {
        public required int Id { get; init; }
        
        public TestException? Error { get; init; }
    }

    public static readonly IReadOnlyList<TestCaseData> WhenSelectorThrows_TestCases
        = new[]
        {
            new TestCaseData(
                    new[] {  new WhenSelectorThrows_Item() { Id = 1, Error = new TestException() } },
                    new DistinctItemSelectionOptions() { Type = ItemSelectionType.Deterministic })
                .SetName("{m})(Single item, Deterministic selection)"),
            new TestCaseData(
                    new[]
                    {
                        new WhenSelectorThrows_Item() { Id = 1 },
                        new WhenSelectorThrows_Item() { Id = 2, Error = new TestException() },
                        new WhenSelectorThrows_Item() { Id = 3 },
                    },
                    new DistinctItemSelectionOptions() { Type = ItemSelectionType.Deterministic })
                .SetName("{m})(Multiple items, Deterministic selection)"),
            new TestCaseData(
                    new[] {  new WhenSelectorThrows_Item() { Id = 1, Error = new TestException() } },
                    new DistinctItemSelectionOptions() { Type = ItemSelectionType.NonDeterministic })
                .SetName("{m})(Single item, Non-deterministic selection)"),
            new TestCaseData(
                    new[]
                    {
                        new WhenSelectorThrows_Item() { Id = 1 },
                        new WhenSelectorThrows_Item() { Id = 2, Error = new TestException() },
                        new WhenSelectorThrows_Item() { Id = 3 },
                    },
                    new DistinctItemSelectionOptions() { Type = ItemSelectionType.NonDeterministic })
                .SetName("{m})(Multiple items, Non-deterministic selection)"),
        };

    [TestCaseSource(nameof(WhenSelectorThrows_TestCases))]
    public void WhenSelectorThrows_ErrorPropagates(
        IReadOnlyList<WhenSelectorThrows_Item>  items,
        DistinctItemSelectionOptions            options)
    {
        using var source = new Subject<DistinctChangeSet<WhenSelectorThrows_Item>>(); 
        
        var stream = new DistinctChangeStream<WhenSelectorThrows_Item>()
        {
            Comparer    = EqualityComparer<WhenSelectorThrows_Item>.Default,
            Source      = source
        };
        
        using var subscription = stream.Select(
                selector:   static item => (item.Error is null)
                    ? item
                    : throw item.Error,
                options:    options)
            .ValidateChangeSets()
            .RecordItems(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().BeEmpty("there were no initial items in the collection");

        source.OnNext(DistinctChangeSet.CreateForReset(items));

        var expectedError = items
            .Select(static item => item.Error)
            .First(static error => error is not null);
        results.Error.Should().Be(expectedError, "consumer errors should propagate downstream");
        results.RecordedChangeSets.Should().BeEmpty("an error occurred during processing of changes");
    }
}
