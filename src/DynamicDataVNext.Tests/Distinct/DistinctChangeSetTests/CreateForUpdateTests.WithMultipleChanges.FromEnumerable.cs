using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

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
                        _ = DistinctChangeSet.CreateForUpdate(changes: (null as IEnumerable<DistinctChange<int>>)!);
                    })
                    .Should().Throw<ArgumentNullException>()
                    .WithParameterName("changes")
                    .Which;
            
                Console.WriteLine(result);
            }

            protected override DistinctChangeSet<int> InvokeUut(IEnumerable<DistinctChange<int>> changes)
                => DistinctChangeSet.CreateForUpdate(changes); 
        }
    }
}
