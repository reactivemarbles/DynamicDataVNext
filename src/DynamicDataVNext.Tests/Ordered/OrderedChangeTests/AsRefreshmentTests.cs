using System;
using System.Collections.Generic;

using AwesomeAssertions;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeTests;

[TestFixture]
public class AsRefreshmentTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenTypeIsNotRefreshment_TestCases
        = new[]
        {
            new TestCaseData(default(OrderedChange<int>))               .SetName("{m}(Type is None)"),
            new TestCaseData(OrderedChange.CreateInsertion(0, 1))       .SetName("{m}(Type is Insertion)"),
            new TestCaseData(OrderedChange.CreateMovement(0, 1, 2))     .SetName("{m}(Type is Movement)"),
            new TestCaseData(OrderedChange.CreateRemoval(1, 2))         .SetName("{m}(Type is Removal)"),
            new TestCaseData(OrderedChange.CreateReplacement(1, 2, 3))  .SetName("{m}(Type is Replacement)"),
            new TestCaseData(OrderedChange.CreateUpdate(0, 1, 2, 3))    .SetName("{m}(Type is Update)")
        };
    [TestCaseSource(nameof(WhenTypeIsNotRefreshment_TestCases))]
    public void WhenTypeIsNotRefreshment_ThrowsException(OrderedChange<int> uut)
    {
        var result = FluentActions.Invoking(() =>
            {
                _ = uut.AsRefreshment();
            })
            .Should().Throw<InvalidOperationException>()
            .Which;
            
        result.Message.Should().Contain(nameof(OrderedChangeType.Refreshment));
        
        Console.WriteLine(result);
    }
    
    // Remaining scenarios are covered by CreateRefreshmentTests
}
