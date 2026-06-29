using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForAdditionTests
{
    [TestFixture]
    public sealed class FromKeyValuePair
        : Base
    {
        protected override KeyedChangeSet<int, int> InvokeUut(
                int key,
                int item)
            => KeyedChangeSet.CreateForAddition(new KeyValuePair<int, int>(
                key:    key,
                value:  item));
    }
}
