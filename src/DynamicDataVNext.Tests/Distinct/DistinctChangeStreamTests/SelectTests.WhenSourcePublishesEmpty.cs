namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

public partial class SelectTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenSourcePublishesEmpty_TestCases
        = new[]
        {
            new TestCaseData(new DistinctItemSelectionOptions() { Type = ItemSelectionType.Deterministic })
                .SetName("{m}(Deterministic Selection)"),
            new TestCaseData(new DistinctItemSelectionOptions() { Type = ItemSelectionType.NonDeterministic })
                .SetName("{m}(Non-Deterministic Selection)")
        };
    [TestCaseSource(nameof(WhenSourcePublishesEmpty_TestCases))]
    public void WhenSourcePublishesEmpty_NotificationDoesNotPropagate(DistinctItemSelectionOptions options)
    {
        using var source = new Signal<DistinctChangeSet<int>>(); 
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = source
        };
        
        using var subscription = stream.Select(
                selector:   static item => item,
                options:    options)
            .ValidateChangeSets()
            .RecordItems(out var results);
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().BeEmpty("there were no initial items in the collection");

        source.OnNext(DistinctChangeSet.Empty<int>());
        
        results.Error.Should().BeNull("no errors should have occurred");
        results.HasCompleted.Should().BeFalse("the source can still publish future notifications");
        results.RecordedChangeSets.Should().BeEmpty("empty changesets should not propagate");
    }    
}
