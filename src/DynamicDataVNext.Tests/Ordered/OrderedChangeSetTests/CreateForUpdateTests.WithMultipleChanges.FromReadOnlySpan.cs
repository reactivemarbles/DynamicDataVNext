using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public static partial class CreateForUpdateTests
{
    public static partial class WithMultipleChanges
    {
        [TestFixture]
        public sealed class FromReadOnlySpan
            : Base
        {
            protected override OrderedChangeSet<int> InvokeUut(IEnumerable<OrderedChange<int>> changes)
                => OrderedChangeSet.CreateForUpdate(changes.ToArray().AsSpan()); 
        }
    }
}
