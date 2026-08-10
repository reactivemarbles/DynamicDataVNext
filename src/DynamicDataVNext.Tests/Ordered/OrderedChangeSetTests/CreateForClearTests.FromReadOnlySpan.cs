namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public partial class CreateForClearTests
{
    [TestFixture]
    public sealed class FromReadOnlySpan
        : Base
    {
        protected override OrderedChangeSet<int> InvokeUut(IReadOnlyList<int> items)
            => OrderedChangeSet.CreateForClear(items.ToArray().AsSpan());
    }
}
