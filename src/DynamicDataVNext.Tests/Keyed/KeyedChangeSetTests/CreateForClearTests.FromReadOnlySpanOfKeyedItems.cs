using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForClearTests
{
    [TestFixture]
    public sealed class FromReadOnlySpanOfKeyedItems
        : Base
    {
        protected override KeyedChangeSet<int, int> InvokeUut(IEnumerable<int> items)
            => KeyedChangeSet.CreateForClear(removals: items.Select(SelectKeyedItem).ToArray().AsSpan());
    }
}
