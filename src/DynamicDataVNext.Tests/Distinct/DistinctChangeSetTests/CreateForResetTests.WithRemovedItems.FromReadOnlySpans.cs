using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

public static partial class CreateForResetTests
{
    public static partial class WithRemovedItems
    {
        [TestFixture]
        public sealed class FromReadOnlySpans
            : Base
        {
            protected override DistinctChangeSet<int> InvokeUut(
                    IEnumerable<int> removedItems,
                    IEnumerable<int> addedItems)
                => DistinctChangeSet.CreateForReset(
                    removedItems:   removedItems.ToArray().AsSpan(),
                    addedItems:     addedItems.ToArray().AsSpan());
        }
    }
}
