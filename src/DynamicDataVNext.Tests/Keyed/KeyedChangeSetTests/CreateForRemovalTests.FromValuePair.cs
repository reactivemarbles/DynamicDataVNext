using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForRemovalTests
{
    [TestFixture]
    public sealed class FromKeyValuePair
        : Base
    {
        protected override KeyedChangeSet<int, int> InvokeUut(
                int key,
                int item)
            => KeyedChangeSet.CreateForRemoval(new KeyValuePair<int, int>(
                key:    key,
                value:  item));
    }
}
