using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public static partial class CreateForReplacementTests
{
    [TestFixture]
    public sealed class FromOrderedReplacement
        : Base
    {
        protected override OrderedChangeSet<int> InvokeUut(
                int index,
                int oldItem,
                int newItem)
            => OrderedChangeSet.CreateForReplacement(new OrderedReplacement<int>()
            {
                Index   = index,
                OldItem = oldItem,
                NewItem  = newItem
            });
    }
}
