using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public static partial class CreateForInsertionsTests
{
    [TestFixture]
    public sealed class FromEnumerable
        : Base
    {
        [Test]
        public void WhenItemsIsNull_ThrowsException()
        {
            var result = FluentActions.Invoking(() =>
                {
                    _ = OrderedChangeSet.CreateForInsertions(
                        index:  0,
                        items:  (null as IEnumerable<int>)!);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("items")
                .Which;
        
            Console.WriteLine(result);
        }

        protected override OrderedChangeSet<int> InvokeUut(
                int                 index,
                IEnumerable<int>    items)
            => OrderedChangeSet.CreateForInsertions(
                index:  index,
                items:  items);
    }
}
