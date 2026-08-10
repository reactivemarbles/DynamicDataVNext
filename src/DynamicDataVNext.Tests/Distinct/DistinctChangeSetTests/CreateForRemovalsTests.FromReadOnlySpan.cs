namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

public static partial class CreateForRemovalsTests
{
    [TestFixture]
    public sealed class FromReadOnlySpan
        : Base
    {
        protected override DistinctChangeSet<int> InvokeUut(IEnumerable<int> items)
            => DistinctChangeSet.CreateForRemovals(items.ToArray().AsSpan());
    }
}
