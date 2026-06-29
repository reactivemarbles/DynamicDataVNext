using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeTests;

public static partial class CreateMovementTests
{
    [TestFixture]
    public sealed class FromOrderedMovement
        : Base
    {
        protected override OrderedChange<int> InvokeUut(
                int oldIndex,
                int newIndex,
                int item)
            => OrderedChange.CreateMovement(new OrderedMovement<int>()
            {
                OldIndex    = oldIndex,
                NewIndex    = newIndex,
                Item        = item
            });
    }
}
