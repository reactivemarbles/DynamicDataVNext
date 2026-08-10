using System.Collections.Generic;

using ReactiveUI.Primitives.Signals;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

public partial class SelectTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenOptionsIsGiven_TestCases
        = new[]
        {
            WhenOptionsIsGiven_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.Deterministic },
                    resultOptions:  new() { ItemsAreMutable = false })
                .SetName("{m}(Deterministic Selection)"),
            WhenOptionsIsGiven_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.NonDeterministic },
                    resultOptions:  new() { ItemsAreMutable = false })
                .SetName("{m}(Non-Deterministic Selection)"),
            WhenOptionsIsGiven_CreateTestCase(
                    options:        new() { Type = ItemSelectionType.Mutable },
                    resultOptions:  new() { ItemsAreMutable = true })
                .SetName("{m}(Mutable Selection)")
        };

    [TestCaseSource(nameof(WhenOptionsIsGiven_TestCases))]
    public void WhenOptionsIsGiven_OptionsPropagates(
        DistinctItemSelectionOptions    options,
        DistinctItemOptions             resultOptions)
    {
        var stream = new DistinctChangeStream<int>()
        {
            Comparer    = EqualityComparer<int>.Default,
            Source      = Signal.Never<DistinctChangeSet<int>>()
        };
        
        var result = stream.Select(
            selector:   static item => item,
            options:    options);
        
        result.Options.Should().Be(resultOptions, "options given for selection should propagate downstream");
    }

    private static TestCaseData WhenOptionsIsGiven_CreateTestCase(
            DistinctItemSelectionOptions    options,
            DistinctItemOptions             resultOptions)
        => new(options, resultOptions);
}
