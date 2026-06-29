using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public static partial class CreateForInsertionTests
{
    [TestFixture]
    public sealed class FromOrderedItem
        : Base
    {
        protected override OrderedChangeSet<int> InvokeUut(
                int index,
                int item)
            => OrderedChangeSet.CreateForInsertion(new OrderedItem<int>()
            {
                Index   = index,
                Item    = item
            });
    }
}
