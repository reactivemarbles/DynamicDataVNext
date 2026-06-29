using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered;

[TestFixture]
public class OrderedMovementTests
{
    [Test]
    public void NewIndexIsNegative_ThrowsException()
    {
        var result = FluentActions.Invoking(() =>
            {
                _ = new OrderedMovement<int>()
                {
                    Item        = 1, 
                    NewIndex    = -1,
                    OldIndex    = 0
                };
            })
            .Should().Throw<ArgumentOutOfRangeException>()
            .Which;
        
        Console.WriteLine(result);
    }

    [Test]
    public void OldIndexIsNegative_ThrowsException()
    {
        var result = FluentActions.Invoking(() =>
            {
                _ = new OrderedMovement<int>()
                {
                    Item        = 1, 
                    NewIndex    = 0,
                    OldIndex    = -1
                };
            })
            .Should().Throw<ArgumentOutOfRangeException>()
            .Which;
        
        Console.WriteLine(result);
    }

    public static readonly IReadOnlyList<TestCaseData> Otherwise_TestCases
        = new[]
        {
            new TestCaseData(0,             0)              .SetName("{m}(Minimum indexes)"),
            new TestCaseData(int.MaxValue,  int.MaxValue)   .SetName("{m}(Maximum indexes)")
        };
    [TestCaseSource(nameof(Otherwise_TestCases))]
    public void Otherwise_ResultIsValid(
        int oldIndex,
        int newIndex)
    {
        var item = 1;
    
        var result = new OrderedMovement<int>()
        {
            Item        = item,
            NewIndex    = newIndex,
            OldIndex    = oldIndex
        };
        
        result.Item.Should().Be(item);
        result.NewIndex.Should().Be(newIndex);
        result.OldIndex.Should().Be(oldIndex);
    }
}
