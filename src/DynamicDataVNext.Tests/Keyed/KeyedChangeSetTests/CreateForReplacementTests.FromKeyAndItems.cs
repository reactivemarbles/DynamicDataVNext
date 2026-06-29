using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForReplacementTests
{
    [TestFixture]
    public sealed class FromKeyAndItems
        : Base
    {
        protected override KeyedChangeSet<int, int> InvokeUut(
                int key,
                int oldItem,
                int newItem)
            => KeyedChangeSet.CreateForReplacement(
                key:        key,
                oldItem:    oldItem,
                newItem:    newItem);
    }
}
