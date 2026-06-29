using System;
using System.Collections.Generic;

using AwesomeAssertions;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeTests;

[TestFixture]
public class AsRemovalTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenTypeIsNotRemoval_TestCases
        = new[]
        {
            new TestCaseData(default(KeyedChange<int, int>))            .SetName("{m}(Type is None)"),
            new TestCaseData(KeyedChange.CreateAddition(1, 2))          .SetName("{m}(Type is Addition)"),
            new TestCaseData(KeyedChange.CreateRefreshment(1, 2))       .SetName("{m}(Type is Refreshment)"),
            new TestCaseData(KeyedChange.CreateReplacement(1, 2, 3))    .SetName("{m}(Type is Replacement)")
        };
    [TestCaseSource(nameof(WhenTypeIsNotRemoval_TestCases))]
    public void WhenTypeIsNotRemoval_ThrowsException(KeyedChange<int, int> uut)
    {
        var result = FluentActions.Invoking(() =>
            {
                _ = uut.AsRemoval();
            })
            .Should().Throw<InvalidOperationException>()
            .Which;
            
        result.Message.Should().Contain(nameof(KeyedChangeType.Removal));
        
        Console.WriteLine(result);
    }
    
    // Remaining scenarios are covered by CreateRemovalTests
}
