namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public static partial class CreateForResetTests
{
    public static partial class WithRemovedItems
    {
        public abstract class Base
        {
            [Test]
            public void WhenRemovedItemsAndAddedItemsAreEmpty_ResultIsEmpty()
            {
                var result = InvokeUut(
                    removedItems:   Array.Empty<int>(),
                    addedItems:     Array.Empty<int>());
                
                result.Should().BeValid();
                result.Type.Should().Be(ChangeSetType.Empty, "an empty changeset should have been constructed");
            }

            [TestCase(1, TestName = "{m}(Single Removal)")]
            [TestCase(5, TestName = "{m}(Multiple Removals)")]
            public void WhenAddedItemsIsEmpty_ResultIsClear(int removedItemCount)
            {
                var removedItems = Enumerable
                    .Range(1, removedItemCount)
                    .ToArray();

                var result = InvokeUut(
                    removedItems:   removedItems,
                    addedItems:     Array.Empty<int>());
                
                result.Should().BeValid();
                result.Type.Should().Be(ChangeSetType.Clear, "a clear operation should have been constructed");
                result.Changes.Length.Should().Be(removedItemCount, "a removal change should have been generated for each removed item");
                result.Changes.Select(static change => change.Type).Should().AllBeEquivalentTo(OrderedChangeType.Removal, "a removal change should have been generated for each removed item");
                result.Changes.Select(static change => change.AsRemoval().Index).Should().BeEquivalentTo(Enumerable.Range(0, removedItemCount).Reverse(), static options => options.WithStrictOrdering(), "the changes should have been sequenced by index, in reverse order");
                result.Changes.Select(static change => change.AsRemoval().Item).Should().BeEquivalentTo(removedItems.Reverse(), static options => options.WithStrictOrdering(), "the given items should have been embedded into the generated changes, in reverse order");
                result.AsClear().ReversedItems.Should().BeEquivalentTo(removedItems.Reverse(), static options => options.WithStrictOrdering(), "all removed items should be listed, in reverse order");
            }

            [TestCase(0, 1, TestName = "{m}(No Removals, Single Addition)")]
            [TestCase(0, 5, TestName = "{m}(No Removals, Multiple Additions)")]
            [TestCase(1, 1, TestName = "{m}(Single Removal, Single Addition)")]
            [TestCase(1, 5, TestName = "{m}(Single Removal, Multiple Additions)")]
            [TestCase(5, 5, TestName = "{m}(Multiple Removals, Multiple Additions)")]
            public void Otherwise_ResultIsReset(
                int removedItemCount,
                int addedItemCount)
            {
                var removedItems = Enumerable
                    .Range(1, removedItemCount)
                    .ToArray();
                
                var addedItems = Enumerable
                    .Range(1 + removedItemCount, addedItemCount)
                    .ToArray();

                var result = InvokeUut(
                    removedItems:   removedItems,
                    addedItems:     addedItems);
                
                result.Should().BeValid();
                result.Type.Should().Be(ChangeSetType.Reset, "a reset operation should have been constructed");
                result.Changes.Length.Should().Be(addedItemCount + removedItemCount, "a change should have been generated for each removed item and each added item");
                result.Changes.Take(removedItemCount).Select(static change => change.Type).Should().AllBeEquivalentTo(OrderedChangeType.Removal, "a removal change should have been generated for each removed item");
                result.Changes.Take(removedItemCount).Select(static change => change.AsRemoval().Index).Should().BeEquivalentTo(Enumerable.Range(0, removedItemCount).Reverse(), static options => options.WithStrictOrdering(), "the removal changes should have been sequenced by index, in reverse order");
                result.Changes.Take(removedItemCount).Select(static change => change.AsRemoval().Item).Should().BeEquivalentTo(removedItems.Reverse(), static options => options.WithStrictOrdering(), "the given items should have been embedded into the generated removal changes, in reverse order");
                result.Changes.Skip(removedItemCount).Select(static change => change.Type).Should().AllBeEquivalentTo(OrderedChangeType.Insertion, "an insertion change should have been generated for each added item");
                result.Changes.Skip(removedItemCount).Select(static change => change.AsInsertion().Index).Should().BeEquivalentTo(Enumerable.Range(0, addedItemCount), static options => options.WithStrictOrdering(), "the removal changes should have been sequenced by index");
                result.Changes.Skip(removedItemCount).Select(static change => change.AsInsertion().Item).Should().BeEquivalentTo(addedItems, static options => options.WithStrictOrdering(), "the given items should have been embedded into the generated insertion changes");
                result.AsReset().ReversedRemovals.Should().BeEquivalentTo(removedItems.Reverse(), static config => config.WithStrictOrdering(), "all removed items should be listed, in reverse order");
                result.AsReset().Additions.Should().BeEquivalentTo(addedItems, static config => config.WithStrictOrdering(), "all added items should be listed");
            }

            protected abstract OrderedChangeSet<int> InvokeUut(
                IReadOnlyList<int>  removedItems,
                IEnumerable<int>    addedItems);
        }
    }
}
