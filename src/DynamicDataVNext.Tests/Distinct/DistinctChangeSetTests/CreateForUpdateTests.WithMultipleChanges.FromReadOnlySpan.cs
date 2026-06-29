using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

public static partial class CreateForUpdateTests
{
    public static partial class WithMultipleChanges
    {
        [TestFixture]
        public sealed class FromReadOnlySpan
            : Base
        {
            protected override DistinctChangeSet<int> InvokeUut(IEnumerable<DistinctChange<int>> changes)
                => DistinctChangeSet.CreateForUpdate(changes.ToArray().AsSpan()); 
        }
    }
}
