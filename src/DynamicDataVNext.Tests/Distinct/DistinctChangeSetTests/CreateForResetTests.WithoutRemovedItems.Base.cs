using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

public static partial class CreateForResetTests
{
    public static partial class WithoutRemovedItems
    {
        public abstract class Base
        {
            [Test]
            public void WhenItemsIsEmpty_ResultIsEmpty()
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
                result.Changes.Select(static change => change.Item).Should().BeEquivalentTo(addedItems, static options => options.WithStrictOrdering(), "an addition change should have been generated for each added item");
                result.AsReset().Removals.Should().BeEmpty("no removed items should have been added");
                result.AsReset().Additions.Should().BeEquivalentTo(addedItems, static config => config.WithStrictOrdering(), "all items should have been listed as added");
            }
            
            protected abstract DistinctChangeSet<int> InvokeUut(IEnumerable<int> addedItems);
        }
    }
}        
