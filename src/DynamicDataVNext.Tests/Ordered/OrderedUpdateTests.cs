using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered;

[TestFixture]
public class OrderedUpdateTests
{
    [Test]
    public void NewIndexIsNegative_ThrowsException()
    {
        var result = FluentActions.Invoking(() =>
            {
                _ = new OrderedUpdate<int>()
                {
                    NewIndex    = -1,
                    NewItem     = 2, 
                    OldIndex    = 0,
                    OldItem     = 3
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
                _ = new OrderedUpdate<int>()
                {
                    NewIndex    = 1,
                    NewItem     = 2, 
                    OldIndex    = -1,
                    OldItem     = 3
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
        var oldItem = 1;
        var newItem = 2;
    
        var result = new OrderedUpdate<int>()
        {
            NewIndex    = newIndex,
            NewItem     = newItem, 
            OldIndex    = oldIndex,
            OldItem     = oldItem
        };
        
        result.NewIndex.Should().Be(newIndex);
        result.NewItem.Should().Be(newItem);
        result.OldIndex.Should().Be(oldIndex);
        result.OldItem.Should().Be(oldItem);
    }
}
