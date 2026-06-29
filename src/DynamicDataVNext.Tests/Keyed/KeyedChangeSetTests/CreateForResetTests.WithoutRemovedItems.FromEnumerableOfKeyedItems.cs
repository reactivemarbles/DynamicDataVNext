using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForResetTests
{
    public static partial class WithoutRemovedItems
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
                        _ = KeyedChangeSet.CreateForReset(additions: (null as IEnumerable<KeyedItem<int, int>>)!);
                    })
                    .Should().Throw<ArgumentNullException>()
                    .WithParameterName("additions")
                    .Which;
        
                Console.WriteLine(result);
            }

            protected override KeyedChangeSet<int, int> InvokeUut(IEnumerable<int> addedItems)
                => KeyedChangeSet.CreateForReset(additions: addedItems.Select(SelectKeyedItem));
        }
    }
}
