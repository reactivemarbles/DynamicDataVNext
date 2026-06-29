using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForAdditionsTests
{
    [TestFixture]
    public sealed class FromEnumerableAndKeySelector
        : Base
    {
        [Test]
        public void WhenItemsIsNull_ThrowsException()
        {
            var result = FluentActions.Invoking(() =>
                {
                    _ = KeyedChangeSet.CreateForAdditions(
                        items:          (null as IEnumerable<int>)!,
                        keySelector:    SelectKey);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("items")
                .Which;
        
            Console.WriteLine(result);
        }

        [Test]
        public void WhenKeySelectorIsNull_ThrowsException()
        {
            var result = FluentActions.Invoking(() =>
                {
                    _ = KeyedChangeSet.CreateForAdditions<int, int>(
                        items:          Array.Empty<int>(),
                        keySelector:    null!);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("keySelector")
                .Which;
        
            Console.WriteLine(result);
        }

        protected override KeyedChangeSet<int, int> InvokeUut(IEnumerable<int> items)
            => KeyedChangeSet.CreateForAdditions(
                items:          items,
                keySelector:    SelectKey);
    }
}
