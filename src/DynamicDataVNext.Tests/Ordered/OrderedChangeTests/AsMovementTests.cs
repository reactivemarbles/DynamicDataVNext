using System;
using System.Collections.Generic;

using AwesomeAssertions;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeTests;

[TestFixture]
public class AsMovementTests
{
    public static readonly IReadOnlyList<TestCaseData> WhenTypeIsNotMovement_TestCases
        = new[]
        {
            new TestCaseData(default(OrderedChange<int>))               .SetName("{m}(Type is None)"),
            new TestCaseData(OrderedChange.CreateInsertion(0, 1))       .SetName("{m}(Type is Insertion)"),
            new TestCaseData(OrderedChange.CreateRefreshment(1, 2))     .SetName("{m}(Type is Refreshment)"),
            new TestCaseData(OrderedChange.CreateRemoval(1, 2))         .SetName("{m}(Type is Removal)"),
            new TestCaseData(OrderedChange.CreateReplacement(1, 2, 3))  .SetName("{m}(Type is Replacement)"),
            new TestCaseData(OrderedChange.CreateUpdate(0, 1, 2, 3))    .SetName("{m}(Type is Update)")
        };
    [TestCaseSource(nameof(WhenTypeIsNotMovement_TestCases))]
    public void WhenTypeIsNotMovement_ThrowsException(OrderedChange<int> uut)
    {
        var result = FluentActions.Invoking(() =>
            {
                _ = uut.AsMovement();
            })
            .Should().Throw<InvalidOperationException>()
            .Which;
            
        result.Message.Should().Contain(nameof(OrderedChangeType.Movement));
        
        Console.WriteLine(result);
    }
    
    // Remaining scenarios are covered by CreateMovementTests
}
