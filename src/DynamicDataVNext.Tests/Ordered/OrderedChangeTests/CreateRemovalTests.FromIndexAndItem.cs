using System;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeTests;

public static partial class CreateRemovalTests
{
    [TestFixture]
    public sealed class FromIndexAndItem
        : Base
    {
        [Test]
        public void IndexIsNegative_ThrowsException()
        {
            var result = FluentActions.Invoking(() =>
                {
                    _ = OrderedChange.CreateRemoval(
                        index:  -1,
                        item:   1);
                })
                .Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("index")
                .Which;
            
            Console.WriteLine(result);
        }
    
        protected override OrderedChange<int> InvokeUut(
                int index,
                int item)
            => OrderedChange.CreateRemoval(
                index:  index,
                item:   item);
    }
}
