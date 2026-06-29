using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForResetTests
{
    public static partial class WithRemovedItems
    {
        [TestFixture]
        public sealed class FromReadOnlySpansAndKeySelector
            : Base
        {
            [Test]
            public void WhenKeySelectorIsNull_ThrowsException()
            {
                var result = FluentActions.Invoking(() =>
                    {
                        _ = KeyedChangeSet.CreateForReset<int, int>(
                            removedItems:   Array.Empty<int>().AsSpan(),
                            addedItems:     Array.Empty<int>().AsSpan(),
                            keySelector:    null!);
                    })
                    .Should().Throw<ArgumentNullException>()
                    .WithParameterName("keySelector")
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
