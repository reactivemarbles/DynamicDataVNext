using System;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public static partial class CreateForReplacementTests
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
                    _ = OrderedChangeSet.CreateForReplacement(
                        index:      -1,
                        oldItem:    1,
                        newItem:    2);
                })
                .Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("index")
                .Which;
            
            Console.WriteLine(result);
        }

        protected override OrderedChangeSet<int> InvokeUut(
                int index,
                int oldItem,
                int newItem)
            => OrderedChangeSet.CreateForReplacement(
                index:      index,
                oldItem:    oldItem,
                newItem:    newItem);
    }
}
