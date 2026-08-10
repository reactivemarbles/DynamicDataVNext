namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForClearTests
{
    [TestFixture]
    public sealed class FromReadOnlySpanOfKeyValuePairs
        : Base
    {
        protected override KeyedChangeSet<int, int> InvokeUut(IEnumerable<int> items)
            => KeyedChangeSet.CreateForClear(removals: items.Select(SelectKeyValuePair).ToArray().AsSpan());
    }
}
