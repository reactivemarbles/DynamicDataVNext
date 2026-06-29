using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public partial class CreateForClearTests
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
                    _ = DistinctChangeSet.CreateForClear(items: (null as IEnumerable<int>)!);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("items")
                .Which;
        
            Console.WriteLine(result);
        }

        protected override OrderedChangeSet<int> InvokeUut(IReadOnlyList<int> items)
            => OrderedChangeSet.CreateForClear(items);
    }
}
