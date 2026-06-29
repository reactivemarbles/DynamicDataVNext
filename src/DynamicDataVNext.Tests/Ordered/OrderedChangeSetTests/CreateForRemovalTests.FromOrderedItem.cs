using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public static partial class CreateForRemovalTests
{
    [TestFixture]
    public sealed class FromOrderedItem
        : Base
    {
        protected override OrderedChangeSet<int> InvokeUut(
                int index,
                int item)
            => OrderedChangeSet.CreateForRemoval(new OrderedItem<int>()
            {
                Index   = index,
                Item    = item
            });
    }
}
