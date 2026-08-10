namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForRemovalsTests
{
    [TestFixture]
    public sealed class FromReadOnlySpanOfKeyedItems
        : Base
    {
        protected override KeyedChangeSet<int, int> InvokeUut(IEnumerable<int> items)
            => KeyedChangeSet.CreateForRemovals(removals: items.Select(SelectKeyedItem).ToArray().AsSpan());
    }
}
