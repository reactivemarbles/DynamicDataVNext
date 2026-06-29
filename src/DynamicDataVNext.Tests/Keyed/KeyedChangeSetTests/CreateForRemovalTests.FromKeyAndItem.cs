using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForRemovalTests
{
    [TestFixture]
    public sealed class FromKeyAndItem
        : Base
    {
        protected override KeyedChangeSet<int, int> InvokeUut(
                int key,
                int item)
            => KeyedChangeSet.CreateForRemoval(
                key:    key,
                item:   item);
    }
}
