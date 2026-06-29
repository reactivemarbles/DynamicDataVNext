using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

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

            public static readonly IReadOnlyList<TestCaseData> Otherwise_TestCases
                = new[]
                {
                    new TestCaseData(1).SetName("{m}(Single item)"),
                    new TestCaseData(5).SetName("{m}(Multiple items)")
                };
            [TestCaseSource(nameof(Otherwise_TestCases))]
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
