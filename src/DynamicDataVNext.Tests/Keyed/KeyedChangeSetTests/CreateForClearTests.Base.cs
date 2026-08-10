namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForClearTests
{
    public abstract class Base
        : KeyedChangeSetTests.Base
    {
        [Test]
        public void WhenNoItemsAreGiven_ResultIsEmpty()
        {
            var result = InvokeUut(items: Array.Empty<int>());
        
            result.Should().BeValid();
            result.Type.Should().Be(ChangeSetType.Empty, "an empty changeset should have been constructed");
        }

        [TestCase(1, TestName = "{m}(Single item)")]
        [TestCase(5, TestName = "{m}(Multiple items)")]
        public void Otherwise_ResultIsClear(int itemCount)
        {
            var items = Enumerable
                .Range(1, itemCount)
                .ToArray();

            var result = InvokeUut(items);
        
            result.Should().BeValid();
            result.Type.Should().Be(ChangeSetType.Clear, "a clear operation should have been constructed");
            result.Changes.Length.Should().Be(itemCount, "a removal change should have been generated for each removed item");
            result.Changes.Select(static change => change.Type).Should().AllBeEquivalentTo(KeyedChangeType.Removal, "a removal change should have been generated for each removed item");
            result.Changes.Select(static change => change.AsRemoval().Key).Should().BeEquivalentTo(items.Select(SelectKey), static options => options.WithStrictOrdering(), "the given item keys should have been embedded into the generated changes");
            result.Changes.Select(static change => change.AsRemoval().Item).Should().BeEquivalentTo(items, static options => options.WithStrictOrdering(), "the given items should have been embedded into the generated changes");
        }
        
        protected abstract KeyedChangeSet<int, int> InvokeUut(IEnumerable<int> items);
    }
}
