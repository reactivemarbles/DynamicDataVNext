using System;
using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

public static partial class CreateForAdditionsTests
{
    [TestFixture]
    public sealed class FromEnumerable
        : Base
    {
        [Test]
        public void WhenItemsIsNull_ThrowsException()
        {
            var result = FluentActions.Invoking(() =>
                {
                    _ = DistinctChangeSet.CreateForAdditions(items: (null as IEnumerable<int>)!);
                })
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("items")
                .Which;
        
            Console.WriteLine(result);
        }

        protected override DistinctChangeSet<int> InvokeUut(IEnumerable<int> items)
            => DistinctChangeSet.CreateForAdditions(items);
    }
}
