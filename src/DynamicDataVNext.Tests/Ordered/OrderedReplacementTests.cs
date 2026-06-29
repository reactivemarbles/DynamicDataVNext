using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered;

[TestFixture]
public class OrderedReplacementTests
{
    [Test]
    public void IndexIsNegative_ThrowsException()
    {
        var result = FluentActions.Invoking(() =>
            {
                _ = new OrderedReplacement<int>()
                {
                    Index   = -1,
                    NewItem = 1,
                    OldItem = 2
                };
            })
            .Should().Throw<ArgumentOutOfRangeException>()
            .Which;
        
        Console.WriteLine(result);
    }

    public static readonly IReadOnlyList<TestCaseData> Otherwise_TestCases
        = new[]
        {
            new TestCaseData(0)             .SetName("{m}(Minimum index)"),
            new TestCaseData(int.MaxValue)  .SetName("{m}(Maximum index)")
        };
    [TestCaseSource(nameof(Otherwise_TestCases))]
    public void Otherwise_ResultIsValid(int index)
    {
        var oldItem = 1;
        var newItem = 2;
    
        var result = new OrderedReplacement<int>()
        {
            Index   = index,
            NewItem = newItem,
            OldItem = oldItem
        };
        
        result.Index.Should().Be(index);
        result.NewItem.Should().Be(newItem);
        result.OldItem.Should().Be(oldItem);
    }
}
