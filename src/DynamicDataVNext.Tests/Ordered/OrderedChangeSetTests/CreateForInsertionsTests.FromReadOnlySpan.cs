namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public static partial class CreateForInsertionsTests
{
    [TestFixture]
    public sealed class FromReadOnlySpan
        : Base
    {
        protected override OrderedChangeSet<int> InvokeUut(
                int                 index,
                IEnumerable<int>    items)
            => OrderedChangeSet.CreateForInsertions(
                index:  index,
                items:  items.ToArray().AsSpan());
    }
}
