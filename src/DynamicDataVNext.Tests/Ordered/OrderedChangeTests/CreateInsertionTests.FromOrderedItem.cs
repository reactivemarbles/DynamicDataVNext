using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeTests;

public static partial class CreateInsertionTests
{
    [TestFixture]
    public sealed class FromOrderedItem
        : Base
    {
        protected override OrderedChange<int> InvokeUut(
                int index,
                int item)
            => OrderedChange.CreateInsertion(new OrderedItem<int>()
            {
                Index   = index,
                Item    = item
            });
    }
}
