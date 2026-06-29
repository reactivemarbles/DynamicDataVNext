using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForRemovalsTests
{
    [TestFixture]
    public sealed class FromEnumerableOfKeyedItems
        : Base
    {
        [Test]
        public void WhenAdditionsIsNull_ThrowsException()
        {
            var result = FluentActions.Invoking(() =>
                {
                    _ = KeyedChangeSet.CreateForRemovals(removals: (null as IEnumerable<KeyedItem<int, int>>)!);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("removals")
                .Which;
        
            Console.WriteLine(result);
        }

        protected override KeyedChangeSet<int, int> InvokeUut(IEnumerable<int> items)
            => KeyedChangeSet.CreateForRemovals(removals: items.Select(SelectKeyedItem));
    }
}
