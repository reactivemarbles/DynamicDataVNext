using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public static partial class CreateForResetTests
{
    public static partial class WithRemovedItems
    {
        [TestFixture]
        public sealed class FromEnumerables
            : Base
        {
            [Test]
            public void WhenAddedItemsIsNull_ThrowsException()
            {
                var result = FluentActions.Invoking(() =>
                    {
                        _ = OrderedChangeSet.CreateForReset(
                            removedItems: Array.Empty<int>(),
                            addedItems:   (null as IEnumerable<int>)!);
                    })
                    .Should().Throw<ArgumentNullException>()
                    .WithParameterName("addedItems")
                    .Which;
                
                Console.WriteLine(result);
            }
            
            [Test]
            public void WhenRemovedItemsIsNull_ThrowsException()
            {
                var result = FluentActions.Invoking(() =>
                    {
                        _ = OrderedChangeSet.CreateForReset(
                            removedItems: (null as IReadOnlyList<int>)!,
                            addedItems:   Array.Empty<int>());
                    })
                    .Should().Throw<ArgumentNullException>()
                    .WithParameterName("removedItems")
                    .Which;
                
                Console.WriteLine(result);
            }
            
            protected override OrderedChangeSet<int> InvokeUut(
                    IReadOnlyList<int>  removedItems,
                    IEnumerable<int>    addedItems)
                => OrderedChangeSet.CreateForReset(
                    removedItems:   removedItems,
                    addedItems:     addedItems);
        }
    }
}
