using System;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeTests;

public static partial class CreateMovementTests
{
    [TestFixture]
    public sealed class FromIndexesAndItem
        : Base
    {
        [Test]
        public void NewIndexIsNegative_ThrowsException()
        {
            var result = FluentActions.Invoking(() =>
                {
                    _ = OrderedChange.CreateMovement(
                        oldIndex:   0,
                        newIndex:   -1,
                        item:       2);
                })
                .Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("newIndex")
                .Which;
            
            Console.WriteLine(result);
        }
    
        [Test]
        public void OldIndexIsNegative_ThrowsException()
        {
            var result = FluentActions.Invoking(() =>
                {
                    _ = OrderedChange.CreateMovement(
                        oldIndex:   -1,
                        newIndex:   1,
                        item:       2);
                })
                .Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("oldIndex")
                .Which;
            
            Console.WriteLine(result);
        }
    
        protected override OrderedChange<int> InvokeUut(
                int oldIndex,
                int newIndex,
                int item)
            => OrderedChange.CreateMovement(
                oldIndex:   oldIndex,
                newIndex:   newIndex,
                item:       item);
    }
}
