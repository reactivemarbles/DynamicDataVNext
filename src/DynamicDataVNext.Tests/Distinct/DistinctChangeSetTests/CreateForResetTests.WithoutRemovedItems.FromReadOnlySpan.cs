namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

public static partial class CreateForResetTests
{
    public static partial class WithoutRemovedItems
    {
        [TestFixture]
        public sealed class FromReadOnlySpan
            : Base
        {
            protected override DistinctChangeSet<int> InvokeUut(IEnumerable<int> addedItems)
                => DistinctChangeSet.CreateForReset(addedItems.ToArray().AsSpan());
        }
    }
}
