using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeTests;

public static partial class CreateRefreshmentTests
{
    [TestFixture]
    public sealed class FromKeyedItem
        : Base
    {
        protected override KeyedChange<int, int> InvokeUut(
                int key,
                int item)
            => KeyedChange.CreateRemoval(new KeyedItem<int, int>()
            {
                Key     = key,
                Item    = item
            });
    }
}
