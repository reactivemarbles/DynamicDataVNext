using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public static partial class CreateForResetTests
{
    public static partial class WithRemovedItems
    {
        [TestFixture]
        public sealed class FromReadOnlySpans
            : Base
        {
            protected override OrderedChangeSet<int> InvokeUut(
                    IReadOnlyList<int>  removedItems,
                    IEnumerable<int>    addedItems)
                => OrderedChangeSet.CreateForReset(
                    removedItems:   removedItems.ToArray().AsSpan(),
                    addedItems:     addedItems.ToArray().AsSpan());
        }
    }
}
