namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForResetTests
{
    public static partial class WithoutRemovedItems
    {
        public abstract class Base
            : KeyedChangeSetTests.Base
        {
            [Test]
            public void WhenNoItemsAreGiven_ResultIsEmpty()
            {
                var result = InvokeUut(addedItems: Array.Empty<int>());
                
                result.Should().BeValid();
                result.Type.Should().Be(ChangeSetType.Empty, "an empty changeset should have been constructed");
            }

            [TestCase(1, TestName = "{m}(Single item)")]
            [TestCase(5, TestName = "{m}(Multiple items)")]
            public void Otherwise_ResultIsReset(int addedItemCount)
            {
                var addedItems = Enumerable
                    .Range(1, addedItemCount)
                    .ToArray();

                var result = InvokeUut(addedItems: addedItems);
                
                result.Should().BeValid();
                result.Type.Should().Be(ChangeSetType.Reset, "a reset operation should have been constructed");
                result.Changes.Length.Should().Be(addedItemCount, "an addition change should have been generated for each added item");
                result.Changes.Select(static change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "an addition change should have been generated for each added item");
                result.Changes.Select(static change => change.AsAddition().Key).Should().BeEquivalentTo(addedItems.Select(SelectKey), static options => options.WithStrictOrdering(), "the given item keys should have been embedded into the generated changes");
                result.Changes.Select(static change => change.AsAddition().Item).Should().BeEquivalentTo(addedItems, static options => options.WithStrictOrdering(), "the given items should have been embedded into the generated changes");
                result.AsReset().Removals.Should().BeEmpty("no removed items should have been added");
                result.AsReset().Additions.Should().BeEquivalentTo(addedItems.Select(SelectKeyedItem), static config => config.WithStrictOrdering(), "all items should have been listed as added");
            }
            
            protected abstract KeyedChangeSet<int, int> InvokeUut(IEnumerable<int> addedItems);
        }
    }
}        
