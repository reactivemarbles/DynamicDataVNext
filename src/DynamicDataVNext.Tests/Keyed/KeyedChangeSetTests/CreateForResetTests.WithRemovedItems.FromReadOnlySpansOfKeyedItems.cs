using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForResetTests
{
    public static partial class WithRemovedItems
    {
        [TestFixture]
        public sealed class FromReadOnlySpansOfKeyedItems
            : Base
        {
            protected override KeyedChangeSet<int, int> InvokeUut(
                    IEnumerable<int> removedItems,
                    IEnumerable<int> addedItems)
                => KeyedChangeSet.CreateForReset(
                    removals:   removedItems.Select(SelectKeyedItem).ToArray().AsSpan(),
                    additions:  addedItems.Select(SelectKeyedItem).ToArray().AsSpan());
        }
    }
}
