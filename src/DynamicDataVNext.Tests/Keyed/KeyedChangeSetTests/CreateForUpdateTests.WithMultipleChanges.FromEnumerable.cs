using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForUpdateTests
{
    public static partial class WithMultipleChanges
    {
        [TestFixture]
        public sealed class FromEnumerable
            : Base
        {
            [Test]
            public void WhenChangesIsNull_ThrowsException()
            {
                var result = FluentActions.Invoking(() =>
                    {
                        _ = KeyedChangeSet.CreateForUpdate(changes: (null as IEnumerable<KeyedChange<int, int>>)!);
                    })
                    .Should().Throw<ArgumentNullException>()
                    .WithParameterName("changes")
                    .Which;
            
                Console.WriteLine(result);
            }

            protected override KeyedChangeSet<int, int> InvokeUut(IEnumerable<KeyedChange<int, int>> changes)
                => KeyedChangeSet.CreateForUpdate(changes); 
        }
    }
}
