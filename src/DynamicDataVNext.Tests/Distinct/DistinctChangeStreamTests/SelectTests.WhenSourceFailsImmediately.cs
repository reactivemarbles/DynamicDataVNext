namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

public partial class SelectTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenSourceFailsImmediately_TestCases
        = new[]
        {
            new TestCaseData(new DistinctItemSelectionOptions() { Type = ItemSelectionType.Deterministic })
                .SetName("{m}(Deterministic Selection)"),
            new TestCaseData(new DistinctItemSelectionOptions() { Type = ItemSelectionType.NonDeterministic })
                .SetName("{m}(Non-Deterministic Selection)")
        };
    [TestCaseSource(nameof(WhenSourceFailsImmediately_TestCases))]
    public void WhenSourceFailsImmediately_ErrorPropagates(DistinctItemSelectionOptions options)
    {
        var error = new TestException();
        
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = Signal.Throw<DistinctChangeSet<int>>(error)
        };
        
        using var subscription = stream.Select(
                selector:   static item => item,
                options:    options)
            .ValidateChangeSets()
            .RecordItems(out var results);
        
        results.Error.Should().BeSameAs(error, "errors should propagate to subscribers");
        results.RecordedChangeSets.Should().BeEmpty("an error occurred during initial subscription");
    }
}
