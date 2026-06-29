using System;
using System.Collections.Generic;

using AwesomeAssertions;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeTests;

[TestFixture]
public class AsAdditionTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenTypeIsNotAddition_TestCases
        = new[]
        {
            new TestCaseData(default(KeyedChange<int, int>))            .SetName("{m}(Type is None)"),
            new TestCaseData(KeyedChange.CreateRefreshment(1, 2))       .SetName("{m}(Type is Refreshment)"),
            new TestCaseData(KeyedChange.CreateRemoval(1, 2))           .SetName("{m}(Type is Removal)"),
            new TestCaseData(KeyedChange.CreateReplacement(1, 2, 3))    .SetName("{m}(Type is Replacement)")
        };
    [TestCaseSource(nameof(WhenTypeIsNotAddition_TestCases))]
    public void WhenTypeIsNotAddition_ThrowsException(KeyedChange<int, int> uut)
    {
        var result = FluentActions.Invoking(() =>
            {
                _ = uut.AsAddition();
            })
            .Should().Throw<InvalidOperationException>()
            .Which;
            
        result.Message.Should().Contain(nameof(KeyedChangeType.Addition));
        
        Console.WriteLine(result);
    }
    
    // Remaining scenarios are covered by CreateAdditionTests
}
