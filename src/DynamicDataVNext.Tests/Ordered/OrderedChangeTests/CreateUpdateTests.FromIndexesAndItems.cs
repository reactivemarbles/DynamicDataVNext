using System;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeTests;

public static partial class CreateUpdateTests
{
    [TestFixture]
    public sealed class FromIndexesAndItems
        : Base
    {
        [Test]
        public void NewIndexIsNegative_ThrowsException()
        {
            var result = FluentActions.Invoking(() =>
                {
                    _ = OrderedChange.CreateUpdate(
                        oldIndex:   0,
                        oldItem:    1,
                        newIndex:   -1,
                        newItem:    3);
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
                    _ = OrderedChange.CreateUpdate(
                        oldIndex:   -1,
                        oldItem:    1,
                        newIndex:   2,
                        newItem:    3);
                })
                .Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("oldIndex")
                .Which;
            
            Console.WriteLine(result);
        }
    
        protected override OrderedChange<int> InvokeUut(
                int oldIndex,
                int oldItem,
                int newIndex,
                int newItem)
            => OrderedChange.CreateUpdate(
                oldIndex:   oldIndex,
                oldItem:    oldItem,
                newIndex:   newIndex,
                newItem:    newItem);
    }
}
