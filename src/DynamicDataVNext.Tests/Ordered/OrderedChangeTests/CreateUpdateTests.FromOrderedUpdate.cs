using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeTests;

public static partial class CreateUpdateTests
{
    [TestFixture]
    public sealed class FromOrderedUpdate
        : Base
    {
        protected override OrderedChange<int> InvokeUut(
                int oldIndex,
                int oldItem,
                int newIndex,
                int newItem)
            => OrderedChange.CreateUpdate(new OrderedUpdate<int>()
            {
                OldIndex    = oldIndex,
                OldItem     = oldItem,
                NewIndex    = newIndex,
                NewItem     = newItem
            });
    }
}
