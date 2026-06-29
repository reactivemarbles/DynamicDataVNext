using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public static partial class CreateForUpdateTests
{
    public static partial class WithMultipleChanges
    {
        [TestFixture]
        public sealed class FromEnumerable
            : Base
        {
            [Test]
            public void WhenChangesIsNull_ThrowsException()
            {
                var result = FluentActions.Invoking(() =>
                    {
                        _ = OrderedChangeSet.CreateForUpdate(changes: (null as IEnumerable<OrderedChange<int>>)!);
                    })
                    .Should().Throw<ArgumentNullException>()
                    .WithParameterName("changes")
                    .Which;
            
                Console.WriteLine(result);
            }

            protected override OrderedChangeSet<int> InvokeUut(IEnumerable<OrderedChange<int>> changes)
                => OrderedChangeSet.CreateForUpdate(changes); 
        }
    }
}
