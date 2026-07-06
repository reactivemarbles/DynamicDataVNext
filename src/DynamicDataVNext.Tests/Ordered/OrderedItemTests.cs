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

    [TestCase(0,            TestName = "{m}(Minimum index)")]
    [TestCase(int.MaxValue, TestName = "{m}(Maximum index)")]
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
