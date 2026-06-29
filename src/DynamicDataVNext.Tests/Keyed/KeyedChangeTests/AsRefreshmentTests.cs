using System;
using System.Collections.Generic;

using AwesomeAssertions;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeTests;

[TestFixture]
public class AsRefreshmentTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenTypeIsNotRefreshment_TestCases
        = new[]
        {
            new TestCaseData(default(KeyedChange<int, int>))            .SetName("{m}(Type is None)"),
            new TestCaseData(KeyedChange.CreateAddition(1, 2))          .SetName("{m}(Type is Addition)"),
            new TestCaseData(KeyedChange.CreateRemoval(1, 2))           .SetName("{m}(Type is Removal)"),
            new TestCaseData(KeyedChange.CreateReplacement(1, 2, 3))    .SetName("{m}(Type is Replacement)")
        };
    [TestCaseSource(nameof(WhenTypeIsNotRefreshment_TestCases))]
    public void WhenTypeIsNotRefreshment_ThrowsException(KeyedChange<int, int> uut)
    {
        var result = FluentActions.Invoking(() =>
            {
                _ = uut.AsRefreshment();
            })
            .Should().Throw<InvalidOperationException>()
            .Which;
            
        result.Message.Should().Contain(nameof(KeyedChangeType.Refreshment));
        
        Console.WriteLine(result);
    }
    
    // Remaining scenarios are covered by CreateRefreshmentTests
}
