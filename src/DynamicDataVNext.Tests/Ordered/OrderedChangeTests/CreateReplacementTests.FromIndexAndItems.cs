using System;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeTests;

public static partial class CreateReplacementTests
{
    [TestFixture]
    public sealed class FromIndexAndItems
        : Base
    {
        [Test]
        public void IndexIsNegative_ThrowsException()
        {
            var result = FluentActions.Invoking(() =>
                {
                    _ = OrderedChange.CreateReplacement(
                        index:      -1,
                        oldItem:    1,
                        newItem:    2);
                })
                .Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("index")
                .Which;
            
            Console.WriteLine(result);
        }
    
        protected override OrderedChange<int> InvokeUut(
                int index,
                int oldItem,
                int newItem)
            => OrderedChange.CreateReplacement(
                index:      index,
                oldItem:    oldItem,
                newItem:    newItem);
    }
}
