using System;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public static partial class CreateForMovementTests
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
                    _ = OrderedChangeSet.CreateForMovement(
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
                    _ = OrderedChangeSet.CreateForMovement(
                        oldIndex:   -1,
                        newIndex:   1,
                        item:       2);
                })
                .Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("oldIndex")
                .Which;
            
            Console.WriteLine(result);
        }

        protected override OrderedChangeSet<int> InvokeUut(
                int oldIndex,
                int newIndex,
                int item)
            => OrderedChangeSet.CreateForMovement(
                oldIndex:   oldIndex,
                newIndex:   newIndex,
                item:       item);
    }
}
