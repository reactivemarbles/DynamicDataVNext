using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForResetTests
{
    public static partial class WithoutRemovedItems
    {
        [TestFixture]
        public sealed class FromReadOnlySpanAndKeySelector
            : Base
        {
            [Test]
            public void WhenKeySelectorIsNull_ThrowsException()
            {
                var result = FluentActions.Invoking(() =>
                    {
                        _ = KeyedChangeSet.CreateForReset<int, int>(
                            addedItems:     Array.Empty<int>().AsSpan(),
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
