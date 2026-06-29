using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public static partial class CreateForRemovalsTests
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
                    _ = OrderedChangeSet.CreateForRemovals(
                        index:  0,
                        items:  (null as IReadOnlyList<int>)!);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("items")
                .Which;
        
            Console.WriteLine(result);
        }

        protected override OrderedChangeSet<int> InvokeUut(
                int                 index,
                IReadOnlyList<int>  items)
            => OrderedChangeSet.CreateForRemovals(
                index:  index,
                items:  items);
    }
}
