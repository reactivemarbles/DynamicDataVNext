using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeTests;

public static partial class CreateReplacementTests
{
    [TestFixture]
    public sealed class FromOrderedReplacement
        : Base
    {
        protected override OrderedChange<int> InvokeUut(
                int index,
                int oldItem,
                int newItem)
            => OrderedChange.CreateReplacement(new OrderedReplacement<int>()
            {
                Index   = index,
                OldItem = oldItem,
                NewItem = newItem
            });
    }
}
