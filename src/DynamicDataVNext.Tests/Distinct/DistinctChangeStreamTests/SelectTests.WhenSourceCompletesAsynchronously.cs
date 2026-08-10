namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

public partial class SelectTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenSourceCompletesAsynchronously_TestCases
        = new[]
        {
            new TestCaseData(new DistinctItemSelectionOptions() { Type = ItemSelectionType.Deterministic })
                .SetName("{m}(Deterministic Selection)"),
            new TestCaseData(new DistinctItemSelectionOptions() { Type = ItemSelectionType.NonDeterministic })
                .SetName("{m}(Non-Deterministic Selection)")
        };

    [TestCaseSource(nameof(WhenSourceCompletesAsynchronously_TestCases))]
    public void WhenSourceCompletesAsynchronously_CompletionPropagates(DistinctItemSelectionOptions options)
    {
        using var streamSource = new Signal<DistinctChangeSet<int>>();
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = streamSource
        };
        
        using var subscription = stream.Select(
                selector:   static item => item,
                options:    options)
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
