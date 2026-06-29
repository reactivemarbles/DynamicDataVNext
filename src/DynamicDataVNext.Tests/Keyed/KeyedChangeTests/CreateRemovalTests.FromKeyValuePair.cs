using System.Collections.Generic;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeTests;

public static partial class CreateRemovalTests
{
    [TestFixture]
    public sealed class FromKeyValuePair
        : Base
    {
        protected override KeyedChange<int, int> InvokeUut(
                int key,
                int item)
            => KeyedChange.CreateRefreshment(new KeyValuePair<int, int>(
                key:    key,
                value:  item));
    }
}
