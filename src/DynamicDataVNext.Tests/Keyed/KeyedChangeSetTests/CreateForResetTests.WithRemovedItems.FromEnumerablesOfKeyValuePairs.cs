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
        public sealed class FromEnumerablesOfKeyValuePairs
            : Base
        {
            [Test]
            public void WhenAdditionsIsNull_ThrowsException()
            {
                var result = FluentActions.Invoking(() =>
                    {
                        _ = KeyedChangeSet.CreateForReset(
                            removals:   Array.Empty<KeyValuePair<int, int>>(),
                            additions:  (null as IEnumerable<KeyValuePair<int, int>>)!);
                    })
                    .Should().Throw<ArgumentNullException>()
                    .WithParameterName("additions")
                    .Which;
        
                Console.WriteLine(result);
            }

            [Test]
            public void WhenRemovalsIsNull_ThrowsException()
            {
                var result = FluentActions.Invoking(() =>
                    {
                        _ = KeyedChangeSet.CreateForReset(
                            removals:   (null as IEnumerable<KeyValuePair<int, int>>)!,
                            additions:  Array.Empty<KeyValuePair<int, int>>());
                    })
                    .Should().Throw<ArgumentNullException>()
                    .WithParameterName("removals")
                    .Which;
        
                Console.WriteLine(result);
            }

            protected override KeyedChangeSet<int, int> InvokeUut(
                    IEnumerable<int> removedItems,
                    IEnumerable<int> addedItems)
                => KeyedChangeSet.CreateForReset(
                    removals:   removedItems.Select(SelectKeyValuePair),
                    additions:  addedItems.Select(SelectKeyValuePair));
        }
    }
}
