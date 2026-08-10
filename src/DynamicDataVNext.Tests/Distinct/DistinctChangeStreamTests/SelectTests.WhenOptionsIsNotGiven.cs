using System.Reflection;



namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

public partial class SelectTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenOptionsIsNotGiven_TestCases
        = new[]
        {
            WhenOptionsIsNotGiven_CreateTestCase(
                    tOut:           typeof(object),
                    streamOptions:  new() { ItemsAreMutable = true },
                    resultOptions:  new() { ItemsAreMutable = true },
                    because:        "we assume that mutability propagates, when it can")
                .SetName("{m}(Mutable Inputs, Potentially Mutable Outputs)"),
            WhenOptionsIsNotGiven_CreateTestCase(
                    tOut:           typeof(int),
                    streamOptions:  new() { ItemsAreMutable = true },
                    resultOptions:  new() { ItemsAreMutable = false },
                    because:        "value types are always immutable")
                .SetName("{m}(Mutable Inputs, Immutable Outputs"),
            WhenOptionsIsNotGiven_CreateTestCase(
                    tOut:           typeof(int),
                    streamOptions:  new() { ItemsAreMutable = false },
                    resultOptions:  new() { ItemsAreMutable = false },
                    because:        "value types are always immutable")
                .SetName("{m}(Immutable Inputs, Immutable Outputs"),
            WhenOptionsIsNotGiven_CreateTestCase(
                    tOut:           typeof(object),
                    streamOptions:  new() { ItemsAreMutable = false },
                    resultOptions:  new() { ItemsAreMutable = false },
                    because:        "we assume that immutability propagates")
                .SetName("{m}(Mutable Inputs, Potentially Mutable Outputs")
        };

    [TestCaseSource(nameof(WhenOptionsIsNotGiven_TestCases))]
    public void WhenOptionsIsNotGiven_ResultOptionsAreInferred(
            Type                tOut,
            DistinctItemOptions streamOptions,
            DistinctItemOptions resultOptions,
            string              because)
        => GetType()
            .GetMethod(nameof(WhenOptionsIsNotGiven_ResultOptionsAreInferred), BindingFlags.NonPublic | BindingFlags.Static)!
            .MakeGenericMethod(tOut)
            .Invoke(this, new object[] { streamOptions, resultOptions, because });

    private static TestCaseData WhenOptionsIsNotGiven_CreateTestCase(
            Type                tOut,
            DistinctItemOptions streamOptions,
            DistinctItemOptions resultOptions,
            string              because)
        => new(tOut, streamOptions, resultOptions, because);

    private static void WhenOptionsIsNotGiven_ResultOptionsAreInferred<TOut>(
        DistinctItemOptions streamOptions,
        DistinctItemOptions resultOptions,
        string              because)
    {
        var stream = new DistinctChangeStream<TOut>()
        {
            Comparer    = EqualityComparer<TOut>.Default,
            Options     = streamOptions,
            Source      = Signal.Never<DistinctChangeSet<TOut>>()
        };
        
        var result = stream.Select(static item => item);
        
        result.Options.Should().Be(resultOptions, because);
    }
}
