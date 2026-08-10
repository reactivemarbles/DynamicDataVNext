namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForRemovalsTests
{
    [TestFixture]
    public sealed class FromReadOnlySpanOfKeyValuePairs
        : Base
    {
        protected override KeyedChangeSet<int, int> InvokeUut(IEnumerable<int> items)
            => KeyedChangeSet.CreateForRemovals(removals: items.Select(SelectKeyValuePair).ToArray().AsSpan());
    }
}
