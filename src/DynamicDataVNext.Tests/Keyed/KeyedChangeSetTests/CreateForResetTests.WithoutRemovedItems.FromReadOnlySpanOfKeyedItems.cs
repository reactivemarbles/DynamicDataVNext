namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForResetTests
{
    public static partial class WithoutRemovedItems
    {
        [TestFixture]
        public sealed class FromReadOnlySpanOfKeyedItems
            : Base
        {
            protected override KeyedChangeSet<int, int> InvokeUut(IEnumerable<int> addedItems)
                => KeyedChangeSet.CreateForReset(additions: addedItems.Select(SelectKeyedItem).ToArray().AsSpan());
        }
    }
}
