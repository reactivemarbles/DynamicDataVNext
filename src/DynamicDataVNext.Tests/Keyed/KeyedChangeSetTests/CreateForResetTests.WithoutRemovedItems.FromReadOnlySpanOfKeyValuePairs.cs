namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForResetTests
{
    public static partial class WithoutRemovedItems
    {
        [TestFixture]
        public sealed class FromReadOnlySpanOfKeyValuePairs
            : Base
        {
            protected override KeyedChangeSet<int, int> InvokeUut(IEnumerable<int> addedItems)
                => KeyedChangeSet.CreateForReset(additions: addedItems.Select(SelectKeyValuePair).ToArray().AsSpan());
        }
    }
}
