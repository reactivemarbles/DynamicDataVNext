using System;

using AwesomeAssertions;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public static partial class CreateForInsertionTests
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
                    _ = OrderedChangeSet.CreateForInsertion(
                        index:  -1,
                        item:   1);
                })
                .Should().Throw<ArgumentOutOfRangeException>()
                .WithParameterName("index")
                .Which;
            
            Console.WriteLine(result);
        }

        protected override OrderedChangeSet<int> InvokeUut(
                int index,
                int item)
            => OrderedChangeSet.CreateForInsertion(
                index:  index,
                item:   item);
    }
}
