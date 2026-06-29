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
        public sealed class FromReadOnlySpansOfKeyValuePairs
            : Base
        {
            protected override KeyedChangeSet<int, int> InvokeUut(
                    IEnumerable<int> removedItems,
                    IEnumerable<int> addedItems)
                => KeyedChangeSet.CreateForReset(
                    removals:   removedItems.Select(SelectKeyValuePair).ToArray().AsSpan(),
                    additions:  addedItems.Select(SelectKeyValuePair).ToArray().AsSpan());
        }
    }
}
