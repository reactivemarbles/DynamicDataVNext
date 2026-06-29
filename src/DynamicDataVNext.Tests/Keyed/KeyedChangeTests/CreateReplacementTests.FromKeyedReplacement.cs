using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeTests;

public static partial class CreateReplacementTests
{
    [TestFixture]
    public sealed class FromKeyedItem
        : Base
    {
        protected override KeyedChange<int, int> InvokeUut(
                int key,
                int oldItem,
                int newItem)
            => KeyedChange.CreateReplacement(new KeyedReplacement<int, int>()
            {
                Key     = key,
                OldItem = oldItem,
                NewItem = newItem
            });
    }
}
