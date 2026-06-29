using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public static partial class CreateForRemovalsTests
{
    [TestFixture]
    public sealed class FromReadOnlySpan
        : Base
    {
        protected override OrderedChangeSet<int> InvokeUut(
                int                 index,
                IReadOnlyList<int>  items)
            => OrderedChangeSet.CreateForRemovals(
                index:  index,
                items:  items.ToArray().AsSpan());
    }
}
