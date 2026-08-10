namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForAdditionsTests
{
    [TestFixture]
    public sealed class FromReadOnlySpanOfKeyValuePairs
        : Base
    {
        protected override KeyedChangeSet<int, int> InvokeUut(IEnumerable<int> items)
            => KeyedChangeSet.CreateForAdditions(additions: items.Select(SelectKeyValuePair).ToArray().AsSpan());
    }
}
