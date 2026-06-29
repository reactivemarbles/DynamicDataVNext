using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public static partial class CreateForMovementTests
{
    [TestFixture]
    public sealed class FromOrderedMovement
        : Base
    {
        protected override OrderedChangeSet<int> InvokeUut(
                int oldIndex,
                int newIndex,
                int item)
            => OrderedChangeSet.CreateForMovement(new OrderedMovement<int>()
            {
                OldIndex    = oldIndex,
                NewIndex    = newIndex,
                Item        = item
            });
    }
}
