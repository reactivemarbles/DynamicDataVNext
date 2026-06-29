using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForAdditionTests
{
    [TestFixture]
    public sealed class FromKeyedItem
        : Base
    {
        protected override KeyedChangeSet<int, int> InvokeUut(
                int key,
                int item)
            => KeyedChangeSet.CreateForAddition(new KeyedItem<int, int>()
            {
                Key     = key,
                Item    = item
            });
    }
}
