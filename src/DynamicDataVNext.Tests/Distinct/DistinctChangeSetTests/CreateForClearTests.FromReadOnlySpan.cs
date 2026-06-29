using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

public partial class CreateForClearTests
{
    [TestFixture]
    public sealed class FromReadOnlySpan
        : Base
    {
        protected override DistinctChangeSet<int> InvokeUut(IEnumerable<int> items)
            => DistinctChangeSet.CreateForClear(items.ToArray().AsSpan());
    }
}
