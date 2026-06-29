using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public static partial class CreateForResetTests
{
    public static partial class WithoutRemovedItems
    {
        [TestFixture]
        public sealed class FromReadOnlySpan
            : Base
        {
            protected override OrderedChangeSet<int> InvokeUut(IEnumerable<int> addedItems)
                => OrderedChangeSet.CreateForReset(addedItems.ToArray().AsSpan());
        }
    }
}
