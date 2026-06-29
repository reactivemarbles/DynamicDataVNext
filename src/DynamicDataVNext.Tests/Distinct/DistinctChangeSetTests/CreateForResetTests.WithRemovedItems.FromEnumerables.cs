using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

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
                        _ = DistinctChangeSet.CreateForReset(
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
                        _ = DistinctChangeSet.CreateForReset(
                            removedItems: (null as IEnumerable<int>)!,
                            addedItems:   Array.Empty<int>());
                    })
                    .Should().Throw<ArgumentNullException>()
                    .WithParameterName("removedItems")
                    .Which;
                
                Console.WriteLine(result);
            }
            
            protected override DistinctChangeSet<int> InvokeUut(
                    IEnumerable<int> removedItems,
                    IEnumerable<int> addedItems)
                => DistinctChangeSet.CreateForReset(
                    removedItems:   removedItems,
                    addedItems:     addedItems);
        }
    }
}
