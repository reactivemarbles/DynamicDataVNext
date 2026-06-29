using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForReplacementTests
{
    [TestFixture]
    public sealed class FromKeyedItem
        : Base
    {
        protected override KeyedChangeSet<int, int> InvokeUut(
                int key,
                int oldItem,
                int newItem)
            => KeyedChangeSet.CreateForReplacement(new KeyedReplacement<int, int>()
            {
                Key     = key,
                OldItem = oldItem,
                NewItem = newItem
            });
    }
}
