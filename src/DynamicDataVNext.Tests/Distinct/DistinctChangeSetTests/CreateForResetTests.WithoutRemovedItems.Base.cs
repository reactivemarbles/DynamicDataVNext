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
                result.Changes.Select(static change => change.Item).Should().BeEquivalentTo(addedItems, static options => options.WithStrictOrdering(), "an addition change should have been generated for each added item");
                result.AsReset().Removals.Should().BeEmpty("no removed items should have been added");
                result.AsReset().Additions.Should().BeEquivalentTo(addedItems, static config => config.WithStrictOrdering(), "all items should have been listed as added");
            }
            
            protected abstract DistinctChangeSet<int> InvokeUut(IEnumerable<int> addedItems);
        }
    }
}        
