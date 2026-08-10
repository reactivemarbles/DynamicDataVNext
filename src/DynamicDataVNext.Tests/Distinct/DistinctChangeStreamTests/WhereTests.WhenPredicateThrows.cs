using System.Collections.Generic;
using System.Linq;

using ReactiveUI.Primitives.Signals;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

public partial class WhereTests
{
    public class WhenPredicateThrows_Item
    {
        public required int Id { get; init; }
        
        public TestException? Error { get; init; }
    }

    public static readonly IReadOnlyList<TestCaseData> WhenPredicateThrows_TestCases
        = new[]
        {
            new TestCaseData(
                    new[] {  new WhenPredicateThrows_Item() { Id = 1, Error = new TestException() } },
                    new DistinctItemOptions() { ItemsAreMutable = false })
                .SetName("{m})(Single immutable item, excluded by predicate)"),
            new TestCaseData(
                    new[] {  new WhenPredicateThrows_Item() { Id = 2, Error = new TestException() } },
                    new DistinctItemOptions() { ItemsAreMutable = false })
                .SetName("{m})(Single immutable item, matching predicate)"),
            new TestCaseData(
                    new[]
                    {
                        new WhenPredicateThrows_Item() { Id = 1 },
                        new WhenPredicateThrows_Item() { Id = 2, Error = new TestException() },
                        new WhenPredicateThrows_Item() { Id = 3 },
                    },
                    new DistinctItemOptions() { ItemsAreMutable = false })
                .SetName("{m})(Multiple immutable items)"),
            new TestCaseData(
                    new[] {  new WhenPredicateThrows_Item() { Id = 1, Error = new TestException() } },
                    new DistinctItemOptions() { ItemsAreMutable = true })
                .SetName("{m})(Single mutable item, excluded by predicate)"),
            new TestCaseData(
                    new[] {  new WhenPredicateThrows_Item() { Id = 2, Error = new TestException() } },
                    new DistinctItemOptions() { ItemsAreMutable = true })
                .SetName("{m})(Single mutable item, matching predicate)"),
            new TestCaseData(
                    new[]
                    {
                        new WhenPredicateThrows_Item() { Id = 1 },
                        new WhenPredicateThrows_Item() { Id = 2, Error = new TestException() },
                        new WhenPredicateThrows_Item() { Id = 3 },
                    },
                    new DistinctItemOptions() { ItemsAreMutable = true })
                .SetName("{m})(Multiple mutable items)")
        };

    [TestCaseSource(nameof(WhenPredicateThrows_TestCases))]
    public void WhenPredicateThrows_ErrorPropagates(
        IReadOnlyList<WhenPredicateThrows_Item> items,
        DistinctItemOptions                     options)
    {
        using var source = new Signal<DistinctChangeSet<WhenPredicateThrows_Item>>(); 
        
        var stream = new DistinctChangeStream<WhenPredicateThrows_Item>()
        {
            Comparer    = EqualityComparer<WhenPredicateThrows_Item>.Default,
            Source      = source
        };
        
        using var subscription = stream.Where(static item => (item.Error is null)
                ? (item.Id % 2) is 0
                : throw item.Error)
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
