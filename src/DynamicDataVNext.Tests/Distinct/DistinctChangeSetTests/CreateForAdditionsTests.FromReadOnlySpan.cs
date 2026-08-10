namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

public static partial class CreateForAdditionsTests
{
    [TestFixture]
    public sealed class FromReadOnlySpan
        : Base
    {
        protected override DistinctChangeSet<int> InvokeUut(IEnumerable<int> items)
            => DistinctChangeSet.CreateForAdditions(items.ToArray().AsSpan());
    }
}
