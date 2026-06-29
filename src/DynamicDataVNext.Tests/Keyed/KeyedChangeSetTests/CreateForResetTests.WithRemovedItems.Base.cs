using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForResetTests
{
    public static partial class WithRemovedItems
    {
        public abstract class Base
            : KeyedChangeSetTests.Base
        {
            [Test]
            public void WhenNoItemsAreGiven_ResultIsEmpty()
            {
                var result = InvokeUut(
                    removedItems:   Array.Empty<int>(),
                    addedItems:     Array.Empty<int>());
                
                result.Should().BeValid();
                result.Type.Should().Be(ChangeSetType.Empty, "an empty changeset should have been constructed");
            }

            public static readonly IReadOnlyList<TestCaseData> WhenNoNewItemsAreGiven_TestCases
                = new[]
                {
                    new TestCaseData(1).SetName("{m}(Single Removal)"),
                    new TestCaseData(5).SetName("{m}(Multiple Removals)")
                };
            [TestCaseSource(nameof(WhenNoNewItemsAreGiven_TestCases))]
            public void WhenNoNewItemsAreGiven_ResultIsClear(int removedItemCount)
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
                result.Changes.Select(static change => change.Type).Should().AllBeEquivalentTo(KeyedChangeType.Removal, "a removal change should have been generated for each removed item");
                result.Changes.Select(static change => change.AsRemoval().Key).Should().BeEquivalentTo(removedItems.Select(SelectKey), static options => options.WithStrictOrdering(), "the given item keys should have been embedded into the generated changes");
                result.Changes.Select(static change => change.AsRemoval().Item).Should().BeEquivalentTo(removedItems, static options => options.WithStrictOrdering(), "the given items should have been embedded into the generated changes");
                result.AsClear().Items.Should().BeEquivalentTo(removedItems.Select(SelectKeyedItem), static config => config.WithStrictOrdering(), "all removed items should be listed");
            }

            public static readonly IReadOnlyList<TestCaseData> Otherwise_TestCases
                = new[]
                {
                    new TestCaseData(0, 1).SetName("{m}(No Removals, Single Addition)"),
                    new TestCaseData(0, 5).SetName("{m}(No Removals, Multiple Additions)"),
                    new TestCaseData(1, 1).SetName("{m}(Single Removal, Single Addition)"),
                    new TestCaseData(1, 5).SetName("{m}(Single Removal, Multiple Additions)"),
                    new TestCaseData(5, 5).SetName("{m}(Multiple Removals, Multiple Additions)")
                };
            [TestCaseSource(nameof(Otherwise_TestCases))]
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
                result.Changes.Take(removedItemCount).Select(static change => change.Type).Should().AllBeEquivalentTo(KeyedChangeType.Removal, "a removal change should have been generated for each removed item");
                result.Changes.Take(removedItemCount).Select(static change => change.AsRemoval().Key).Should().BeEquivalentTo(removedItems.Select(SelectKey), static options => options.WithStrictOrdering(), "the given item keys should have been embedded into the generated removal changes");
                result.Changes.Take(removedItemCount).Select(static change => change.AsRemoval().Item).Should().BeEquivalentTo(removedItems, static options => options.WithStrictOrdering(), "the given items should have been embedded into the generated removal changes");
                result.Changes.Skip(removedItemCount).Select(static change => change.Type).Should().AllBeEquivalentTo(KeyedChangeType.Addition, "an addition change should have been generated for each added item");
                result.Changes.Skip(removedItemCount).Select(static change => change.AsAddition().Key).Should().BeEquivalentTo(addedItems.Select(SelectKey), static options => options.WithStrictOrdering(), "the given item keys should have been embedded into the generated addition changes");
                result.Changes.Skip(removedItemCount).Select(static change => change.AsAddition().Item).Should().BeEquivalentTo(addedItems, static options => options.WithStrictOrdering(), "the given items should have been embedded into the generated addition changes");
                result.AsReset().Removals.Should().BeEquivalentTo(removedItems.Select(SelectKeyedItem), static config => config.WithStrictOrdering(), "all removed items should be listed");
                result.AsReset().Additions.Should().BeEquivalentTo(addedItems.Select(SelectKeyedItem), static config => config.WithStrictOrdering(), "all added items should be listed");
            }

            protected abstract KeyedChangeSet<int, int> InvokeUut(
                IEnumerable<int> removedItems,
                IEnumerable<int> addedItems);
        }
    }
}
