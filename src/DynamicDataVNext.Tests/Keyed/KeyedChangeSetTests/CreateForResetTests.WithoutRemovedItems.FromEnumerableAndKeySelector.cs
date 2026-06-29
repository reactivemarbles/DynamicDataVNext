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
        public sealed class FromEnumerableAndKeySelector
            : Base
        {
            [Test]
            public void WhenAddedItemsIsNull_ThrowsException()
            {
                var result = FluentActions.Invoking(() =>
                    {
                        _ = KeyedChangeSet.CreateForReset(
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
                            addedItems:     Array.Empty<int>().AsEnumerable(),
                            keySelector:    null!);
                    })
                    .Should().Throw<ArgumentNullException>()
                    .WithParameterName("keySelector")
                    .Which;
        
                Console.WriteLine(result);
            }

            protected override KeyedChangeSet<int, int> InvokeUut(IEnumerable<int> addedItems)
                => KeyedChangeSet.CreateForReset(
                    addedItems:     addedItems,
                    keySelector:    SelectKey);
        }
    }
}
