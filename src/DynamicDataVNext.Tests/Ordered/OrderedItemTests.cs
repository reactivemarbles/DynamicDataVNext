using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered;

[TestFixture]
public class OrderedItemTests
{
    [Test]
    public void IndexIsNegative_ThrowsException()
    {
        var result = FluentActions.Invoking(() =>
            {
                _ = new OrderedItem<int>()
                {
                    Index   = -1, 
                    Item    = 1 
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
        var item = 1;
    
        var result = new OrderedItem<int>()
        {
            Index   = index,
            Item    = item 
        };
        
        result.Index.Should().Be(index);
        result.Item.Should().Be(item);
    }
}
