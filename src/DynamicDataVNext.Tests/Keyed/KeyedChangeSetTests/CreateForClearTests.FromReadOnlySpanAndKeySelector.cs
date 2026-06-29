using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForClearTests
{
    [TestFixture]
    public sealed class FromEnumeraFromReadOnlySpanAndKeySelectorbleAndKeySelector
        : Base
    {
        [Test]
        public void WhenKeySelectorIsNull_ThrowsException()
        {
            var result = FluentActions.Invoking(() =>
                {
                    _ = KeyedChangeSet.CreateForClear<int, int>(
                        items:          Array.Empty<int>().AsSpan(),
                        keySelector:    null!);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("keySelector")
                .Which;
        
            Console.WriteLine(result);
        }

        protected override KeyedChangeSet<int, int> InvokeUut(IEnumerable<int> items)
            => KeyedChangeSet.CreateForClear(
                items:          items.ToArray().AsSpan(),
                keySelector:    SelectKey);
    }
}
