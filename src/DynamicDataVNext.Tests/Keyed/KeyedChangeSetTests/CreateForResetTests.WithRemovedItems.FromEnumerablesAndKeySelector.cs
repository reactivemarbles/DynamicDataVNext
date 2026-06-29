using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForResetTests
{
    public static partial class WithRemovedItems
    {
        [TestFixture]
        public sealed class FromEnumerablesAndKeySelector
            : Base
        {
            [Test]
            public void WhenAddedItemsIsNull_ThrowsException()
            {
                var result = FluentActions.Invoking(() =>
                    {
                        _ = KeyedChangeSet.CreateForReset(
                            removedItems:   Array.Empty<int>(),
                            addedItems:     (null as IEnumerable<int>)!,
                            keySelector:    SelectKey);
                    })
                    .Should().Throw<ArgumentNullException>()
                    .WithParameterName("addedItems")
                    .Which;
        
                Console.WriteLine(result);
            }

            [Test]
            public void WhenKeySelectorIsNull_ThrowsException()
            {
                var result = FluentActions.Invoking(() =>
                    {
                        _ = KeyedChangeSet.CreateForReset<int, int>(
                            removedItems:   Array.Empty<int>().AsEnumerable(),
                            addedItems:     Array.Empty<int>().AsEnumerable(),
                            keySelector:    null!);
                    })
                    .Should().Throw<ArgumentNullException>()
                    .WithParameterName("keySelector")
                    .Which;
        
                Console.WriteLine(result);
            }

            [Test]
            public void WhenRemovedItemsIsNull_ThrowsException()
            {
                var result = FluentActions.Invoking(() =>
                    {
                        _ = KeyedChangeSet.CreateForReset(
                            removedItems:   (null as IEnumerable<int>)!,
                            addedItems:     Array.Empty<int>(),
                            keySelector:    SelectKey);
                    })
                    .Should().Throw<ArgumentNullException>()
                    .WithParameterName("removedItems")
                    .Which;
        
                Console.WriteLine(result);
            }

            protected override KeyedChangeSet<int, int> InvokeUut(
                    IEnumerable<int> removedItems,
                    IEnumerable<int> addedItems)
                => KeyedChangeSet.CreateForReset(
                    removedItems:   removedItems,
                    addedItems:     addedItems,
                    keySelector:    SelectKey);
        }
    }
}
