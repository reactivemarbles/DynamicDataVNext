using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeTests;

public static partial class CreateReplacementTests
{
    [TestFixture]
    public sealed class FromKeyAndItems
        : Base
    {
        protected override KeyedChange<int, int> InvokeUut(
                int key,
                int oldItem,
                int newItem)
            => KeyedChange.CreateReplacement(
                key:        key,
                oldItem:    oldItem,
                newItem:    newItem);
    }
}
