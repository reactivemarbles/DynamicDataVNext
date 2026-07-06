using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

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

            [TestCase(1, TestName = "{m}(Single item)")]
            [TestCase(5, TestName = "{m}(Multiple items)")]
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
                result.Changes.Select(static change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "a removal change should have been generated for each removed item");
                result.Changes.Select(static change => change.Item).Should().BeEquivalentTo(removedItems, static options => options.WithStrictOrdering(), "a removal change should have been generated for each removed item");
                result.AsClear().Items.Should().BeEquivalentTo(removedItems, static config => config.WithStrictOrdering(), "all removed items should be listed");
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
                result.Changes.Length.Should().Be(addedItemCount + removedItemCount, "a change should have been generated for each removed item and added item");
                result.Changes.Take(removedItemCount).Select(static change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Removal, "a removal change should have been generated for each removed item");
                result.Changes.Skip(removedItemCount).Select(static change => change.Type).Should().AllBeEquivalentTo(DistinctChangeType.Addition, "an addition change should have been generated for each added item");
                result.Changes.Select(static change => change.Item).Should().BeEquivalentTo(Enumerable.Concat(removedItems, addedItems), static options => options.WithStrictOrdering(), "a change should have been generated for each removed item and each added item");
                result.AsReset().Removals.Should().BeEquivalentTo(removedItems, static config => config.WithStrictOrdering(), "all removed items should be listed");
                result.AsReset().Additions.Should().BeEquivalentTo(addedItems, static config => config.WithStrictOrdering(), "all added items should be listed");
            }

            protected abstract DistinctChangeSet<int> InvokeUut(
                IEnumerable<int> removedItems,
                IEnumerable<int> addedItems);
        }
    }
}
