using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForUpdateTests
{
    public static partial class WithMultipleChanges
    {
        [TestFixture]
        public sealed class FromReadOnlySpan
            : Base
        {
            protected override KeyedChangeSet<int, int> InvokeUut(IEnumerable<KeyedChange<int, int>> changes)
                => KeyedChangeSet.CreateForUpdate(changes.ToArray().AsSpan()); 
        }
    }
}
