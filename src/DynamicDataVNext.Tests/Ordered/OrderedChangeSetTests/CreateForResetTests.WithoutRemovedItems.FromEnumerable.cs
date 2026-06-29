using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public static partial class CreateForResetTests
{
    public static partial class WithoutRemovedItems
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
                        _ = DistinctChangeSet.CreateForReset(addedItems: (null as IEnumerable<int>)!);
                    })
                    .Should().Throw<ArgumentNullException>()
                    .WithParameterName("addedItems")
                    .Which;
                
                Console.WriteLine(result);
            }

            protected override OrderedChangeSet<int> InvokeUut(IEnumerable<int> addedItems)
                => OrderedChangeSet.CreateForReset(addedItems);
        }
    }
}
